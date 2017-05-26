using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Controllers
{
    [BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        [BreadCrumb(Title = "Main index", Order = 1)]
        public ActionResult Index()
        {
            return View();
        }

        [BreadCrumb(Title = "Posts List", Order = 3)]
        public ActionResult Posts()
        {
            this.SetCurrentBreadCrumbTitle("dynamic title 1");

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Wiki",
                Url = string.Format("{0}?id=1", Url.Action("Index", "Home", values: new { area = "" })),
                Order = 1
            });
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Lab",
                Url = string.Format("{0}?id=2", Url.Action("Index", "Home", values: new { area = "" })),
                Order = 2
            });

            return View();
        }

        [BreadCrumb(TitleResourceName = "LocalizedPosts",
                    TitleResourceType = typeof(Resources.Controllers_HomeController),
                    Order = 1)]
        public ActionResult LocalizedPosts()
        {
            return View();
        }

        public IActionResult SetFaLanguage()
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("fa-IR"))),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("LocalizedPosts");
        }
    }
}