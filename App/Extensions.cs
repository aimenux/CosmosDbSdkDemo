using System;
using App.Launchers;

namespace App
{
    public static class Extensions
    {
        private static bool WaitForConfirmationIsDisabled
        {
            get
            {
#if RELEASE
                return true;
#else
                return false;
#endif
            }
        }

        public static void WriteLine(this ConsoleColor color, object value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WaitForConfirmation(this LauncherTargets target)
        {
            if (WaitForConfirmationIsDisabled) return;
            ConsoleColor.Yellow.WriteLine($"\nPress any key to run '{target}'\n");
            Console.ReadKey();
        }
    }
}
