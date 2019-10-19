using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchWork
{
    class Models
    {
        public List<Area> Areas = new List<Area>();

        /// <summary>
        /// Вероятность возникновения КТ во всем объеме
        /// </summary>
        public double ShortCrackTotalProbability { get; set; }
    }

    class Area
    {
        /// <summary>
        /// Объем участка
        /// </summary>
        public double Value { get; set; }
        
        /// <summary>
        /// Поврежденность участка
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Вероятность возникновения КТ на участке
        /// </summary>
        public double ShortCrackProbability  { get; set; }

        /// <summary>
        ///  Количество структурных элементов на участке
        /// </summary>
        public double StructuralElementsCount { get; set; }

        /// <summary>
        ///  Количество разрушенных СЭ на участке
        /// </summary>
        public double DestroyedStructuralElementsCount  { get; set; }

        /// <summary>
        /// Начальная поврежденность участка
        /// </summary>
        public double InitialDamage { get; set; }

        /// <summary>
        /// Начальное количество разрушенных СЭ
        /// </summary>
        public double InitialDestroyedStructuralElementsCount { get; set; }

        /// <summary>
        /// Напряжения
        /// </summary>
        public Stress Stress { get; set; }

        /// <summary>
        /// Короткие напряжения
        /// </summary>
        public List<ShortCrack> ShortCracks { get; set; }

        /// <summary>
        /// Скорость накопления напряжений
        /// </summary>
        public double DamageAccumulationRate { get; set; }

        public Area()
        {
            ShortCracks = new List<ShortCrack>();
            Stress = new Stress();
        }
    }

    class Stress
    {
        /// <summary>
        /// Радиальное напряжение
        /// </summary>
        public double Radial { get; set; }

        /// <summary>
        /// Первое главное напряжение
        /// </summary>
        public double MaxPrincipal { get; set; }
    }

    class ShortCrack
    {
        /// <summary>
        /// Длина
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// КИН
        /// </summary>
        public double SIF { get; set; }

        /// <summary>
        /// Скорость развития
        /// </summary>
        public double DevelopmentRate { get; set; }
    }

    class MackroCrack
    {
        public double Length { get; set; }
        public double SIF { get; set; }
        public double DevelopmentRate { get; set; }
    }
}
