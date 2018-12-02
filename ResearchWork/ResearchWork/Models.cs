﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchWork
{
    class Models
    {
        public List<Area> Areas = new List<Area>();
    }

    class Area
    {
        public double Damage { get; set; }
        public double M_destructed { get; set; }
        public double Damage0 { get; set; }

        /// <summary>
        /// Начальное количество разрушенных СЭ
        /// </summary>
        public double Mdestr0 { get; set; }

        public Stresses Stress { get; set; }
        public Coordinates Coordinate { get; set; }

        public List<ShortCrack> ShortCracks { get; set; }
        public Area()
        {
            ShortCracks = new List<ShortCrack>();
            Stress = new Stresses();
        }
        
    }
    class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Alpha { get; set; }
    }

     class Stresses
    {
        public object Radial { get; set; }
        public double MaxPrincipal { get; set; }
    }


    class ShortCrack
    {
        public double Length { get; set; }
        public double SIF { get; set; }
    }
    class MackroCrack
    {
        public double length { get; set; }
        public double SIF { get; set; }
    }

    //class Stress
    //{
    //    public double RadialStress { get; set; }
    //    public double FirstMainStress { get; set; }
    //}

    class Record
    {
        public Stresses Stress { get; set; }
    }




}
