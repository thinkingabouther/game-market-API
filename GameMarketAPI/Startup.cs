using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using game_market_API.Models;
using game_market_API.Security;
using game_market_API.Services;
using game_market_API.Services.ClientService;
using game_market_API.Services.ExceptionLoggingService;
using game_market_API.Services.NotifyingService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace game_market_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RedisLoggingService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context => throw new ApiProblemDetailsException(context.ModelState);
            });
            services.AddDbContextPool<GameMarketDbContext>(options => options
                .UseSqlite("Data Source=game-market.db"));
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameKeyService, GameKeyService>();
            services.AddScoped<IPaymentSessionService, PaymentSessionService>();
            services.AddScoped<IAcquiringService, AcquiringService>();
            services.AddScoped<INotifyingService, ClientNotifyingService>();
            services.AddScoped<INotifyingService, VendorNotifyingService>();
            services.AddScoped<IExceptionLoggingService, RedisLoggingService>();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RedisLoggingService redisService)
        {
            app.UseExceptionHandler("/error");

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            // app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            // {
            //     IsApiOnly = false, 
            //     WrapWhenApiPathStartsWith = "/api",
            //     ShowStatusCode = true,
            //     UseApiProblemDetailsException = true
            // }); // using AutoWrapper to wrap responses and exceptions consistently
            

            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            redisService.Connect();
        }
    }
}