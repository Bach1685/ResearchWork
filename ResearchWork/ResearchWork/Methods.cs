using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfueliteSolutionsRandomHelper;

namespace ResearchWork
{
    class Methods
    {
        public static void Systematizing(List<Area> area, object[,] arrData, int iLastRow)
        {
            double[] recordMaxPrincipal = new double[20];
            double[] recordRadial = new double[20];
            int z = 0, alpha = 0, r = 0, a = 0, column = 0;
            bool isThreeNodes = true, isThreeRow = true;
            double countRow = 0;

            for (int i = 0; i < iLastRow; i++)
            {
                double w = (double)arrData[i + 1, 4];
                double ww = (double)arrData[i + 1, 6];
                double www = Math.Round((double)arrData[i + 1, 5], 2);

                if (Math.Round((double)arrData[i + 1, 4], 2) >= ((-12.5 + 0.42 * z) - 0.39) && Math.Round((double)arrData[i + 1, 4], 2) <= ((-12.5 + 0.42 * z) + 0.39)
                   && (Math.Round((double)arrData[i + 1, 6], 2) >= ((90 - (0.75 * alpha)) - 0.5) && (Math.Round((double)arrData[i + 1, 6], 2) <= ((90 - (0.75 * alpha)) + 0.5)))
                   && (Math.Round((double)arrData[i + 1, 5], 2) >= ((25 + (0.083 * r)) - 0.06) && Math.Round((double)arrData[i + 1, 5], 2) <= ((25 + (0.083 * r)) + 0.06)))
                {
                    double Z = (double)arrData[i + 1, 4];
                    double ALPHA = (double)arrData[i + 1, 6];
                    double R = (double)arrData[i + 1, 5];

                    recordMaxPrincipal[a] = (double)arrData[i + 1, 11];
                    recordRadial[a] = (double)arrData[i + 1, 13];

                    a++; // счетчик узлов на участке 

                    if (a == 20)
                    {
                        Area ar = new Area();
                        ar.Stress.Radial = recordRadial.Sum() / recordRadial.Length;
                        ar.Stress.MaxPrincipal = recordMaxPrincipal.Sum() / recordMaxPrincipal.Length;
                        area.Add(ar);

                        //if (area.Count == 900)
                        //{ break; } // удалить!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        a = 0;
                        r -= 2;
                        alpha -= 2;
                        //    z -= 2;
                        column += 2;
                        countRow = 0;

                        if (z == 30)
                        {
                            z = 0;
                            alpha += 2;
                            column = 0;

                            if (alpha == 120)
                            {
                                r += 2;
                                alpha = 0;
                            }
                        }
                        i = -1; // для поиска заново
                        continue;
                    }

                    i = -1; // для поиска заново
                    z++;

                    if (isThreeRow)
                    {
                        if (z == 3 + column)
                        {
                            z -= 3;
                            alpha++;
                            isThreeNodes = !isThreeNodes;
                            countRow++;
                        }

                        if (countRow == 3)
                        {
                            countRow = 0;
                            isThreeRow = !isThreeRow;
                            alpha -= 3;
                            r++;
                        }
                    }
                    else
                    {
                        if (z == 3 + column)
                        {
                            z -= 3;
                            alpha += 2;
                            countRow++;
                        }

                        if (countRow == 2)
                        {
                            countRow = 0;
                            isThreeRow = !isThreeRow;
                            isThreeNodes = !isThreeNodes;
                            alpha -= 4;
                            r++;
                        }
                    }

                    if ((z == 1 + column) && !isThreeNodes)
                        z++;
                }
            }
        }

        public static void TimeСycle(List<Area> areas, int stepsNumber, int cyclesPerStepNumber)
        {
            int cycleCounter = 0;
            double shortCrackTotalProbability = 0;

            while (cycleCounter <= cyclesPerStepNumber * stepsNumber) //ЦИКЛ ПО ВРЕМЕНИ
            {
                AreaCycle(areas/*, cycleCounter*/, cyclesPerStepNumber, ref shortCrackTotalProbability);

                if (shortCrackTotalProbability >= 0.5)
                {
                    List<double> ShortCrackProbabilityList = new List<double>();
                    List<int> lineNumber = new List<int>();

                    foreach (Area area in areas)
                        if (area.ShortCrackProbability != 0)
                            ShortCrackProbabilityList.Add(area.ShortCrackProbability);

                    double minShortCrackProbability = ShortCrackProbabilityList.Min();

                    //foreach (Area area in areas)
                    //    if (lineNumber.Count() == 0)
                    //        lineNumber.Add(Math.Round(area.ShortCrackProbability / minShortCrackProbability));
                    //    else
                    //        lineNumber.Add(lineNumber.Last() + Math.Round(area.ShortCrackProbability / minShortCrackProbability));


                    //foreach (Area area in areas)
                    for (int areaCounter = 0; areaCounter < areas.Count(); areaCounter++)
                    {
                        int areaNumberOnLine = (int)Math.Round(areas[areaCounter].ShortCrackProbability / minShortCrackProbability);
                        for (int i = 0; i < areaNumberOnLine; i++)
                            lineNumber.Add(areaCounter);
                    }
                    Random rand = new Random();

                    int ShortCrackAreaIndex = rand.Next(lineNumber.Count());

                    ShortCrack shortCrack = new ShortCrack();
                    shortCrack.Length = 0.0002;

                    double B_tr = 0.006, h_rt = 0.002, f_g = 1.0;

                    double f_B = Math.Sqrt(1 / (Math.Cos((3.14 * (shortCrack.Length / 2) / B_tr) * Math.Sqrt(shortCrack.Length / (2 * h_rt)))));
                    double F_function = (1.04 + (0.202 * Math.Pow((shortCrack.Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((shortCrack.Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
                    shortCrack.SIF = (Math.Abs(areas[ShortCrackAreaIndex].Stress.Radial) + Math.Abs(areas[ShortCrackAreaIndex].Stress.MaxPrincipal)) * (Math.Sqrt(3.14 * (shortCrack.Length / 2)) / (3.14 / 2)) * F_function;

                    areas[ShortCrackAreaIndex].ShortCracks.Add(shortCrack);
                }

                cycleCounter += cyclesPerStepNumber;
                //    stepsCounter++;
                //}
            }
        }

        public static void AreaCycle(List<Area> areas/*, int cycleCounter*/, int cyclesPerStepNumber, ref double shortCrackTotalProbability)
        {
            foreach (var area in areas)
            {
                //areas[areasCounter].Damage = cycleCounter == 0 ? areas[areasCounter].Damage0 : ;
                area.DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((Math.Abs(area.Stress.Radial) + Math.Abs(area.Stress.MaxPrincipal)), 5.1);
                area.Damage += area.DamageAccumulationRate * cyclesPerStepNumber;
                area.DestroyedStructuralElementsNumber = Math.Round(area.StructuralElementsNumber * area.Damage);

                if (area.ShortCracks.Count() > 0)
                {
                    double V_sum = 0; // тут ли обнулять? В восьмой студии только одно обнуление в самом начале проги... -- скорее тут

                    //ShortCracksCycle(areas/*, cycleCounter*/, areasCounter, cyclesPerStepNumber, ref V_sum);
                    ShortCracksCycle(area, areas.Count(), cyclesPerStepNumber, ref V_sum);

                    double koef = V_sum / area.Value; // коэфф. для расчета в следующей строке

                    area.DestroyedStructuralElementsNumber = area.DestroyedStructuralElementsNumber - Math.Round(area.DestroyedStructuralElementsNumber * koef);
                }

                    area.ShortCrackProbability = 1.0 - (Math.Pow((1.0 - area.Damage), area.DestroyedStructuralElementsNumber));

                    if (area.ShortCrackProbability > 1)
                        area.ShortCrackProbability = 1;
                    else if (area.ShortCrackProbability < 0)
                        area.ShortCrackProbability = 0;

                    shortCrackTotalProbability += area.ShortCrackProbability;

                    if (shortCrackTotalProbability > 1)
                        shortCrackTotalProbability = 1;
                    else if (shortCrackTotalProbability < 0)
                        shortCrackTotalProbability = 0;
                

            }



            //for (int areasCounter = 0; areasCounter < areas.Count(); areasCounter++) // ЦИКЛ ПО УЧАСТКАМ
            //{
            //    //areas[areasCounter].Damage = cycleCounter == 0 ? areas[areasCounter].Damage0 : ;
            //    areas[areasCounter].DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal), 5.1);
            //    areas[areasCounter].Damage += areas[areasCounter].DamageAccumulationRate * cyclesPerStepNumber;
            //    areas[areasCounter].DestroyedStructuralElementsNumber = Math.Round(areas[areasCounter].StructuralElementsNumber * areas[areasCounter].Damage);

            //    if (areas[areasCounter].ShortCracks.Count() > 0)
            //    {
            //        double V_sum = 0; // тут ли обнулять? В восьмой студии только одно обнуление в самом начале проги 

            //        ShortCracksCycle(areas/*, cycleCounter*/, areasCounter, cyclesPerStepNumber, ref V_sum);

            //        double koef = V_sum / areas[areasCounter].Value; // коэфф. для расчета в следующей строке

            //        areas[areasCounter].DestroyedStructuralElementsNumber = areas[areasCounter].DestroyedStructuralElementsNumber - Math.Round(areas[areasCounter].DestroyedStructuralElementsNumber * koef);

            //        areas[areasCounter].ShortCrackProbability = 1.0 - (Math.Pow((1.0 - areas[areasCounter].Damage), areas[areasCounter].DestroyedStructuralElementsNumber));

            //        if (areas[areasCounter].ShortCrackProbability > 1)
            //            areas[areasCounter].ShortCrackProbability = 1;
            //        else if (areas[areasCounter].ShortCrackProbability < 0)
            //            areas[areasCounter].ShortCrackProbability = 0;

            //        shortCrackTotalProbability += areas[areasCounter].ShortCrackProbability;

            //        if (shortCrackTotalProbability > 1)
            //            shortCrackTotalProbability = 1;
            //        else if (shortCrackTotalProbability < 0)
            //            shortCrackTotalProbability = 0;
            //    }
            //}
        }

        //public static void ShortCracksCycle(List<Area> areas/*, int cycleCounter*/, int areasCounter, int numberCyclesPerStep, ref double V_sum)
        public static void ShortCracksCycle(Area area, int areasCount, int numberCyclesPerStep, ref double V_sum)
        {
            area.Value = ((4.9052 * Math.Pow(10.0, -4.0)) / areasCount) * 0.002;

            foreach (var shortCrack in area.ShortCracks)
            {
                shortCrack.DevelopmentRate = Math.Pow(10.0, -6.0) * Math.Pow(shortCrack.SIF, 5.0);

                shortCrack.Length += shortCrack.DevelopmentRate * numberCyclesPerStep;

                if (shortCrack.Length > 0.005) //насколько это корректно
                {
                    shortCrack.SIF = 3;
                    shortCrack.Length = 0.005;
                }
                else
                {
                    double B_tr = 0.006, h_rt = 0.002, f_g = 1.0;

                    double f_B = Math.Sqrt(1 / (Math.Cos((3.14 * (shortCrack.Length / 2) / B_tr) * Math.Sqrt(shortCrack.Length / (2 * h_rt)))));
                    double F_function = (1.04 + (0.202 * Math.Pow((shortCrack.Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((shortCrack.Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
                    shortCrack.SIF = (Math.Abs(area.Stress.Radial) + Math.Abs(area.Stress.MaxPrincipal)) * (Math.Sqrt(3.14 * (shortCrack.Length / 2)) / (3.14 / 2)) * F_function;
                }

                double r0 = (1 / (2.0 * 3.14)) * (shortCrack.SIF / (Math.Abs(area.Stress.Radial) + Math.Abs(area.Stress.MaxPrincipal)));
                double r90 = r0 / 1.125;
                double b = Math.Sqrt((r0 * r0) + (r90 * r90));
                double F = (3.14 * Math.Pow(((shortCrack.Length / 2) + r0), 2)) / 2;
                double V = F * 2 * r90;
                V_sum += V;
            }

            //for (int shortCrackCounter = 0; shortCrackCounter < areas[areasCounter].ShortCracks.Count(); shortCrackCounter++) // ЦИКЛ ПО КОРОТКИМ ТРЕЩИНАМ
            //{
            //    areas[areasCounter].ShortCracks[shortCrackCounter].DevelopmentRate = Math.Pow(10.0, -6.0) * Math.Pow(areas[areasCounter].ShortCracks[shortCrackCounter].SIF, 5.0);

            //    areas[areasCounter].ShortCracks[shortCrackCounter].Length += areas[areasCounter].ShortCracks[shortCrackCounter].DevelopmentRate * numberCyclesPerStep;

            //    if (areas[areasCounter].ShortCracks[shortCrackCounter].Length > 0.005)
            //    {
            //        areas[areasCounter].ShortCracks[shortCrackCounter].SIF = 3;
            //        areas[areasCounter].ShortCracks[shortCrackCounter].Length = 0.005;
            //    }
            //    else
            //    {
            //        double B_tr = 0.006, h_rt = 0.002, f_g = 1.0;

            //        double f_B = Math.Sqrt(1 / (Math.Cos((3.14 * (areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / B_tr) * Math.Sqrt(areas[areasCounter].ShortCracks[shortCrackCounter].Length / (2 * h_rt)))));
            //        double F_function = (1.04 + (0.202 * Math.Pow((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
            //        areas[areasCounter].ShortCracks[shortCrackCounter].SIF = (areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal) * (Math.Sqrt(3.14 * (areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2)) / (3.14 / 2)) * F_function;
            //    }

            //    double r0 = (1 / (2.0 * 3.14)) * (areas[areasCounter].ShortCracks[shortCrackCounter].SIF / (areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal));
            //    double r90 = r0 / 1.125;
            //    double b = Math.Sqrt((r0 * r0) + (r90 * r90));
            //    double F = (3.14 * Math.Pow(((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) + r0), 2)) / 2;
            //    double V = F * 2 * r90;
            //    V_sum += V;
            //}
        }
    }

}
