DNTBreadCrumb.Core
=======
`DNTBreadCrumb.Core` Creates custom bread crumb definitions, based on Twitter Bootstrap 3.x features for ASP.NET Core applications.



Install via NuGet
-----------------
To install DNTBreadCrumb.Core, run the following command in the Package Manager Console:

```
PM> Install-Package DNTBreadCrumb.Core
```

You can also view the [package page](http://www.nuget.org/packages/DNTBreadCrumb.Core/) on NuGet.



Usage:
-----------------
- After installing the DNTBreadCrumb.Core package, add the following definition to the [_ViewImports.cshtml](/src/DNTBreadCrumb.Core.TestWebApp/Views/_ViewImports.cshtml) file:
```csharp
@addTagHelper *, DNTBreadCrumb.Core
```

- Then modify the [_Layout.cshtml](/src/DNTBreadCrumb.Core.TestWebApp/Views/Shared/_Layout.cshtml) file to add its new tag-helper:
```xml
 <breadcrumb asp-homepage-title="Home"
             asp-homepage-url="@Url.Action("Index", "Home")"
             asp-homepage-glyphicon="glyphicon glyphicon-home"></breadcrumb>
```


- Now you can add the `BreadCrumb` attributes to your controller or action methods:
```csharp
[BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0)]
public class HomeController : Controller
{
   [BreadCrumb(Title = "Main index", Order = 1)]
   public ActionResult Index()
   {
      return View();
   }
```
Please follow the [DNTBreadCrumb.Core.TestWebApp](/src/DNTBreadCrumb.Core.TestWebApp) sample for more scenarios.