using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arg)
        {
            ControllerMainForm cm;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            switch (arg.Length)
            {
                case 0: cm = new ControllerMainForm(); 
                    break;
                case 1: cm = new ControllerMainForm(Convert.ToInt32(arg[0])); break;
                case 2: cm = new ControllerMainForm(Convert.ToInt32(arg[0]), Convert.ToInt32(arg[1])); break;                                
                default : cm = new ControllerMainForm(); break; // если передали больше параметров, чем определено в конструкторе
            }

            Application.Run(cm);
        }
    }
}
