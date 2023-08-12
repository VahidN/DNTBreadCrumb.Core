using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Controllers;

[BreadCrumb(Title = "News Root",
               UseDefaultRouteUrl = true,
               RemoveAllDefaultRouteValues = true,
               Order = 0,
               GlyphIcon = "bi bi-link-45deg",
               IgnoreAjaxRequests = true)]
public class NewsController : Controller
{
    [BreadCrumb(Title = "Main index", Order = 1, IgnoreAjaxRequests = true)]
    public ActionResult Index(string id)
    {
        if (!string.IsNullOrWhiteSpace(id))
        {
            this.SetCurrentBreadCrumbTitle(id);
        }

        return View();
    }

    [BreadCrumb(Title = "News Archive", Order = 2, IgnoreAjaxRequests = true)]
    public ActionResult Archive(int? id)
    {
        if (id != null)
        {
            this.SetCurrentBreadCrumbTitle($"News item {id.Value}");
            this.AddBreadCrumb(new BreadCrumb
                               {
                                   Title = "News Archive",
                                   Order = 1,
                                   Url = Url.Action("Archive", "News", new { id = "", area = "" }),
                               });
        }

        return View();
    }
}