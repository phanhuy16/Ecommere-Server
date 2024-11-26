
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Server.Contracts;
using Server.Services;
using Server.Helper;
using Server.Data;
using Server.Entities;
using Server.Utilities.Pagination;
using Microsoft.IdentityModel.Logging;



var builder = WebApplication.CreateBuilder(args);

var Jwt = builder.Configuration.GetSection("Jwt");

// Add services to the container
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<ICart, CartService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<ISetUp, SetupService>();
builder.Services.AddScoped<ISupplier, SupplierService>();
builder.Services.AddScoped<IPromotion, PromotionService>();

builder.Services.AddScoped<IClaimsSetup, ClaimsSetupService>();
// Helper
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("HRConnectString"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped(typeof(ApplicationSettings));

// Configure DbContext with connection string
builder.Services.AddDbContext<EFDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRConnectString"))
);

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("DepartmentPolicy",
//     policy => policy.RequireClaim("department"));
// });

if (builder.Environment.IsDevelopment())
{
    IdentityModelEventSource.LogCompleteSecurityArtifact = true;
}


// Configure Identity and Authentication
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<EFDataContext>()
                .AddSignInManager()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.SignIn.RequireConfirmedEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Email confirmation
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

}).AddEntityFrameworkStores<EFDataContext>().AddDefaultTokenProviders();

var key = Encoding.UTF8.GetBytes(Jwt.GetSection("SecurityKey").Value!);

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = Jwt["Issuer"],
    ValidAudience = Jwt["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key),
    // ClockSkew = TimeSpan.Zero
    ClockSkew = TimeSpan.FromMinutes(5),
};

builder.Services.AddSingleton(tokenValidationParams);

var jwtSection = builder.Configuration.GetSection("Jwt");
if (!jwtSection.Exists())
{
    throw new Exception("Jwt section is missing in appsettings.json");
}


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParams;

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    c.CustomSchemaIds(type => type.FullName);
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder =>
    {
        builder.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
    {
        var accessor = o.GetRequiredService<IHttpContextAccessor>();
        var request = accessor.HttpContext.Request;
        var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        return new UriService(uri);
    });
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseCors("Open");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
