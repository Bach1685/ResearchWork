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
        public static void AreaCycle(List<Area> areas, int cyclesPerStepCount, ref double shortCrackTotalProbability)
        {
            foreach (var area in areas)
            {
                FillDamageCharacteristics(area, cyclesPerStepCount);

                if (area.ShortCracks.Count() > 0)
                {
                    ShortCracksCycle(area, areas.Count(), cyclesPerStepCount, out double ExcludedVolume);

                    if (ExcludedVolume > area.Volume)
                        ExcludedVolume = area.Volume;

                    area.DestroyedStructuralElementsCount = area.DestroyedStructuralElementsCount - Math.Round(area.DestroyedStructuralElementsCount * (ExcludedVolume / area.Volume));
                }

                area.ShortCrackProbability = 1 - (Math.Pow((1 - area.Damage), area.DestroyedStructuralElementsCount));
                shortCrackTotalProbability += area.ShortCrackProbability;
            }
        }

        public static void FillDamageCharacteristics(Area area, int cyclesPerStepCount)
        {
            area.DamageAccumulationRate = 0.224 * Math.Pow(10.0, -17.0) * Math.Pow((area.Stress.Radial + area.Stress.MaxPrincipal), 5.1);
            area.Damage += area.DamageAccumulationRate * cyclesPerStepCount;
            area.DestroyedStructuralElementsCount = Math.Round(area.StructuralElementsCount * area.Damage);
        }
    }
}
