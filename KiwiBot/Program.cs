using KiwiBot.BooruClients.Abstract;
using KiwiBot.Data;
using KiwiBot.Data.Repository;
using KiwiBot.Extensions;
using KiwiBot.Handlers;
using KiwiBot.Helpers;
using KiwiBot.Services;
using KiwiBot.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Tasks;

namespace KiwiBot
{
    public class Program 
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            using (var context = builder.Services.GetService<DataContext>())
            {
                context.Database.Migrate();
                context.Initialize();
            }

            await builder.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                });

                services.AddHttpClient();
                services.AddLogging(builder =>
                {
                    builder.AddConsole(); 
                });

                // register all implementations of IAbstractBooruFactory
                Assembly.GetEntryAssembly().GetTypesAssignableFrom<IAbstractBooruFactory>().ForEach((t) =>
                {
                    services.AddScoped(typeof(IAbstractBooruFactory), t);
                });

                // register all handlers
                Assembly.GetEntryAssembly().GetTypesAssignableFrom<BaseHandler>().ForEach((t) =>
                {
                    services.AddScoped(t);
                });

                services.AddScoped<IMessageService, MessageService>();
                services.AddScoped<IChatService, ChatService>();
                services.AddScoped<IBooruService, BooruService>();
                services.AddScoped<IGlobalRepository, GlobalRepository>();
                services.AddSingleton<IHostedService, TelegramBot>();

                services.Configure<BotSettings>(hostContext.Configuration.GetSection("BotSettings"));
            });
    }
}
