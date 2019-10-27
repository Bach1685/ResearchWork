using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfueliteSolutionsRandomHelper;

namespace ResearchWork
{
    partial class Calculation
    {
        public static void ShortCracksCycle(Area area, int areasCount, int cyclesPerStepCount, ref double V_sum)
        {
            foreach (var shortCrack in area.ShortCracks)
            {
                shortCrack.DevelopmentRate = Math.Pow(10, -6) * Math.Pow(shortCrack.SIF, 5);

                shortCrack.Length += shortCrack.DevelopmentRate * cyclesPerStepCount;

                if (shortCrack.Length > 0.005) //насколько это корректно (типа выставлять костыльно КИН и длину)
                {
                    //shortCrack.SIF = 3;
                    //shortCrack.Length = 0.005;

                    ExistsMackroCrack = true;
                }
                else
                {
                    shortCrack.SIF = GetRecalculatedSIF(area, shortCrack);
                }

                V_sum += GetExcludedVolume(shortCrack, area);

                if (V_sum > area.Volume * areasCount)
                { }
            }
        }

        public static double GetRecalculatedSIF(Area area, ShortCrack shortCrack)
        {
            double B_tr = 0.0125; // ширина подшипника
            double h_tr = 0.002; // толщина баббитового слоя (должна варьироваться)
            double a_tr = shortCrack.Length;
            double b_tr = (a_tr / 2) < h_tr ? (a_tr / 2) : h_tr;
            double f_g = 1, f_teta = 1; //  f_g = 1 - почему?
            double f_B = Math.Sqrt(1 / Math.Cos((Math.PI * a_tr / B_tr) * Math.Sqrt(b_tr / h_tr))); // в маткаде проверил, что тут NaN быть не должно

            double F1 = 1.04; //1.13 - 0.09 * (b_tr / a_tr);
            double F2 = 0.202; //-0.54 + 0.89 * (0.2 + (b_tr / a_tr));
            double F3 = -1.06; //0.5 - (1 / (0.65 + (b_tr / a_tr))) + (14 * (1 - (b_tr / a_tr)));
            double F = (F1 + (F2 * Math.Pow((b_tr / h_tr), 2)) + (F3 * Math.Pow((b_tr / h_tr), 4))) * f_teta * f_g * f_B;

            double SIF = (area.Stress.Radial + area.Stress.MaxPrincipal) * (Math.Sqrt(Math.PI * (shortCrack.Length / 2)) / (Math.PI / 2)) * F; // F_function; // тут не должно быть NaN

            if (shortCrack.SIF < 0)
            { }

            return SIF;
        }

        public static double GetExcludedVolume(ShortCrack shortCrack, Area area)
        {
            double r0 = (1 / (2 * Math.PI)) * Math.Pow((shortCrack.SIF / (area.Stress.Radial + area.Stress.MaxPrincipal)), 2);
            double r90 = r0 / 1.125;
            double S = Math.PI * Math.Pow(r0, 2);
            double TorVolume = S * 2 * Math.PI * (shortCrack.Length / 2);

            return TorVolume;
        }
    }
}
