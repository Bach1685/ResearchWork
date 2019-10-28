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
        public static bool ExistsMackroCrack { get; set; }

        public static string Result { get; set; }

        public static void TimeСycle(List<Area> areas, int stepsCount, int cyclesPerStepCount)
        {
            Result = "Не возникла ни одна короткая трещина";
            int cycleCounter = 0;
            ExistsMackroCrack = false;
            areas.FindAll(x => x.ShortCracks != null).ForEach(y => y.ShortCracks.Clear());
            areas.ForEach(x => x.ShortCrackProbability = 0);
            areas.ForEach(x => x.Damage = 0);
            areas.ForEach(x => x.DamageAccumulationRate = 0);
            areas.ForEach(x => x.DestroyedStructuralElementsCount = 0);


            while (cycleCounter <= cyclesPerStepCount * stepsCount) //ЦИКЛ ПО ВРЕМЕНИ
            {
                double shortCrackTotalProbability = 0;

                AreaCycle(areas, cyclesPerStepCount, ref shortCrackTotalProbability);

                if (shortCrackTotalProbability >= 0.5)
                {
                    Form1 form1 = new Form1();
                    Result = $"Появилась короткая трещина. Прошло циклов: {cycleCounter}";
                    CreateShortCrack(areas);
                }

                if (ExistsMackroCrack)
                    return;

                cycleCounter += cyclesPerStepCount;
            }
        }

        public static void CreateShortCrack(List<Area> areas)
        {
            int ShortCrackAreaIndex = GetShortCrackAreaIndex(areas);

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

        public static int GetShortCrackAreaIndex(List<Area> areas)
        {
            List<int> lineNumber = new List<int>();

            var minShortCrackProbability = areas.FindAll(x => x.ShortCrackProbability != 0).Min(y => y.ShortCrackProbability);

            int areasCount = areas.Count();
            for (int areaCounter = 0; areaCounter < areasCount; areaCounter++)
            {
                int areaNumberOnLine = (int)Math.Round(areas[areaCounter].ShortCrackProbability / minShortCrackProbability);
                for (int i = 0; i < areaNumberOnLine; i++)
                    lineNumber.Add(areaCounter);
            }
            Random rand = new Random();

            return lineNumber[rand.Next(lineNumber.Count())];
        }
    }
}
