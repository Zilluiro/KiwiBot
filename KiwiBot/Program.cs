using Microsoft.Extensions.DependencyInjection;
using System;

namespace KiwiBot
{
    public class Program 
    {
        public static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Application application = new Application(serviceCollection);

            application.Start(serviceCollection);
        }
    }
}
