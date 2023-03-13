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
            string zel = Convert.ToString(Convert.ToInt32(Math.Truncate(x)), 2);
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

        public double Monna(string output)
        {
            double result = 0.0;
            StringBuilder mona = new();
            mona.Append("");
            bool dotFlag = false;
            bool realFlag = false;
            for (int i = output.Length - 1; i >= 0; i--)
            {
                if (output[i] == '.')
                { 
                    dotFlag = true;
                    continue;
                    //mona.Append(output[i]);
                }
                if (output[i] != '0' && !dotFlag)
                {
                    realFlag = true;
                }
            }
            dotFlag = false;
            for (int i = output.Length - 1; i >= 0; i--)
            {
                if (output[i] == '.')
                {
                    dotFlag = true;
                    continue;
                }
                if (realFlag)
                {
                    mona.Append(output[i]);
                }
                else if (dotFlag)
                {
                    mona.Append(output[i]);
                }
            }
            string tmp = mona.ToString();
            for (int i = 0; i < tmp.Length; i++)
            {
                double doubTmp = Convert.ToDouble(tmp[i].ToString());
                result += doubTmp / Math.Pow(2, i);
            }
            return result;
        }

        //testing
        /// Другое отображение 2-адического числа на отрезок [0,1] 
        public double Reverse(string output)
        {
            double result = 0;
            for (int i = 1; i < output.Length; i++)
            {
                if (output[output.Length - i] != '.')
                {
                    result += int.Parse(output[output.Length - i].ToString()) / Math.Pow(2, i);
                }
            }
            return result;
        }
    }
}
