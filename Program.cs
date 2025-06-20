using System;
using System.Windows.Forms;

namespace EducationalPractice
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new StartMenuForm());
        }
    }
}