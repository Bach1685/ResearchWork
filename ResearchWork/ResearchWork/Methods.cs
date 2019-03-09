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

                        //if (area.Count == 100)
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
                AreaCycle(areas, cycleCounter, cyclesPerStepNumber, ref shortCrackTotalProbability);

                if(shortCrackTotalProbability >= 0.5)
                {
                    List<double> ShortCrackProbabilityList = new List<double>();
                    List<int> lineNumber = new List<int>();

                    foreach (Area area in areas)
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
                    shortCrack.SIF = (areas[ShortCrackAreaIndex].Stress.Radial + areas[ShortCrackAreaIndex].Stress.MaxPrincipal) * (Math.Sqrt(3.14 * (shortCrack.Length / 2)) / (3.14 / 2)) * F_function;

                    areas[ShortCrackAreaIndex].ShortCracks.Add(shortCrack);
                }

                    cycleCounter += cyclesPerStepNumber;
                //    stepsCounter++;
                //}
            }
        }

        public static void AreaCycle(List<Area> areas, int cycleCounter, int cyclesPerStepNumber, ref double shortCrackTotalProbability)
        {
            for (int areasCounter = 0; areasCounter < areas.Count(); areasCounter++) // ЦИКЛ ПО УЧАСТКАМ
            {
                //areas[areasCounter].Damage = cycleCounter == 0 ? areas[areasCounter].Damage0 : ;
                areas[areasCounter].DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal), 5.1);
                areas[areasCounter].Damage += areas[areasCounter].DamageAccumulationRate * cyclesPerStepNumber;
                areas[areasCounter].DestroyedStructuralElementsNumber = Math.Round(areas[areasCounter].StructuralElementsNumber * areas[areasCounter].Damage);

                if (areas[areasCounter].ShortCracks.Count() > 0)
                {
                    double V_sum = 0; // тут ли обнулять? В восьмой студии только одно обнуление в самом начале проги 

                    ShortCracksCycle(areas, cycleCounter, areasCounter, cyclesPerStepNumber, ref V_sum);

                    double koef = V_sum / areas[areasCounter].Value; // коэфф. для расчета в следующей строке

                    areas[areasCounter].DestroyedStructuralElementsNumber = areas[areasCounter].DestroyedStructuralElementsNumber - Math.Round(areas[areasCounter].DestroyedStructuralElementsNumber * koef);

                    areas[areasCounter].ShortCrackProbability = 1.0 - (Math.Pow((1.0 - areas[areasCounter].Damage), areas[areasCounter].DestroyedStructuralElementsNumber));

                    if (areas[areasCounter].ShortCrackProbability > 1)
                        areas[areasCounter].ShortCrackProbability = 1;
                    else if (areas[areasCounter].ShortCrackProbability < 0)
                        areas[areasCounter].ShortCrackProbability = 0;

                    shortCrackTotalProbability += areas[areasCounter].ShortCrackProbability;

                    if (shortCrackTotalProbability > 1)
                        shortCrackTotalProbability = 1;
                    else if (shortCrackTotalProbability < 0)
                        shortCrackTotalProbability = 0;
                }
            }
        }

        public static void ShortCracksCycle(List<Area> areas, int cycleCounter, int areasCounter, int numberCyclesPerStep, ref double V_sum)
        {
            areas[areasCounter].Value = ((4.9052 * Math.Pow(10.0, -4.0)) / areas.Count()) * 0.002;

            for (int shortCrackCounter = 0; shortCrackCounter < areas[areasCounter].ShortCracks.Count(); shortCrackCounter++) // ЦИКЛ ПО КОРОТКИМ ТРЕЩИНАМ
            {
                areas[areasCounter].ShortCracks[shortCrackCounter].DevelopmentRate = Math.Pow(10.0, -6.0) * Math.Pow(areas[areasCounter].ShortCracks[shortCrackCounter].SIF, 5.0);

                areas[areasCounter].ShortCracks[shortCrackCounter].Length += areas[areasCounter].ShortCracks[shortCrackCounter].DevelopmentRate * numberCyclesPerStep;

                if (areas[areasCounter].ShortCracks[shortCrackCounter].Length > 0.005)
                {
                    areas[areasCounter].ShortCracks[shortCrackCounter].SIF = 3;
                    areas[areasCounter].ShortCracks[shortCrackCounter].Length = 0.005;
                }
                else
                {
                    double B_tr = 0.006, h_rt = 0.002, f_g = 1.0;

                    double f_B = Math.Sqrt(1 / (Math.Cos((3.14 * (areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / B_tr) * Math.Sqrt(areas[areasCounter].ShortCracks[shortCrackCounter].Length / (2 * h_rt)))));
                    double F_function = (1.04 + (0.202 * Math.Pow((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / 0.002, 2)) + ((-1.06) * Math.Pow(((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
                    areas[areasCounter].ShortCracks[shortCrackCounter].SIF = (areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal) * (Math.Sqrt(3.14 * (areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2)) / (3.14 / 2)) * F_function;
                }

                double r0 = (1 / (2.0 * 3.14)) * (areas[areasCounter].ShortCracks[shortCrackCounter].SIF / (areas[areasCounter].Stress.Radial + areas[areasCounter].Stress.MaxPrincipal));
                double r90 = r0 / 1.125;
                double b = Math.Sqrt((r0 * r0) + (r90 * r90));
                double F = (3.14 * Math.Pow(((areas[areasCounter].ShortCracks[shortCrackCounter].Length / 2) + r0), 2)) / 2;
                double V = F * 2 * r90;
                V_sum += V;
            }
        }




                //_______________________________________________________________________________________новодел
                //_______________________________________________________________________________________новодел
                //_______________________________________________________________________________________новодел

                //if (cycle == 0) // если цикл нулевой
                //{
                //    areas[i].Damage = areas[i].Damage0;
                //    Pa2[i] = 1.0 - pow(1.0 - damage[i], M_destructed0);
                //}
                //else // а если ненулевой:
                //{
                //    rate_of_damage_accumulation[i] = 0.224 * pow(10.0, -17.0) * pow((radial_stress[i] + sigma1_stress[i]), 5.1); // скорость накопения повреждений на i-м участке
                //    damage[i] += rate_of_damage_accumulation[i] * step; // поврежденность на i-м участке
                //    M_destructed[i] = floor(M[i] * damage[i] + 0.5); //количество разрушенных СЭ;

                //    if (number_short_crack_in_Itoi_area[i] != 0) // если на i-м участке есть КТ, то происходит расчет нового кол-во разруш. СЭ на этом участке
                //                                                 // также здесь расчитывается новая длина и КИН каждой трещины
                //    {
                for (j = 0; j < counter_of_short_cracks; j++) // цикл по коротким трещинам
                {
                    if (number_area_for_short_crack[j] == i) // если номер участка для j-й КТ совпадает с номером рассматриваемого i-го участка, тогда:
                    {
                        rate_of_development_of_short_crack[j] = pow(10.0, -6.0) * pow(KI[j], 5.0); // скорость роста трещины
                        length_of_short_crack[j] += rate_of_development_of_short_crack[j] * step; // длина трещины

                        if (length_of_short_crack[j] < 0.005)
                            richTextBox2->Text += "Длина трещины № " + (j + 1) + ": " + String::Format("{0:F4}", length_of_short_crack[j]) + " м на участке №" + (number_area_for_short_crack[j] + 1) + ".\n";

                        if (length_of_short_crack[j] > 0.005)
                        {
                            tt++; // потом будет условие, где понадобится значение tt, которое сперва равно нулю 
                            richTextBox2->Text += "Длина трещины № " + (j + 1) + " превысила 5 мм.\n";
                            richTextBox2->Text += "КИН трещины № " + (j + 1) + " превысил 3 МПа" + String::Format("{0}", Convert::ToChar(0x221A)) + "м.\n";
                            KI[j] = 3;
                            length_of_short_crack[j] = 0.005;
                        }

                        else
                        {
                            B_tr = 0.006;
                            h_rt = 0.002;
                            f_g = 1.0;
                            f_B = sqrt(1 / (cos((3.14 * (length_of_short_crack[j] / 2) / B_tr) * sqrt(length_of_short_crack[j] / (2 * h_rt)))));
                            F_function = (1.04 + (0.202 * pow((length_of_short_crack[j] / 2) / 0.002, 2)) + ((-1.06) * pow(((length_of_short_crack[j] / 2) / 0.002), 4))) * 1.0 * f_g * f_B;
                            KI[j] = (radial_stress[i] + sigma1_stress[i]) * (sqrt(3.14 * (length_of_short_crack[j] / 2)) / (3.14 / 2)) * F_function;
                            richTextBox2->Text += "КИН трещины № " + (j + 1) + ": " + String::Format("{0:F4}", KI[j]) + " МПа" + String::Format("{0}", Convert::ToChar(0x221A)) + "м на участке №" + (number_area_for_short_crack[j] + 1) + ".\n";
                        }

                        r0 = (1 / (2.0 * 3.14)) * (KI[j] / (radial_stress[i] + sigma1_stress[i]));
                        r90 = r0 / 1.125;
                        b = sqrt((r0 * r0) + (r90 * r90));
                        F = (3.14 * pow(((length_of_short_crack[j] / 2) + r0), 2)) / 2;
                        V = F * 2 * r90;
                        V_sum += V;
                    }
                }
                koef = V_sum / value_of_one_erea; // коэфф. для расчета в следующей строке
                M_destructed[i] = M_destructed[i] - floor((M_destructed[i] * koef) + 0.5);
            }

                    Pa2[i] = 1.0 - (pow((1.0 - damage[i]), M_destructed[i]));
                    if (Pa2[i] > 1)
                        Pa2[i] = 1;
                    if (Pa2[i] < 0)
                        Pa2[i] = 0;
                }

                Pa2_sum += Pa2[i]; // суммируем вер-ти возн. КТ со всех участков
                if (Pa2_sum > 1)
                    Pa2_sum = 1;
                if (Pa2_sum < 0)
                    Pa2_sum = 0;

                if (i == kk * nn / 20) // вот это надо, если программа будет долго считать. После 60 секунд расчета 
                                       // будет выскакивать предложение завершить работу программы. Вот чтобы этого не произошло,
                                       // нужно как бы сделать событие "do events",
                                       // но программа считает быстро, поэтому острой необходимости в этом нет
                {
                    kk++;
                    Application::DoEvents();
                }
            } // ЦИКЛ ПО УЧАСТКАМ ЗАКОНЧИЛСЯ

        }
    }

}
