using System;
using System.Threading.Tasks;
using App.Bootstrappers;

namespace App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var bootstrappers = new ISdkBootstrapper[]
            {
                new Sdk2Bootstrapper(),
                new Sdk3Bootstrapper()
            };

            foreach (var bootstrapper in bootstrappers)
            {
                ConsoleColor.Cyan.WriteLine(bootstrapper.Name);
                await bootstrapper.LaunchAsync(args);
            }

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}
