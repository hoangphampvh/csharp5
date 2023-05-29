using ASMC5.data;
using ASMC5.Models;
using assiment_csad4.Configruration;
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
builder.Services.AddTransient<IProductServiece, ProductServiece>();
builder.Services.AddTransient<IBillServiece, BillServiece>();
builder.Services.AddTransient<IBillDetailServiece, BillDetailServiece>();
builder.Services.AddTransient<ICartServiece, CartServiece>();
builder.Services.AddTransient<ICartDetailServiece, CartDetailServiece>();

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

//

builder.Services.AddIdentity<User, Position>()
                .AddEntityFrameworkStores<ASMDBContext>()
    .AddSignInManager<SignInManager<User>>();
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