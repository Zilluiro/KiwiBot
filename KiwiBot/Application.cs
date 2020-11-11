using KiwiBot.Extensions;
using KiwiBot.Helpers;
using KiwiBot.Services;
using KiwiBot.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace KiwiBot
{
    class Application
    {
        private IConfiguration Configuration { get; set; }

        public Application(IServiceCollection serviceCollection)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            ConfigureServices(serviceCollection);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddLogging(builder =>
            {
               builder.AddConsole(); 
            });

            // register all implementations of IBooruService
            Assembly.GetEntryAssembly().GetTypesAssignableFrom<IBooruService>().ForEach((t)=>
            {
                services.AddScoped(typeof(IBooruService), t);
            });

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<TelegramBot>();

            services.Configure<BotSettings>(Configuration.GetSection("BotSettings"));
        }

        public void Start(IServiceCollection services)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            TelegramBot telegramBot = provider.GetService<TelegramBot>();

            telegramBot.Start();
        }
    }
}
