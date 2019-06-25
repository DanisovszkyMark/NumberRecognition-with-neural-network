using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NumberRecognition.Paint
{
    class PaintProcessor
    {
        public static float[] PaintToInput(Grid grid)
        {

            float[] atmInput = new float[grid.Children.Count];

            for (int i = 0; i < grid.Children.Count; i++)
            {
                if ((grid.Children[i] as Rectangle).Fill == Brushes.White) atmInput[i] = 1;
            }

            return atmInput;
        }

        public static bool SavePaint(Grid grid, int number, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, true);

                float[] atm = PaintToInput(grid);
                for (int i = 0; i < atm.Length; i++)
                {
                    sw.Write(atm[i]);
                }

                string atmNumber = "";
                for (int i = 0; i < 10; i++)
                {
                    if (i == number) atmNumber += "1";
                    else atmNumber += "0";
                }


                sw.WriteLine("-" + atmNumber);
                sw.Close();
            }
            catch { return false; }

            return true;
        }
    }
}
