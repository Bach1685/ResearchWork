using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfueliteSolutionsRandomHelper;

namespace ResearchWork
{
    partial class Methods
    {
        public static void TimeСycle(List<Area> areas, int stepsCount, int cyclesPerStepCount)
        {
            int cycleCounter = 0;
            //double shortCrackTotalProbability = 0;

            while (cycleCounter <= cyclesPerStepCount * stepsCount) //ЦИКЛ ПО ВРЕМЕНИ
            {
                double shortCrackTotalProbability = 0;

                AreaCycle(areas, cyclesPerStepCount, ref shortCrackTotalProbability);

                if (areas.Exists(x => x.ShortCracks.Exists(y => y.SIF >= 3)))
                {
                    SendResultToForm(areas, cycleCounter);
                    return;
                }


                if (shortCrackTotalProbability < 0.5)
                { }

                if (shortCrackTotalProbability >= 0.5)
                {
                    List<int> lineNumber = new List<int>();

                    var minShortCrackProbability = areas.FindAll(x => x.ShortCrackProbability != 0).Min(y => y.ShortCrackProbability);

                    for (int areaCounter = 0; areaCounter < areas.Count(); areaCounter++)
                    {
                        int areaNumberOnLine = (int)Math.Round(areas[areaCounter].ShortCrackProbability / minShortCrackProbability);
                        for (int i = 0; i < areaNumberOnLine; i++)
                            lineNumber.Add(areaCounter);
                    }
                    Random rand = new Random();

                    int ShortCrackAreaIndex = lineNumber[rand.Next(lineNumber.Count())];

                    ShortCrack shortCrack = new ShortCrack();
                    shortCrack.Length = 0.0002;

                    double B_tr = 0.0125; // ширина подшипника
                    double h_tr = 0.002; // толщина баббитового слоя (должна варьироваться)
                    double a_tr = shortCrack.Length;
                    double b_tr = (a_tr / 2) < h_tr ? (a_tr / 2) : h_tr;
                    double f_g = 1, f_teta = 1;
                    //  f_g = 1 - почему?
                    double f_B = Math.Sqrt(1 / Math.Cos((Math.PI * a_tr / B_tr) * Math.Sqrt(b_tr / h_tr)));

                    double F1 = 1.04; 
                    double F2 = 0.202;
                    double F3 = -1.06;
                    double F = (F1 + (F2 * Math.Pow((b_tr / h_tr), 2)) + (F3 * Math.Pow((b_tr / h_tr), 4))) * f_teta * f_g * f_B;

                    shortCrack.SIF = (areas[ShortCrackAreaIndex].Stress.Radial + areas[ShortCrackAreaIndex].Stress.MaxPrincipal) * (Math.Sqrt(Math.PI * (shortCrack.Length / 2)) / (Math.PI / 2)) * F; // F_function; // тут не должно быть NaN

                    areas[ShortCrackAreaIndex].ShortCracks.Add(shortCrack);
                }

                cycleCounter += cyclesPerStepCount;
            }
        }
    }
}
