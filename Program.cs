using System.Runtime;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopOwnerCore.Application_Core.Data;
using ShopOwnerCore.Application_Core.Email;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Enum;
using ShopOwnerCore.Application_Core.Interface;
using ShopOwnerCore.Application_Core.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ShopOwnerCore", Version = "v1" });
    s.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ShopOwnerCore", Version = "v2" });
    s.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Add JWT with Bearer",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Name = "Bearer",
            },
            new List<string>()
        }
    });
});


       
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader());

});
builder.Services.Configure<IISOptions>(options => { });

builder.Services.AddDbContext<ApplicationContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection"), b => b.MigrationsAssembly("ShopOwnerCore")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUnitWork, UnitWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddIdentity<User, ApplicationRole>(Options =>
{
    Options.Password.RequiredLength = 6;
    Options.Password.RequireDigit = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.Password.RequireNonAlphanumeric = false;

    Options.Lockout.AllowedForNewUsers = true;
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    Options.Lockout.MaxFailedAccessAttempts = 3;

}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey= true,
        ValidIssuer= builder.Configuration["JwtSettings:validIssuer"],
        ValidAudience= builder.Configuration["JwtSettings:validAudience"],
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:secret"]!))

        



    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("./swagger/v1/swagger.json", "ShopCore v1");
        options.SwaggerEndpoint("./swagger/v2/swagger.json", "ShopCore v2");
        options.RoutePrefix = string.Empty;
    });
}


app.UseCors("CorsPolicy");
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});
    
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();


app.Run();
