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
                        ar.Stress.Radial = Math.Abs(recordRadial.Sum() / recordRadial.Length); // суммируются не модульные выражения
                        ar.Stress.MaxPrincipal = Math.Abs(recordMaxPrincipal.Sum() / recordMaxPrincipal.Length);
                        area.Add(ar);

                        if (area.Count == 900)
                        { break; } // удалить!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
    }
}
