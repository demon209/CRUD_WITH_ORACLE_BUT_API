using DotNetEnv;
using MVC.router;
using MVC.Services;

DotNetEnv.Env.Load(); // Load .env sớm nhất có thể, trước builder

var builder = WebApplication.CreateBuilder(args);

// (Debug) In ra để kiểm tra đã load thành công chưa
Console.WriteLine("ENV connStr: " + Environment.GetEnvironmentVariable("ORACLE_CONN_STRING"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerPetService, CustomerPetService>();
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


// Sử dụng UseEndpoints để đăng ký các routes
app.UseEndpoints(endpoints =>
{
    RouteConfig.RegisterRoutes(endpoints); 

    
});

app.Run();
