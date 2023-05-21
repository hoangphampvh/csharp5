using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AddTransient cac Interface o day
builder.Services.AddTransient<IUserServiece, UserServiece>();
builder.Services.AddTransient<IPositionServiece, PositionServiece>();

// cau hinh identity
builder.Services.AddIdentity<User, Position>()
                .AddEntityFrameworkStores<ASMDBContext>();
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
builder.Services.AddDbContext<ASMDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});

// cau hinh jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["JwtIssuer"],
                            ValidAudience = builder.Configuration["JwtAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKeys"]))
                        };

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
     options.Cookie.Name = "CookieName";
     options.LoginPath = "/Account/Login";
     options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Thiết lập chính sách bảo mật
     options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thiết lập thời gian sống của cookie
     options.SlidingExpiration = true; // Cho phép cookie được cập nhật thời gian sống mỗi khi có request mới
     options.Cookie.HttpOnly = true;
 });

builder.Services.AddAuthentication().AddGoogle(option =>
{
    var ggConfig =builder.Configuration.GetSection("Authentication:Google");
    option.ClientId = ggConfig["ClientId"];
    option.ClientSecret = ggConfig["ClientSecret"];
    option.CallbackPath = "/LoginFromGoogle";

});

//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
