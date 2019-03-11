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
        public double StructuralElementsNumber  { get; set; }

        /// <summary>
        ///  Количество разрушенных СЭ на участке
        /// </summary>
        public double DestroyedStructuralElementsNumber  { get; set; }

        /// <summary>
        /// Начальная поврежденность участка
        /// </summary>
        public double InitialDamage { get; set; }

        /// <summary>
        /// Начальное количество разрушенных СЭ
        /// </summary>
        public double InitialDestroyedStructuralElementsNumber { get; set; }

        public Stress Stress { get; set; }

        public Coordinates Coordinate { get; set; }

        public List<ShortCrack> ShortCracks { get; set; }

        public double DamageAccumulationRate { get; set; }

        public Area()
        {
            ShortCracks = new List<ShortCrack>();
            Stress = new Stress();
        }
        
    }
    class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Alpha { get; set; }
    }

     class Stress
    {
        public double Radial { get; set; }
        public double MaxPrincipal { get; set; }
    }


    class ShortCrack
    {
        public double Length { get; set; }
        public double SIF { get; set; }
        public double DevelopmentRate { get; set; }
    }
    class MackroCrack
    {
        public double length { get; set; }
        public double SIF { get; set; }
        public double DevelopmentRate { get; set; }
    }

    //class Stress
    //{
    //    public double RadialStress { get; set; }
    //    public double FirstMainStress { get; set; }
    //}

    class Record
    {
        public Stress Stress { get; set; }
    }




}
