using ASMC5.data;
using ASMC5.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ASMDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});
builder.Services.AddHttpClient();


// cau hinh identity
builder.Services.AddIdentity<User, Position>()
                .AddEntityFrameworkStores<ASMDBContext>()
    .AddSignInManager<SignInManager<User>>();

builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;

});
//
var sessionTimes = new Dictionary<string, TimeSpan>()
{
    {"User", new TimeSpan(0, 30, 0)}, // 30p
};


// Đăng ký session vào dịch vụ
builder.Services.AddSession(options =>
{
    foreach (var key in sessionTimes.Keys)
    {
        TimeSpan duration;
        if (sessionTimes.TryGetValue(key, out duration))
        {
            options.IdleTimeout = duration;
        }
    }
});

// cau hinh google

builder.Services.AddAuthentication(options =>
{
    // DefaultAuthenticateScheme: Đây là Scheme sẽ được sử dụng để xác thực yêu cầu đã được xác thực thành công.
    // Trong trường hợp này,CookieAuthenticationDefaults.AuthenticationScheme  được sử dụng, điều này có nghĩa là sử dụng xác thực bằng cookie.
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

 // cau hinh cookie
 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
 {
     options.Cookie.Name = "User";
     options.LoginPath = "/Account/Login";
     options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Thiết lập chính sách bảo mật
     options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thiết lập thời gian sống của cookie
     options.SlidingExpiration = true; // Cho phép cookie được cập nhật thời gian sống mỗi khi có request mới
     options.Cookie.HttpOnly = true;
 });

builder.Services.AddAuthentication().AddGoogle(option =>
{
    var ggConfig = builder.Configuration.GetSection("Authentication:Google");
    option.ClientId = ggConfig["ClientId"];
    option.ClientSecret = ggConfig["ClientSecret"];
    option.CallbackPath = "/LoginFromGoogle";
    option.Scope.Add("openid");
    option.Scope.Add("profile");


});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("ADMIN"); });
    options.AddPolicy("Clinet", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("Clinet"); });
    options.AddPolicy("ClinetOld", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("ClientOld"); });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
