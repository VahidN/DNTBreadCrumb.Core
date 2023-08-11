using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[BreadCrumb(Title = "Admin, Home", UseDefaultRouteUrl = true, Order = 0, IgnoreAjaxRequests = true)]
public class HomeController : Controller
{
    [BreadCrumb(Title = "Index", Order = 1, IgnoreAjaxRequests = true)]
    public IActionResult Index() => View();
}