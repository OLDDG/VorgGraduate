using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Calculate
    {
        private string? Ffunc;
        private string? Gfunc;
        private string? RegisterSize;
        private string? InputX;
        private string? InputY;

        public static void Main()
        { 
        }

        public Calculate(IItem items)
        { 
            Ffunc = items.Ffunc;
            Gfunc = items.Gfunc;
            RegisterSize = items.RegisterSize;
            InputX = items.InputX;
            InputY = items.InputY;
        }
            
        // correct.
        /// Перевод из double в 2-ичную строку. 
        public string Two (double x, int n)
        {
            string zel = Convert.ToString(Convert.ToInt64(Math.Truncate(x)), 2);
            List<double> asd = new ();
            asd.Add(x - Math.Truncate(x));
            StringBuilder result = new(zel);
            result.Append('.');

            for (int i = 1; i <= n; i++)
            {
                double tmp = (2 * asd[i - 1]) - Math.Truncate(asd[i - 1] * 2);
                asd.Add(tmp);
                result.Append(Convert.ToInt32(Math.Truncate(asd[i - 1] * 2)));
            }

            return result.ToString();
        }

        public double Monna(string output, int size)
        {
            output = output.Replace('.'.ToString(), string.Empty);
            string reversed = new (output.Reverse().ToArray());
            
            double result = 0;
            int pow = size;
            for (int i = reversed.Length - 1; i >= reversed.Length - size; i--)
            {
                double doubTmp = Convert.ToDouble(reversed[i].ToString());
                result += doubTmp / Math.Pow(2, pow);
                pow--;
            }
            return result;
        }

        //testing
        /// Другое отображение 2-адического числа на отрезок [0,1] 
        public double Reverse(string output, int size)
        {
            output = output.Replace('.'.ToString(), string.Empty);
            double result = 0;
            int pow = 1;
            for (int i = 0; i < size; i++)
            {
                double doubTmp = Convert.ToDouble(output[i].ToString());
                result += doubTmp / Math.Pow(2, pow);
                pow++;
            }
            return result;
        }
    }
}
