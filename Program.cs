using DotNetEnv;

DotNetEnv.Env.Load(); // ⬅️ Load .env sớm nhất có thể, trước builder

var builder = WebApplication.CreateBuilder(args);

// (Debug) In ra để kiểm tra đã load thành công chưa
Console.WriteLine("ENV connStr: " + Environment.GetEnvironmentVariable("ORACLE_CONN_STRING"));

// Add services to the container.
builder.Services.AddControllersWithViews();
// Gọi test kết nối
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Intro}/{id?}");

app.MapControllerRoute(
    name: "khachhang_route",
    pattern: "khachhang",
    defaults: new { controller = "Home", action = "Khachhang" });

app.MapControllerRoute(
    name: "thucung_route",
    pattern: "thucung",
    defaults: new { controller = "Home", action = "Thucung" });
app.MapControllerRoute(
    name: "sanpham_route",
    pattern: "sanpham",
    defaults: new { controller = "Home", action = "Sanpham" });
app.MapControllerRoute(
    name: "hoadon_route",
    pattern: "hoadon",
    defaults: new { controller = "Home", action = "Hoadon" });
app.MapControllerRoute(
    name: "hoadonchitiet_route",
    pattern: "hoadonchitiet",
    defaults: new { controller = "Home", action = "Hoadonchitiet" });
app.Run();
