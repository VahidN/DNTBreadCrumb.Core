﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DNTBreadCrumb.Core;

/// <summary>
///     BreadCrumb TagHelper
/// </summary>
[HtmlTargetElement("breadcrumb")]
public class BreadCrumbTagHelper : TagHelper
{
    /// <summary>
    ///     Such as 'Home' or 'خانه'
    /// </summary>
    [HtmlAttributeName("asp-homepage-title")]
    public string? HomePageTitle { set; get; }

    /// <summary>
    ///     Such as @Url.Action("Index", "Home")
    /// </summary>
    [HtmlAttributeName("asp-homepage-url")]
    public string? HomePageUrl { set; get; }

    /// <summary>
    ///     such as `bi bi-house`
    /// </summary>
    [HtmlAttributeName("asp-homepage-glyphicon")]
    public string? HomePageGlyphIcon { set; get; }

    /// <summary>
    ///     Set version of bootstrap that you are using
    /// </summary>
    [HtmlAttributeName("asp-bootstrap-version")]
    public BootstrapVersion BootstrapVersion { get; set; } = BootstrapVersion.V3;

    /// <summary>
    /// </summary>
    protected HttpRequest? Request => ViewContext?.HttpContext.Request;

    /// <summary>
    /// </summary>
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="output"></param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (output == null)
        {
            throw new ArgumentNullException(nameof(output));
        }

        var breadCrumbs = ViewContext?.HttpContext.Items[BreadCrumbExtentions.CurrentBreadCrumbKey] as List<BreadCrumb>;
        if (breadCrumbs == null || !breadCrumbs.Any())
        {
            return;
        }

        var currentFullUrl = Request?.GetEncodedUrl();
        var currentRouteUrl = getCurrentRouteUrl();
        var isCurrentPageHomeUrl = !string.IsNullOrWhiteSpace(HomePageUrl) &&
                                   (HomePageUrl.Equals(currentFullUrl, StringComparison.OrdinalIgnoreCase) ||
                                    HomePageUrl.Equals(currentRouteUrl, StringComparison.OrdinalIgnoreCase));


        output.TagName = "ol";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "breadcrumb");

        if (isCurrentPageHomeUrl)
        {
            var itemBuilder = new TagBuilder("li");
            itemBuilder.AddCssClass("active");
            if (BootstrapVersion is BootstrapVersion.V4 or BootstrapVersion.V5)
            {
                itemBuilder.AddCssClass("breadcrumb-item");
            }

            itemBuilder.InnerHtml.AppendHtml(
                                             $"<span class='{HomePageGlyphIcon}' aria-hidden='true'></span> {HomePageTitle}");
            output.Content.AppendHtml(itemBuilder);
        }
        else
        {
            var itemBuilder = new TagBuilder("li");
            if (BootstrapVersion is BootstrapVersion.V4 or BootstrapVersion.V5)
            {
                itemBuilder.AddCssClass("breadcrumb-item");
            }

            itemBuilder.InnerHtml.AppendHtml(
                                             $"<a href='{HomePageUrl}'><span class='{HomePageGlyphIcon}' aria-hidden='true'></span> {HomePageTitle}</a>");
            output.Content.AppendHtml(itemBuilder);
        }

        foreach (var node in breadCrumbs.OrderBy(x => x.Order))
        {
            if (node.Url != null && node.Url.Equals(HomePageUrl, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (node.Url == null ||
                (!node.ForceUrl &&
                 (node.Url.Equals(currentFullUrl, StringComparison.OrdinalIgnoreCase) ||
                  node.Url.Equals(currentRouteUrl, StringComparison.OrdinalIgnoreCase))))
            {
                var itemBuilder = new TagBuilder("li");
                itemBuilder.AddCssClass("active");
                if (BootstrapVersion is BootstrapVersion.V4 or BootstrapVersion.V5)
                {
                    itemBuilder.AddCssClass("breadcrumb-item");
                }

                if (!string.IsNullOrWhiteSpace(node.GlyphIcon))
                {
                    itemBuilder.InnerHtml.AppendHtml(
                                                     $"<span class='{node.GlyphIcon}' aria-hidden='true'></span> ");
                }

                itemBuilder.InnerHtml.AppendHtml($"{node.Title}");
                output.Content.AppendHtml(itemBuilder);
            }
            else
            {
                var itemBuilder = new TagBuilder("li");
                if (BootstrapVersion is BootstrapVersion.V4 or BootstrapVersion.V5)
                {
                    itemBuilder.AddCssClass("breadcrumb-item");
                }

                itemBuilder.InnerHtml.AppendHtml($"<a href='{node.Url}'>");
                if (!string.IsNullOrWhiteSpace(node.GlyphIcon))
                {
                    itemBuilder.InnerHtml.AppendHtml(
                                                     $"<span class='{node.GlyphIcon}' aria-hidden='true'></span> ");
                }

                itemBuilder.InnerHtml.AppendHtml($"{node.Title}");
                itemBuilder.InnerHtml.AppendHtml("</a>");
                output.Content.AppendHtml(itemBuilder);
            }
        }
    }

    private string? getCurrentRouteUrl()
    {
        var routeValues = ViewContext?.ActionDescriptor.RouteValues;
        if (routeValues is null)
        {
            return string.Empty;
        }

        if (routeValues.TryGetValue("action", out var action))
        {
            var urlHelper = ViewContext?.HttpContext.Items.Values.OfType<IUrlHelper>().FirstOrDefault();
            if (urlHelper == null)
            {
                throw new InvalidOperationException("Failed to find the IUrlHelper of the ViewContext.HttpContext.");
            }

            return urlHelper.Action(action);
        }

        if (routeValues.TryGetValue("page", out var page))
        {
            return page;
        }

        return string.Empty;
    }
}