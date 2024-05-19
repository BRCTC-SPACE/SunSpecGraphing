using System.Runtime.InteropServices;

namespace SunSpecGraphing
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new mainWindow());
        }
    

        // NOTE: This is for debugging
        // TODO: Remove/comment out before submission
        internal static class ConsoleAllocator
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool AllocConsole();
        }
    }
}