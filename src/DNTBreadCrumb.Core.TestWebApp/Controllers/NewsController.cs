using Microsoft.AspNetCore.Mvc;

namespace DNTBreadCrumb.Core.TestWebApp.Controllers
{
    [BreadCrumb(Title = "News Root", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true,
        Order = 0, GlyphIcon = "glyphicon glyphicon-link")]
    public class NewsController : Controller
    {

        [BreadCrumb(Title = "Main index", Order = 1)]
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                this.SetCurrentBreadCrumbTitle(id);
            }

            return View();
        }

        [BreadCrumb(Title = "News Archive", Order = 2)]
        public ActionResult Archive(int? id)
        {
            if (id != null)
            {
                this.SetCurrentBreadCrumbTitle(string.Format("News item {0}", id.Value));
                this.AddBreadCrumb(new BreadCrumb
                {
                    Title = "News Archive",
                    Order = 1,
                    Url = Url.Action("Archive", "News", values: new { id = "", area = "" })
                });
            }

            return View();
        }
    }
}