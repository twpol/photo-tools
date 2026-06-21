using System;
using System.Linq;
using Eto.Forms;

namespace Photo_Reviewer
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var folder = args.FirstOrDefault(static arg => !arg.StartsWith('-'));
            if (folder == null)
            {
                Console.Error.WriteLine("Error: No photo folder path specified");
                return;
            }
            new Application(Eto.Platform.Detect).Run(new MainForm(new PhotoFolder(folder)));
        }
    }
}
