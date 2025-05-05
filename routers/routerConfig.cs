using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MVC.router
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Intro}/{id?}");

            endpoints.MapControllerRoute(
                name: "khachhang_route",
                pattern: "khachhang",
                defaults: new { controller = "Customer", action = "Index" });

            endpoints.MapControllerRoute(
                name: "thucung_route",
                pattern: "thucung",
                defaults: new { controller = "Pet", action = "Index" });

            endpoints.MapControllerRoute(
                name: "sanpham_route",
                pattern: "sanpham",
                defaults: new { controller = "Product", action = "Index" });

            endpoints.MapControllerRoute(
                name: "hoadon_route",
                pattern: "hoadon",
                defaults: new { controller = "Order", action = "Index" });

        }
    }
}
