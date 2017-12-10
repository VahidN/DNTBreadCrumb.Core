using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Sets the current item's title. It's useful for changing the title of the current action method's bread crumb dynamically.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="title"></param>
        public static bool SetCurrentBreadCrumbTitle(this HttpContext ctx, string title)
        {
            if (ctx == null)
            {
                return false;
            }

            var url = ctx.Request.GetEncodedUrl();
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
        /// Sets the current item's title. It's useful for changing the title of the current action method's bread crumb dynamically.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="title"></param>
        public static bool SetCurrentBreadCrumbTitle(this Controller ctx, string title)
        {
            return ctx.HttpContext.SetCurrentBreadCrumbTitle(title);
        }
    }
}