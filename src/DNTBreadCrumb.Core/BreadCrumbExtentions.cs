using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.RazorPages;
#endif

namespace DNTBreadCrumb.Core
{
    /// <summary>
    /// BreadCrumb Extentions
    /// </summary>
    public static class BreadCrumbExtentions
    {
        /// <summary>
        /// The key value of the current item in the ctx.Items
        /// </summary>
        public const string CurrentBreadCrumbKey = "Current_BreadCrumb_Key";

        /// <summary>
        /// Clears the stack of the current items
        /// </summary>
        /// <param name="ctx"></param>
        public static bool ClearBreadCrumbs(this HttpContext ctx)
        {
            if (ctx == null)
            {
                return false;
            }

            ctx.Items[CurrentBreadCrumbKey] = new List<BreadCrumb>();

            return true;
        }

        /// <summary>
        /// Clears the stack of the current items
        /// </summary>
        /// <param name="ctx"></param>
        public static bool ClearBreadCrumbs(this Controller ctx)
        {
            return ctx.HttpContext.ClearBreadCrumbs();
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Clears the stack of the current items
        /// </summary>
        public static bool ClearBreadCrumbs(this PageModel pageModel)
        {
           return pageModel.HttpContext.ClearBreadCrumbs();
        }
#endif

        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumb"></param>
        public static bool AddBreadCrumb(this HttpContext ctx, BreadCrumb breadCrumb)
        {
            if (ctx == null)
            {
                return false;
            }

            var currentBreadCrumbs = ctx.Items[CurrentBreadCrumbKey] as List<BreadCrumb> ?? new List<BreadCrumb>();
            if (currentBreadCrumbs.Any(crumb => crumb.Url.Equals(breadCrumb.Url, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            currentBreadCrumbs.Add(breadCrumb);
            ctx.Items[CurrentBreadCrumbKey] = currentBreadCrumbs;

            return true;
        }

        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumb"></param>
        public static bool AddBreadCrumb(this Controller ctx, BreadCrumb breadCrumb)
        {
            if (ctx == null)
            {
                return false;
            }

            ctx.HttpContext.AddBreadCrumb(breadCrumb);

            return true;
        }

        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbAction"></param>
        public static bool AddBreadCrumb(this Controller ctx, Action<BreadCrumb> breadCrumbAction)
        {
            var breadCrumb = new BreadCrumb();
            breadCrumbAction(breadCrumb);
            return ctx.AddBreadCrumb(breadCrumb);
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        public static bool AddBreadCrumb(this PageModel pageModel, BreadCrumb breadCrumb)
        {
            pageModel.HttpContext.AddBreadCrumb(breadCrumb);
            return true;
        }

        /// <summary>
        /// Adds a custom bread crumb to the list
        /// </summary>
        public static bool AddBreadCrumb(this PageModel pageModel, Action<BreadCrumb> breadCrumbAction)
        {
            var breadCrumb = new BreadCrumb();
            breadCrumbAction(breadCrumb);
            return pageModel.AddBreadCrumb(breadCrumb);
        }
#endif

        /// <summary>
        /// Sets the specified item's title
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        public static bool SetBreadCrumbTitle(this HttpContext ctx, string url, string title)
        {
            if (ctx == null)
            {
                return false;
            }

            var currentBreadCrumbs = ctx.Items[CurrentBreadCrumbKey] as List<BreadCrumb> ?? new List<BreadCrumb>();
            var breadCrumb = currentBreadCrumbs.FirstOrDefault(crumb => crumb.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
            if (breadCrumb == null)
            {
                return false;
            }

            breadCrumb.Title = title;
            ctx.Items[CurrentBreadCrumbKey] = currentBreadCrumbs;

            return true;
        }

        /// <summary>
        /// Sets the specified item's title
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        public static bool SetBreadCrumbTitle(this Controller ctx, string url, string title)
        {
            return ctx.HttpContext.SetBreadCrumbTitle(url, title);
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Sets the specified item's title
        /// </summary>
        public static bool SetBreadCrumbTitle(this PageModel pageModel, string url, string title)
        {
            return pageModel.HttpContext.SetBreadCrumbTitle(url, title);
        }
#endif

        /// <summary>
        /// Sets the current item's title. It's useful for changing the title of the current action method's bread crumb dynamically.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="title"></param>
        public static bool SetCurrentBreadCrumbTitle(this HttpContext ctx, string title)
        {
            return ctx.ModifyCurrentBreadCrumb(b => b.Title = title);
        }

        /// <summary>
        /// Sets the current item's title. It's useful for changing the title of the current action method's bread crumb dynamically.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="title"></param>
        public static bool SetCurrentBreadCrumbTitle(this Controller ctx, string title)
        {
            return ctx.HttpContext.SetCurrentBreadCrumbTitle(title);
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Sets the current item's title. It's useful for changing the title of the current action method's bread crumb dynamically.
        /// </summary>
        public static bool SetCurrentBreadCrumbTitle(this PageModel pageModel, string title)
        {
            return pageModel.HttpContext.SetCurrentBreadCrumbTitle(title);
        }
#endif

        /// <summary>
        ///     Modifies the current bread crumb
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbAction"></param>
        public static bool ModifyCurrentBreadCrumb(this HttpContext ctx, Action<BreadCrumb> breadCrumbAction)
        {
            if (ctx == null)
            {
                return false;
            }

            var url = ctx.Request.GetEncodedUrl();
            var currentBreadCrumbs = ctx.Items[CurrentBreadCrumbKey] as List<BreadCrumb> ??
                                     new List<BreadCrumb>();
            var breadCrumb = currentBreadCrumbs.FirstOrDefault(crumb =>
                crumb.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
            if (breadCrumb == null)
            {
                return false;
            }

            breadCrumbAction(breadCrumb);
            ctx.Items[CurrentBreadCrumbKey] = currentBreadCrumbs;

            return true;
        }

        /// <summary>
        ///     Modifies the current bread crumb
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbAction"></param>
        public static bool ModifyCurrentBreadCrumb(this Controller ctx, Action<BreadCrumb> breadCrumbAction)
        {
            return ctx.HttpContext.ModifyCurrentBreadCrumb(breadCrumbAction);
        }

#if NETSTANDARD2_0
        /// <summary>
        ///     Modifies the current bread crumb
        /// </summary>
        public static bool ModifyCurrentBreadCrumb(this PageModel pageModel, Action<BreadCrumb> breadCrumbAction)
        {
            return pageModel.HttpContext.ModifyCurrentBreadCrumb(breadCrumbAction);
        }
#endif

        /// <summary>
        /// Returns all the breadcrumbs
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbFilter"></param>
        public static IEnumerable<BreadCrumb> GetBreadCrumbs(this HttpContext ctx, Predicate<BreadCrumb> breadCrumbFilter = null)
        {
            if (ctx == null)
            {
                return Enumerable.Empty<BreadCrumb>();
            }

            var currentBreadCrumbs = ctx.Items[CurrentBreadCrumbKey] as List<BreadCrumb> ??
                                     new List<BreadCrumb>();

            return currentBreadCrumbs.Where(breadCrumb => breadCrumbFilter == null || breadCrumbFilter(breadCrumb));
        }

        /// <summary>
        /// Returns all the breadcrumbs
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="breadCrumbFilter"></param>
        public static IEnumerable<BreadCrumb> GetBreadCrumbs(this Controller ctx, Predicate<BreadCrumb> breadCrumbFilter = null)
        {
            return ctx.HttpContext.GetBreadCrumbs(breadCrumbFilter);
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Returns all the breadcrumbs
        /// </summary>
        public static IEnumerable<BreadCrumb> GetBreadCrumbs(this PageModel pageModel, Predicate<BreadCrumb> breadCrumbFilter = null)
        {
            return pageModel.HttpContext.GetBreadCrumbs(breadCrumbFilter);
        }
#endif
    }
}