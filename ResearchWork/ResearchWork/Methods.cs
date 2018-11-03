﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchWork
{
    class Methods
    {
        public static void Systematizing (List<Area> Area, object[,] arrData, int iLastRow)
        {
            double[] recordMaxPrincipal = new double[20];
            double[] recordRadial = new double[20];
         //   int i;

          //  double r;
            int z = 0, alpha = 0, r = 0;
            int a = 0;
            bool isThreeNodes = true;
            double countRow = 0, countDeep = 1;

            for (int i = 0; i < iLastRow; i++)
            {
                if (i == 40000)
                {

                }
                if ((double)arrData[i + 1, 4] > ((-12.5 + 0.42 * z) - 0.3) && (double)arrData[i + 1, 4] < ((-12.5 + 0.42 * z) + 0.3)
                    && ((double)arrData[i + 1, 6] > (90 - (0.5 * alpha)) && (double)arrData[i + 1, 6] < (90 + (0.5 * alpha)))
                    && ((double)arrData[i + 1, 5] > (25 - (0.4 * r)) && (double)arrData[i + 1, 5] < (25 + (0.4 * r))))
                {
                    recordMaxPrincipal[a] = (double)arrData[i + 1, 11];
                    recordRadial[a] = (double)arrData[i + 1, 13];
                    a++;

                    i = 0;

                    if (i == 40000)
                    {

                    }

                    if (isThreeNodes)
                    {
                        z++;
                        //if (z == 2)
                        //{
                        //    z -= 2;
                        //    alpha++;
                        //    isThreeNodes = !isThreeNodes;
                        //    countRow++;
                        //}
                    }
                    else
                    {
                        z += 2;
                        //if (z == 2)
                        //{
                        //    z -= 2;
                        //    alpha++;
                        //    if (countDeep % 2 != 0)
                        //        isThreeNodes = !isThreeNodes;
                        //    countRow++;
                        //}
                    }
                    if (z == 2)
                    {
                        z -= 2;
                        alpha++;
                        if (countDeep % 2 != 0)
                        {
                            isThreeNodes = !isThreeNodes;
                            countRow++;
                        }
                        else
                            countRow += 2;
                    }

                    if (countRow == 2)
                    {
                        r++;
                        alpha -= 3;
                        isThreeNodes = !isThreeNodes;
                        countDeep++;
                    }



                }
            }





            //    for (int j = 0; j < iLastRow; j++) // находим узел отсчета
            //{
            //    if (arrData[i + 1, 4] == -12.5 && arrData[i + 1, 6] == 90)
            //    {
            //        recordMaxPrincipal[0] = arrData[i + 1, 11];
            //        recordRadial[0] = arrData[i + 1, 13];
            //    }
            //}

            //for (i = 0; i < iLastRow; i++) // находим узел отсчета
            //{
            //    if (arrData[i + 1, 4] == (-12.5+) && arrData[i + 1, 6] == 90 && (arrData[i + 1, 5] > 24.96 && arrData[i + 1, 5] < 25.04))
            //    {
            //        recordMaxPrincipal[0] = arrData[i + 1, 11];
            //        recordRadial[0] = arrData[i + 1, 13];
            //        break;
            //    }
            //}

           



            //for (int i = 0; i < iLastRow; i++)
            //    if (arrData[i+1, 4] == -12.5 && arrData[i+1, 6] == 90) // находим узел с самой верхней левой координатой, чтобы сообщить нулевому участку всю нужную информацию от этого узла 
            //    {
            //        recordMaxPrincipal[0] = arrData[i + 1, 11];

            //        Area area = new Area();
            //      //  area.Coordinate.Z = arrData[i + 1, 4];
            //      //  area.Coordinate.Alpha = arrData[i + 1, 6];
            //        //area.Stress.Rad = 
            //        //    // нужно среднее арифметическое брать с соседних узлов
            //        //sigma1_stress[0] = sigma1_stress[i];
            //        //radial_stress[0] = radial_stress[i];
            //       // break;
            //    }
            //// а может рекурсивная функция тут нужна

            //w = 1;
            //for (i = 0; i < number_of_nodes; i++) // тут я сортирую узлы, чтобы сообщить каждому (кроме нулевого) участку правильное 
            //                                      //	значение координаты и напряжений 
            //{
            //    for (j = 0; j < number_of_nodes; j++)
            //    {
            //        if (abs(ALPHA[i] - ALPHA[j]) < 0.5 && (Z[i] - Z[j]) < -0.6 && (Z[i] - Z[j]) > -1.0)
            //        {
            //            if (Z[j] == 0)
            //            {
            //                for (k = 0; k < number_of_nodes; k++)
            //                    if ((ALPHA[i] - ALPHA[k]) > 0.1 && (ALPHA[i] - ALPHA[k]) < 1.6 && (Z[i] - Z[k]) < 11.9 && (Z[i] - Z[k]) > 11.3)
            //                    {
            //                        Z[i + 1] = Z[k];
            //                        ALPHA[i + 1] = ALPHA[k];
            //                        radial_stress[i + 1] = radial_stress[k];
            //                        sigma1_stress[i + 1] = sigma1_stress[k];

            //                        w++;
            //                        break;
            //                    }
            //                break;
            //            }

            //            Z[i + 1] = Z[j];
            //            ALPHA[i + 1] = ALPHA[j];
            //            radial_stress[i + 1] = radial_stress[j];
            //            sigma1_stress[i + 1] = sigma1_stress[j];

            //            k++;
            //            break;
            //        }
            //    }
            //    if (w == 60)
            //        break;
            //}
            //// в итоге получаем, что, к примеру, элемент массива radial_stress[0] - это радиальные напряжения на участке №0,  radial_stress[1] - на участке №1 и так далее

        }

        //public double Recurs (double Z, double[,] arrData)
        //{
        //    if(Z)


        //        (Z[i + 1, 4] - Z[j]) < -0.6 && (Z[i] - Z[j]) > -1.0

        //}
    }
}
