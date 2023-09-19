using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using Blazored.Toast;
using ChatHubs.Hubs;
using ClientUI.Service.IResponsitories;
using ClientUI.Service.Responsitories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// backend
builder.Services.AddHttpClient("examapi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BackEndAPIURL"]);
});

builder.Services.AddScoped(api => new HttpClient { BaseAddress = new Uri(builder.Configuration["BackEndAPIURL"]) });

builder.Services.AddDbContext<ASMDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});



// Đăng ký HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// mud
builder.Services.AddScoped<MudBlazor.Services.IKeyInterceptorFactory, MudBlazor.Services.KeyInterceptorFactory>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredToast();

builder.Services.AddAuthorizationCore();
// service
builder.Services.AddTransient<IResponsitoriesProduct, ResponsitoriesProduct>();
builder.Services.AddTransient<IResponsitoriesCartDetail, ResponsitoriesCartDetail>();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<IResponsitoriesUpload, ResponsitoriesUpload>();

// Add services to the container.

// chatHub
//cấu hình nén phản hồi
builder.Services.AddResponseCompression(otp =>
{
    otp.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/octet-stream"}); 
});

builder.Services.AddControllersWithViews();


// cau hinh identity
builder.Services.AddIdentity<User, Position>()
                .AddEntityFrameworkStores<ASMDBContext>().AddSignInManager<SignInManager<User>>();

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

builder.Services.AddSession();
// cau hinh google

builder.Services.AddAuthentication(options =>
{
    // DefaultAuthenticateScheme: Đây là Scheme sẽ được sử dụng để xác thực yêu cầu đã được xác thực thành công.
    // Trong trường hợp này,CookieAuthenticationDefaults.AuthenticationScheme  được sử dụng, điều này có nghĩa là sử dụng xác thực bằng cookie.
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };

            })
 // cau hinh cookie




 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
 {
     options.Cookie.Name = "_tokenAuthorization";
     options.LoginPath = "/Login/Login";
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
    option.AuthorizationEndpoint += "?prompt=consent"; // thêm prompt consent để yêu cầu xác thực người dùng
    option.AccessType = "offline"; // yêu cầu truy cập offline để có thể lấy refresh token
    option.SaveTokens = true;

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("ADMIN"); });
    options.AddPolicy("CLIENT", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("CLIENT"); });
    options.AddPolicy("ClinetOld", policy => { policy.RequireAuthenticatedUser(); policy.RequireRole("ClientOld"); });

});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// dang ky middleware
app.UseHttpsRedirection();
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapBlazorHub();
app.MapHub<ChatHub>("/chatHub");
app.UseResponseCompression();
app.MapFallbackToPage("/_Host");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LoginWithGoogle}");//Trang bắt đầu sau Layout
app.Run();
