using ASMC5.data;
using ASMC5.Models;
using BILL.CacheRedisData;
using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using BLL.Serviece.Implements.PaymentService;
using BLL.Serviece.Implements.PaymentService.VnPay.Config;
using BLL.Serviece.Interfaces;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.ModelConfiguration.mailConfig;
using Catel.Services;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext
builder.Services.AddDbContext<ASMDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});

// Add Dependencies
builder.Services.AddTransient<IUserServiece, UserServiece>();
builder.Services.AddTransient<IPositionServiece, PositionServiece>();
builder.Services.AddTransient<IProductServiece, ProductServiece>();
builder.Services.AddTransient<IBillServiece, BillServiece>();
builder.Services.AddTransient<IBillDetailServiece, BillDetailServiece>();
builder.Services.AddTransient<ICartServiece, CartServiece>();
builder.Services.AddTransient<ICartDetailServiece, CartDetailServiece>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddTransient<IUploadFileService, UploadFileService>();
builder.Services.AddTransient<ISendMailService, SendMailService>();
builder.Services.AddTransient<IMerchantService, MerchantService>();
builder.Services.AddTransient<IPaymentDestination, PaymentDestinationService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.Configure<VnpayConfig>(builder.Configuration.GetSection("Vnpay"));
// Add Identity
builder.Services.AddIdentity<User, Position>()
                .AddEntityFrameworkStores<ASMDBContext>()
    .AddSignInManager<SignInManager<User>>();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {

        //tự cấp token
        ValidateIssuer = false,
        ValidateAudience = false,
        //ký vào token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
       
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true,
    };
});

// mail setting
builder.Services.AddControllers();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
