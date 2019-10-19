//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EfueliteSolutionsRandomHelper;

//namespace ResearchWork
//{
//    class Methods
//    {
//        public static void Systematizing(List<Area> area, object[,] arrData, int iLastRow)
//        { 
//            double[] recordMaxPrincipal = new double[20];
//            double[] recordRadial = new double[20];
//            int z = 0, alpha = 0, r = 0, a = 0, column = 0;
//            bool isThreeNodes = true, isThreeRow = true;
//            double countRow = 0;

//            for (int i = 0; i < iLastRow; i++)
//            {
//                double w = (double)arrData[i + 1, 4];
//                double ww = (double)arrData[i + 1, 6];
//                double www = Math.Round((double)arrData[i + 1, 5], 2);

//                if (Math.Round((double)arrData[i + 1, 4], 2) >= ((-12.5 + 0.42 * z) - 0.39) && Math.Round((double)arrData[i + 1, 4], 2) <= ((-12.5 + 0.42 * z) + 0.39)
//                   && (Math.Round((double)arrData[i + 1, 6], 2) >= ((90 - (0.75 * alpha)) - 0.5) && (Math.Round((double)arrData[i + 1, 6], 2) <= ((90 - (0.75 * alpha)) + 0.5)))
//                   && (Math.Round((double)arrData[i + 1, 5], 2) >= ((25 + (0.083 * r)) - 0.06) && Math.Round((double)arrData[i + 1, 5], 2) <= ((25 + (0.083 * r)) + 0.06)))
//                {
//                    double Z = (double)arrData[i + 1, 4];
//                    double ALPHA = (double)arrData[i + 1, 6];
//                    double R = (double)arrData[i + 1, 5];

//                    recordMaxPrincipal[a] = (double)arrData[i + 1, 11];
//                    recordRadial[a] = (double)arrData[i + 1, 13];

//                    a++; // счетчик узлов на участке 

//                    if (a == 20)
//                    {
//                        Area ar = new Area();
//                        ar.Stress.Radial = Math.Abs(recordRadial.Sum() / recordRadial.Length); // суммируются не модульные выражения
//                        ar.Stress.MaxPrincipal = Math.Abs(recordMaxPrincipal.Sum() / recordMaxPrincipal.Length);
//                        area.Add(ar);

//                        if (area.Count == 900)
//                        { break; } // удалить!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

//                        a = 0;
//                        r -= 2;
//                        alpha -= 2;
//                        //    z -= 2;
//                        column += 2;
//                        countRow = 0;

//                        if (z == 30)
//                        {
//                            z = 0;
//                            alpha += 2;
//                            column = 0;

//                            if (alpha == 120)
//                            {
//                                r += 2;
//                                alpha = 0;
//                            }
//                        }
//                        i = -1; // для поиска заново
//                        continue;
//                    }

//                    i = -1; // для поиска заново
//                    z++;

//                    if (isThreeRow)
//                    {
//                        if (z == 3 + column)
//                        {
//                            z -= 3;
//                            alpha++;
//                            isThreeNodes = !isThreeNodes;
//                            countRow++;
//                        }

//                        if (countRow == 3)
//                        {
//                            countRow = 0;
//                            isThreeRow = !isThreeRow;
//                            alpha -= 3;
//                            r++;
//                        }
//                    }
//                    else
//                    {
//                        if (z == 3 + column)
//                        {
//                            z -= 3;
//                            alpha += 2;
//                            countRow++;
//                        }

//                        if (countRow == 2)
//                        {
//                            countRow = 0;
//                            isThreeRow = !isThreeRow;
//                            isThreeNodes = !isThreeNodes;
//                            alpha -= 4;
//                            r++;
//                        }
//                    }

//                    if ((z == 1 + column) && !isThreeNodes)
//                        z++;
//                }
//            }
//        }

//        public static void TimeСycle(List<Area> areas, int stepsCount, int cyclesPerStepCount)
//        {
//            int cycleCounter = 0;
//            //double shortCrackTotalProbability = 0;

//            while (cycleCounter <= cyclesPerStepCount * stepsCount) //ЦИКЛ ПО ВРЕМЕНИ
//            {
//                double shortCrackTotalProbability = 0;

//                AreaCycle(areas, cyclesPerStepCount, ref shortCrackTotalProbability);

//                if (areas.Exists(x => x.ShortCracks.Exists(y => y.SIF >= 3)))
//                {
//                    SendResultToForm(areas, cycleCounter);
//                    return;
//                }


//                if (shortCrackTotalProbability < 0.5)
//                { }

//                if (shortCrackTotalProbability >= 0.5)
//                {
//                    List<int> lineNumber = new List<int>();

//                    var minShortCrackProbability = areas.FindAll(x => x.ShortCrackProbability != 0).Min(y => y.ShortCrackProbability);

//                    for (int areaCounter = 0; areaCounter < areas.Count(); areaCounter++)
//                    {
//                        int areaNumberOnLine = (int)Math.Round(areas[areaCounter].ShortCrackProbability / minShortCrackProbability);
//                        for (int i = 0; i < areaNumberOnLine; i++)
//                            lineNumber.Add(areaCounter);
//                    }
//                    Random rand = new Random();

//                    int ShortCrackAreaIndex = lineNumber[rand.Next(lineNumber.Count())];

//                    ShortCrack shortCrack = new ShortCrack();
//                    shortCrack.Length = 0.0002;

//                    double B_tr = 0.0125; // ширина подшипника
//                    double h_tr = 0.002; // толщина баббитового слоя (должна варьироваться)
//                    double a_tr = shortCrack.Length;
//                    double b_tr = (a_tr / 2) < h_tr ? (a_tr / 2) : h_tr;
//                    double f_g = 1, f_teta = 1;
//                    //  f_g = 1 - почему?
//                    double f_B = Math.Sqrt(1 / Math.Cos((Math.PI * a_tr / B_tr) * Math.Sqrt(b_tr / h_tr)));

//                    double F1 = 1.04; 
//                    double F2 = 0.202;
//                    double F3 = -1.06;
//                    double F = (F1 + (F2 * Math.Pow((b_tr / h_tr), 2)) + (F3 * Math.Pow((b_tr / h_tr), 4))) * f_teta * f_g * f_B;

//                    shortCrack.SIF = (areas[ShortCrackAreaIndex].Stress.Radial + areas[ShortCrackAreaIndex].Stress.MaxPrincipal) * (Math.Sqrt(Math.PI * (shortCrack.Length / 2)) / (Math.PI / 2)) * F; // F_function; // тут не должно быть NaN

//                    areas[ShortCrackAreaIndex].ShortCracks.Add(shortCrack);
//                }

//                cycleCounter += cyclesPerStepCount;
//            }
//        }

//        public static void AreaCycle(List<Area> areas/*, int cycleCounter*/, int cyclesPerStepCount, ref double shortCrackTotalProbability)
//        {
//            foreach (var area in areas)
//            {
//                area.DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((area.Stress.Radial + area.Stress.MaxPrincipal), 5.1);
//                area.Damage += area.DamageAccumulationRate * cyclesPerStepCount;
//                area.Value = ((1.021 * Math.Pow(10, -6)) / areas.Count); // ПЕРЕСЧИТАТЬ!!!!!!! // вроде разобрался с этим: "видимо, areasCount нужен строго для модели с толщиной слоя 2 мм, ведь 

//                area.StructuralElementsCount = Math.Round(area.Value * 4000 * Math.Pow(10, 9), 0); // 4000 * 1000 - структурных элементов на одном участке(НЕТ. Это в 1 м^3)
//                area.DestroyedStructuralElementsCount = Math.Round(area.StructuralElementsCount * area.Damage);

//                if (area.ShortCracks.Count() > 0)
//                {
//                    double V_sum = 0; // тут ли обнулять? В восьмой студии только одно обнуление в самом начале проги... -- скорее тут

//                    //ShortCracksCycle(areas/*, cycleCounter*/, areasCounter, cyclesPerStepNumber, ref V_sum);
//                    ShortCracksCycle(area, areas.Count(), cyclesPerStepCount, ref V_sum); // V_sum не должен быть NaN

//                    if (area.ShortCracks.Exists(x => x.SIF >= 3))
//                        return;

//                    if (V_sum > area.Value)
//                        V_sum = area.Value;

//                    double koef = V_sum / area.Value; // коэфф. для расчета в следующей строке

//                    if (double.IsNaN(area.ShortCrackProbability))
//                    { }
//                    area.DestroyedStructuralElementsCount = area.DestroyedStructuralElementsCount - Math.Round(area.DestroyedStructuralElementsCount * koef);

//                    if (double.IsNaN(area.DestroyedStructuralElementsCount))
//                    { }

//                    if (area.DestroyedStructuralElementsCount < 0)
//                        area.DestroyedStructuralElementsCount = 0;
//                }
//                // может стоит обнулять
//                    area.ShortCrackProbability = 1 - (Math.Pow((1 - area.Damage), area.DestroyedStructuralElementsCount));

//                if(double.IsNaN(area.ShortCrackProbability))
//                { }

//                    if (area.ShortCrackProbability > 1)
//                        area.ShortCrackProbability = 1;
//                    else if (area.ShortCrackProbability < 0)
//                        area.ShortCrackProbability = 0;

//                    shortCrackTotalProbability += area.ShortCrackProbability;

//                if (double.IsNaN(shortCrackTotalProbability))
//                { }

//                if (shortCrackTotalProbability > 1)
//                        shortCrackTotalProbability = 1;
//                    else if (shortCrackTotalProbability < 0)
//                        shortCrackTotalProbability = 0;
//            }
//        }

//        public static void ShortCracksCycle(Area area, int areasCount, int cyclesPerStepCount, ref double V_sum)
//        {
//            //area.Value = ((1.021 * Math.Pow(10.0, -6.0)) / areasCount); // вроде разобрался с этим: "видимо, areasCount нужен строго для модели с толщиной слоя 2 мм, ведь 

//            //area.Value = ((4.9052 * Math.Pow(10.0, -4.0)) / areasCount) * 0.002; //ОБЪЕМ ДРУГОЙ! пОТОМУ ЧТО УЧАСТКОВ СТАЛО БОЛЬШЕ, А ТОЛЩИНА НЕ ИЗМЕНИЛАСЬ

//            foreach (var shortCrack in area.ShortCracks)
//            {
//                shortCrack.DevelopmentRate = Math.Pow(10, -6) * Math.Pow(shortCrack.SIF, 5);

//                shortCrack.Length += shortCrack.DevelopmentRate * cyclesPerStepCount;

//                if (shortCrack.Length > 0.005) //насколько это корректно (типа выставлять костыльно КИН и длину)
//                {
//                    shortCrack.SIF = 3;
//                    shortCrack.Length = 0.005;

//                    return;
//                }
//                else
//                {
//                    //double B_tr = 0.0125, h_tr = 0.002, f_g = 1, f_teta = 1; // B_tr должно быть равно 12.5 мм, а не 6 мм
//                    double B_tr = 0.0125; // ширина подшипника
//                    double h_tr = 0.002; // толщина баббитового слоя (должна варьироваться)
//                    double a_tr = shortCrack.Length;
//                    double b_tr = (a_tr / 2) < h_tr ? (a_tr / 2) : h_tr;
//                    double f_g = 1, f_teta = 1; // B_tr должно быть равно 12.5 мм, а не 6 мм
//                    //  f_g = 1 - почему?
//                    double f_B = Math.Sqrt(1 / Math.Cos((Math.PI * a_tr / B_tr) * Math.Sqrt(b_tr / h_tr))); // в маткаде проверил, что тут NaN быть не должно

//                    double F1 = 1.04; //1.13 - 0.09 * (b_tr / a_tr);
//                    double F2 = 0.202; //-0.54 + 0.89 * (0.2 + (b_tr / a_tr));
//                    double F3 = -1.06; //0.5 - (1 / (0.65 + (b_tr / a_tr))) + (14 * (1 - (b_tr / a_tr)));
//                    double F = (F1 + (F2 * Math.Pow((b_tr / h_tr), 2)) + (F3 * Math.Pow((b_tr / h_tr), 4))) * f_teta * f_g * f_B;

//                    //double F_function = (1.04 + (0.202 * Math.Pow((shortCrack.Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((shortCrack.Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
//                    shortCrack.SIF = (area.Stress.Radial + area.Stress.MaxPrincipal) * (Math.Sqrt(Math.PI * (shortCrack.Length / 2)) / (Math.PI / 2)) * F; // F_function; // тут не должно быть NaN

//                    if (shortCrack.SIF < 0)
//                    { }
//                }

//                //double r0 = (1 / (2 * Math.PI)) * (shortCrack.SIF / (area.Stress.Radial + area.Stress.MaxPrincipal));
//                double r0 = (1 / (2 * Math.PI)) * Math.Pow((shortCrack.SIF / (area.Stress.Radial + area.Stress.MaxPrincipal)), 2);
//                double r90 = r0 / 1.125;
//                //double b; // = Math.Sqrt((r0 * r0) + (r90 * r90));
//                //double S = (Math.PI * Math.Pow(((shortCrack.Length / 2) + r0), 2)) / 2;
//                double S = Math.PI * Math.Pow(r0, 2);
//                double V_tor = S * 2 * Math.PI * (shortCrack.Length / 2);

//                V_sum += V_tor;

//                if (double.IsNaN(V_sum))
//                { }

//                if (V_sum > area.Value * areasCount)
//                { }
//            }
//        }


//        public static void SendResultToForm(List<Area> areas, int cycleCounter)
//        {
//            Form1 form1 = new Form1();
//            form1.richTextBox1.Text += $"{cycleCounter}";
//        }

//        //public static void TimeСycle(List<Area> areas, int stepsCount, int cyclesPerStepCount)
//        //{
//        //    int cycleCounter = 0;
//        //    double shortCrackTotalProbability = 0;

//        //    while (cycleCounter <= cyclesPerStepCount * stepsCount) //ЦИКЛ ПО ВРЕМЕНИ
//        //    {
//        //        AreaCycle(areas, cyclesPerStepCount, ref shortCrackTotalProbability);

//        //        if (shortCrackTotalProbability < 0.5)
//        //        { }

//        //        if (shortCrackTotalProbability >= 0.5)
//        //        {
//        //            List<double> ShortCrackProbabilityList = new List<double>();
//        //            List<int> lineNumber = new List<int>();

//        //            //foreach (Area area in areas)
//        //            //    if (area.ShortCrackProbability != 0)
//        //            //        ShortCrackProbabilityList.Add(area.ShortCrackProbability);

//        //            //double minShortCrackProbability = ShortCrackProbabilityList.Min();


//        //            // double minShortCrackProbability = areas.Min(x => x.ShortCrackProbability);
//        //            var minShortCrackProbability = areas.FindAll(x => x.ShortCrackProbability != 0).Min(y => y.ShortCrackProbability);

//        //            //foreach (Area area in areas)
//        //            //    if (lineNumber.Count() == 0)
//        //            //        lineNumber.Add(Math.Round(area.ShortCrackProbability / minShortCrackProbability));
//        //            //    else
//        //            //        lineNumber.Add(lineNumber.Last() + Math.Round(area.ShortCrackProbability / minShortCrackProbability));


//        //            //foreach (Area area in areas)
//        //            for (int areaCounter = 0; areaCounter < areas.Count(); areaCounter++)
//        //            {
//        //                int areaNumberOnLine = (int)Math.Round(areas[areaCounter].ShortCrackProbability / minShortCrackProbability);
//        //                for (int i = 0; i < areaNumberOnLine; i++)
//        //                    lineNumber.Add(areaCounter);
//        //            }
//        //            Random rand = new Random();

//        //            int ShortCrackAreaIndex = lineNumber[rand.Next(lineNumber.Count())];


//        //            // так, надо переделать внизу!!!!!!!!!!!!!!!!!!!!!111
//        //            ShortCrack shortCrack = new ShortCrack();
//        //            shortCrack.Length = 0.0002;

//        //            double B_tr = 0.0125, h_rt = 0.002, f_g = 1.0; // B_tr должно быть равно 12.5 мм, а не 6 мм
//        //            //h_rt должно варьироваться, это ж толщина слоя 

//        //            double f_B = Math.Sqrt(1 / (Math.Cos((Math.PI * (shortCrack.Length / 2) / B_tr) * Math.Sqrt(shortCrack.Length / (2 * h_rt)))));
//        //            double F_function = (1.04 + (0.202 * Math.Pow((shortCrack.Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((shortCrack.Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
//        //            shortCrack.SIF = (Math.Abs(areas[ShortCrackAreaIndex].Stress.Radial) + Math.Abs(areas[ShortCrackAreaIndex].Stress.MaxPrincipal)) * (Math.Sqrt(Math.PI * (shortCrack.Length / 2)) / (Math.PI / 2)) * F_function;

//        //            areas[ShortCrackAreaIndex].ShortCracks.Add(shortCrack);
//        //        }

//        //        cycleCounter += cyclesPerStepCount;
//        //        //    stepsCounter++;
//        //        //}
//        //    }
//        //}
//    }
//}
