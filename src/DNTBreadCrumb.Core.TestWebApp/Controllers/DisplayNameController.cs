using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("DisplayName Controller")]
    public class DisplayNameController:Controller
    {
        [BreadCrumb(Order = 1)]
        [Description("Main index")]
        public ActionResult Index()
        {
            return View();
        }

        [BreadCrumb(Order = 1)]
        [Description("Site Reports")]
        public ActionResult Reports()
        {
            return View(viewName: "Index");
        }
    }
}