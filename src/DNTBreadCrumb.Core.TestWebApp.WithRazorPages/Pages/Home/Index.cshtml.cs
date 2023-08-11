using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DNTBreadCrumb.Core.TestWebApp.WithRazorPages.Pages.Home;

public class IndexModel : PageModel
{
    public void OnGet()
    {
        this.AddBreadCrumb(new BreadCrumb
                           {
                               Title = "Wiki",
                               Url = "/wiki",
                               Order = 1,
                           });
        this.AddBreadCrumb(new BreadCrumb
                           {
                               Title = "Lab",
                               Url = "/lab",
                               Order = 2,
                           });
    }
}