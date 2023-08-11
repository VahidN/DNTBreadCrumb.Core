using System;
using System.Globalization;
using DNTBreadCrumb.Core.TestWebApp.WithFeatureFolders.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.WithFeatureFolders.Features.Home;

[BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0, IgnoreAjaxRequests = true)]
public class HomeController : Controller
{
    [BreadCrumb(Title = "Main index", Order = 1, IgnoreAjaxRequests = true)]
    public ActionResult Index() => View();

    [BreadCrumb(Title = "Posts List", Order = 3, IgnoreAjaxRequests = true)]
    public ActionResult Posts()
    {
        this.SetCurrentBreadCrumbTitle("dynamic title 1");

        this.AddBreadCrumb(new BreadCrumb
                           {
                               Title = "Wiki",
                               Url = string.Format("{0}?id=1", Url.Action("Index", "Home", new { area = "" })),
                               Order = 1,
                           });
        this.AddBreadCrumb(new BreadCrumb
                           {
                               Title = "Lab",
                               Url = string.Format("{0}?id=2", Url.Action("Index", "Home", new { area = "" })),
                               Order = 2,
                           });

        return View();
    }

    [BreadCrumb(TitleResourceName = "LocalizedPosts",
                   TitleResourceType = typeof(Controllers_HomeController),
                   Order = 1,
                   IgnoreAjaxRequests = true)]
    public ActionResult LocalizedPosts() => View();

    public IActionResult SetFaLanguage()
    {
        Response.Cookies.Append(
                                CookieRequestCultureProvider.DefaultCookieName,
                                CookieRequestCultureProvider
                                    .MakeCookieValue(new RequestCulture(new CultureInfo("fa-IR"))),
                                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                               );

        return RedirectToAction("LocalizedPosts");
    }
}