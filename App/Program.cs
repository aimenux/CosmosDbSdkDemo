using System;
using System.Threading.Tasks;
using App.Launchers;

namespace App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var launchers = new ISdkLauncher[]
            {
                new Sdk2Launcher(),
                new Sdk3Launcher()
            };

            foreach (var launcher in launchers)
            {
                ConsoleColor.Cyan.WriteLine(launcher.Name);
                await launcher.LaunchAsync(args);
            }

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}
