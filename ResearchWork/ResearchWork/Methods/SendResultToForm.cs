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
        public static void SendResultToForm(List<Area> areas, int cycleCounter)
        {
            Form1 form1 = new Form1();
            form1.richTextBox1.Text += $"{cycleCounter}";
        }
    }
}
