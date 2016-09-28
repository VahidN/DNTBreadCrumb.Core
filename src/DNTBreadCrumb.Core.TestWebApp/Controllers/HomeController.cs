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

        [BreadCrumb(Title = "Posts list", Order = 3)]
        public ActionResult Posts()
        {
            this.SetCurrentBreadCrumbTitle("dynamic title 1");

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Wiki",
                Url = string.Format("{0}?id=1", Url.Action("Index", "Home")),
                Order = 1
            });
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Lab",
                Url = string.Format("{0}?id=2", Url.Action("Index", "Home")),
                Order = 2
            });

            return View();
        }
    }
}