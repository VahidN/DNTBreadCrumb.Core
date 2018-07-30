using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace DNTBreadCrumb.Core
{
    /// <summary>
    /// BreadCrumb Action Filter. It can be applied to action methods or controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class BreadCrumbAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Use this property to remove all of the previous items of the current stack
        /// </summary>
        public bool ClearStack { get; set; }

        /// <summary>
        /// An optional glyph icon of the current item
        /// </summary>
        public string GlyphIcon { get; set; }

        /// <summary>
        /// If UseDefaultRouteUrl is set to true, this property indicated all of the route items should be removed from the final URL
        /// </summary>
        public bool RemoveAllDefaultRouteValues { get; set; }

        /// <summary>
        /// If UseDefaultRouteUrl is set to true, this property indicated which route items should be removed from the final URL
        /// </summary>
        public string[] RemoveRouteValues { get; set; }

        /// <summary>
        /// Title of the current item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the resource name (property name) to use as the key for lookups on the resource type.
        /// </summary>
        public string TitleResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource type to use for title lookups.
        /// </summary>
        public Type TitleResourceType { get; set; }

        /// <summary>
        /// A constant URL of the current item. If UseDefaultRouteUrl is set to true, its value will be ignored
        /// </summary>
        public string Url { get; set; }

        public bool UseAbsoluteUrl { get; set; }

        /// <summary>
        /// This property is useful for controller level bread crumbs. If it's true, the Url value will be calculated automatically from the DefaultRoute
        /// </summary>
        public bool UseDefaultRouteUrl { get; set; }

        /// <summary>
        /// This property is useful when you need a back functionality. If it's true, the Url value will be previous Url using UrlReferrer
        /// </summary>
        public bool UsePreviousUrl { get; set; }

        /// <summary>
        /// Disables the breadcrumb for Ajax requests. Its default value is true.
        /// </summary>
        public bool IgnoreAjaxRequests { get; set; } = true;

        /// <summary>
        /// Adds the current item to the stack
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IgnoreAjaxRequests && isAjaxRequest(filterContext))
            {
                return;
            }

            if (ClearStack)
            {
                filterContext.HttpContext.ClearBreadCrumbs();
            }

            var url = string.IsNullOrWhiteSpace(Url) ? filterContext.HttpContext.Request.GetEncodedUrl() : Url;

            var request = filterContext.HttpContext.Request;
            var scheme = request.Scheme;
            var hostName = request.Host.Host;
            var port = request.Host.Port;

            if (UseAbsoluteUrl)
            {
                url = new UriBuilder(scheme, hostName, port ?? 80, url).ToString();
            }

            if (UseDefaultRouteUrl)
            {
                url = getDefaultControllerActionUrl(filterContext);
            }

            if (UsePreviousUrl)
            {
                url = filterContext.HttpContext.Request.Headers["Referrer"];
            }

            setEmptyTitleFromResources();
            setEmptyTitleFromAttributes(filterContext);

            filterContext.HttpContext.AddBreadCrumb(new BreadCrumb
            {
                Url = url,
                Title = Title,
                Order = Order,
                GlyphIcon = GlyphIcon
            });

            base.OnActionExecuting(filterContext);
        }

        private static bool isAjaxRequest(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            return request?.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private string getDefaultControllerActionUrl(ActionExecutingContext filterContext)
        {
            var defaultRoute = filterContext.RouteData.Routers.OfType<Route>().FirstOrDefault();
            if (defaultRoute == null)
            {
                throw new InvalidOperationException("The default route of this controller not found.");
            }

            var defaultAction = defaultRoute.Defaults["action"] as string;
            if (defaultAction == null)
            {
                throw new InvalidOperationException("The default action of this controller not found.");
            }

            if (RemoveAllDefaultRouteValues)
            {
                return new UrlHelper(filterContext).ActionWithoutRouteValues(defaultAction);
            }

            if (RemoveRouteValues == null || !RemoveRouteValues.Any())
            {
                return new UrlHelper(filterContext).Action(defaultAction);
            }

            return new UrlHelper(filterContext).ActionWithoutRouteValues(defaultAction, RemoveRouteValues);
        }

        private void setEmptyTitleFromAttributes(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return;
            }

            var descriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
            {
                return;
            }

            var currentFilter = filterContext.ActionDescriptor
                                             .FilterDescriptors
                                             .Select(filterDescriptor => filterDescriptor)
                                             .FirstOrDefault(filterDescriptor => ReferenceEquals(filterDescriptor.Filter, this));
            if (currentFilter == null)
            {
                return;
            }

            MemberInfo typeInfo = null;

            if (currentFilter.Scope == FilterScope.Action)
            {
                typeInfo = descriptor.MethodInfo;
            }
            if (currentFilter.Scope == FilterScope.Controller)
            {
                typeInfo = descriptor.ControllerTypeInfo;
            }

            if (typeInfo == null)
            {
                return;
            }

            Title = typeInfo.GetCustomAttribute<DisplayNameAttribute>(inherit: true)?.DisplayName;
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = typeInfo.GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
            }
        }

        private void setEmptyTitleFromResources()
        {
            if (string.IsNullOrWhiteSpace(TitleResourceName) || TitleResourceType == null)
            {
                return;
            }

            var property = TitleResourceType.GetTypeInfo().GetDeclaredProperty(TitleResourceName);
            if (property != null)
            {
                var propertyGetter = property.GetMethod;

                // We only support internal and public properties
                if (propertyGetter == null || (!propertyGetter.IsAssembly && !propertyGetter.IsPublic))
                {
                    // Set the property to null so the exception is thrown as if the property wasn't found
                    property = null;
                }
            }

            if (property == null)
            {
                throw new InvalidOperationException($"ResourceType `{TitleResourceType.FullName}` does not have the `{TitleResourceName}` property.");
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException($"`{TitleResourceType.FullName}.{TitleResourceName}` property is not an string.");
            }

            Title = (string)property.GetValue(null, null);
        }
    }
}