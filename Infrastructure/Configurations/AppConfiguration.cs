using Application.Services.Implementations;
using Application.Services.Interfaces;
using Data;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Configurations
{
    public static class AppConfiguration
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            // Service life time DI
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IProductLineService, ProductLineService>();
            services.AddScoped<IVNPayService, VNPayService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IStatisticService, StatisticsService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.Net 8.0 - SuaMe88", Description = "APIs Service", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                 });
            });
        }

        public static void AddFirebase(this IServiceCollection services)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(Path.Combine(currentDirectory, "firebase-adminsdk.json")),
            });
        }
        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
        }
    }
}
