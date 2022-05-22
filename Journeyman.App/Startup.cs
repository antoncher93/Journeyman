using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Journeyman.App.Data;
using Microsoft.EntityFrameworkCore;
using Journeyman.App.Logging;
using Journeyman.App.BotCommands;

namespace Journeyman.App
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BotDbContext>(options => options.UseSqlServer(connection, 
                sqlServerOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure()));

            var token = Configuration["Token"];
            services.AddSingleton<ITelegramBotClient>(p => new TelegramBotClient(token));
            services.AddTransient<IBot, Bot>();
            services.AddTransient<BanUserCommand>();
            services.AddTransient<WarnUserCommand>();
            services.AddTransient<StartBotCommand>();
            services.AddTransient<AgreementBotCommand>();
            services
                .AddControllers()
                .AddNewtonsoftJson();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBot bot)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            bot.Start(string.Format(Configuration["Url"], "api/message/update"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
