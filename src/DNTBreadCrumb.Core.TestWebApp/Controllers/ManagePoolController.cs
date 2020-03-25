using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Controllers
{
    [BreadCrumb(Title = "Pool Manager", Order = 0, UseDefaultRouteUrl = true)]
    public class ManagePoolController : Controller
    {
        [BreadCrumb(Title = "List of pool", Order = 1)]
        public IActionResult NotIndex() // it's a default action here
        {
            return View();
        }

        [BreadCrumb(Title = "Create a pool", Order = 1)]
        public IActionResult Create()
        {
            return View();
        }
    }
}