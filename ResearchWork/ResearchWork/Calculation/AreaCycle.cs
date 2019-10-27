﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfueliteSolutionsRandomHelper;

namespace ResearchWork
{
    partial class Calculation
    {
        public static void AreaCycle(List<Area> areas, int cyclesPerStepCount, ref double shortCrackTotalProbability)
        {
            foreach (var area in areas)
            {
                area.DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((area.Stress.Radial + area.Stress.MaxPrincipal), 5.1);
                area.Damage += area.DamageAccumulationRate * cyclesPerStepCount;
                area.Volume = ((1.021 * Math.Pow(10, -6)) / areas.Count);

                area.StructuralElementsCount = Math.Round(area.Volume * 4000 * Math.Pow(10, 9), 0); // 4000 * 1000 - структурных элементов на одном участке(НЕТ. Это в 1 м^3)
                area.DestroyedStructuralElementsCount = Math.Round(area.StructuralElementsCount * area.Damage);

                if (area.ShortCracks.Count() > 0)
                {
                    double V_sum = 0; // тут ли обнулять? В восьмой студии только одно обнуление в самом начале проги... -- скорее тут

                    ShortCracksCycle(area, areas.Count(), cyclesPerStepCount, ref V_sum); // V_sum не должен быть NaN

                    if (V_sum > area.Volume)
                        V_sum = area.Volume;

                    if (double.IsNaN(area.ShortCrackProbability))
                    { }
                    area.DestroyedStructuralElementsCount = area.DestroyedStructuralElementsCount - Math.Round(area.DestroyedStructuralElementsCount * (V_sum / area.Volume));

                    if (double.IsNaN(area.DestroyedStructuralElementsCount))
                    { }

                    if (area.DestroyedStructuralElementsCount < 0)
                        area.DestroyedStructuralElementsCount = 0;
                }
                // может стоит обнулять
                area.ShortCrackProbability = 1 - (Math.Pow((1 - area.Damage), area.DestroyedStructuralElementsCount));

                if (double.IsNaN(area.ShortCrackProbability))
                { }

                if (area.ShortCrackProbability > 1)
                    area.ShortCrackProbability = 1;
                else if (area.ShortCrackProbability < 0)
                    area.ShortCrackProbability = 0;

                shortCrackTotalProbability += area.ShortCrackProbability;

                if (double.IsNaN(shortCrackTotalProbability))
                { }

                if (shortCrackTotalProbability > 1)
                    shortCrackTotalProbability = 1;
                else if (shortCrackTotalProbability < 0)
                    shortCrackTotalProbability = 0;
            }
        }
    }
}