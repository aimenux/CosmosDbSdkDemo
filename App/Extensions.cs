using System;

namespace App
{
    public static class Extensions
    {
        public static void WriteLine(this ConsoleColor color, object value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WaitForConfirmation(this Targets target)
        {
            ConsoleColor.Yellow.WriteLine($"Press any key to run '{target}'");
            Console.ReadKey();
        }
    }
}
