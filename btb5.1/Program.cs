using btb5._1.Repositories;
using Microsoft.EntityFrameworkCore;
using btb5._1.Models;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Đăng ký các dịch vụ vào container
// =============================

// 1. Đăng ký DbContext và cấu hình kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true;                // Bảo mật cookie
    options.Cookie.IsEssential = true;             // Cookie là cần thiết
});

// 3. Đăng ký Dependency Injection cho Repository
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// 4. Đăng ký Controller và Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =============================
// Cấu hình Middleware pipeline
// =============================

// 1. Xử lý lỗi cho môi trường Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. Middleware cơ bản
app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS
app.UseStaticFiles();      // Phục vụ file tĩnh như CSS, JS, images

// 3. Kích hoạt Session và định tuyến
app.UseRouting();    // Middleware định tuyến
app.UseSession();    // Kích hoạt Session
app.UseAuthorization(); // Middleware xác thực quyền

// 4. Cấu hình định tuyến cho các Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Đặt Login là trang mặc định

// 5. Chạy ứng dụng
app.Run();
