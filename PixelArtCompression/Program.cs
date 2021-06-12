using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelArtCompression
{
    static class Program
    {

        private static Form1 UI;

        [STAThread]
        static void Main()
        {
            UI = new Form1();
            Application.EnableVisualStyles();
            Application.Run(UI);
        }

        public static Form1 GetUI()
        {
            return UI;
        }

    }
}
