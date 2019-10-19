using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ResearchWork
{
    public partial class Form1 : Form
    {
        public object[,] stressCash;
        public int iLastRow;

        public Form1()
        {
            InitializeComponent();
           // button1.Click += button1_Click;
            openFileDialog1.Filter = "Text files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            //  saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            //label1.Text = "Начальная поврежденность";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Area> areas = new List<Area>();
            double InitialDestroyedStructuralElementsCount = 0;
            int stepsCount = 0;
            int cyclesPerStepCount = 0;

            try
            {
                InitialDestroyedStructuralElementsCount = Convert.ToDouble(textBox1.Text);
                cyclesPerStepCount = Convert.ToInt32(textBox3.Text);
                stepsCount = Convert.ToInt32(textBox4.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Некорректный ввод!");
                return;
            }

            // Task task = Task.Run(() => Methods.Systematizing(area, arrData, iLastRow));

            Methods.Systematizing(areas, stressCash, iLastRow); // нужно попробовать работать не со списком, а с очередью какой-нибудь или стеком
            // или просто удалять элемент из списка...
            foreach (Area area in areas)
            {
                area.InitialDestroyedStructuralElementsCount = Convert.ToDouble(InitialDestroyedStructuralElementsCount);
               // area.StructuralElementsCount = 4000; // перенести
            }
            Models md = new Models();

            Methods.TimeСycle(areas, stepsCount, cyclesPerStepCount); // надо проверить, как передается ShortCrackTotalProbability

            //Mdestructed0 = Math.Round((numberStructuralElements * damage0),0); //Зная поврежденность и общее количество СЭ, можно узнать количество разрушенных СЭ;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(openFileDialog1.FileName); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку

            iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;  //последняя заполненная строка в столбце А
            stressCash = (object[,])ObjWorkSheet.Range["A1:O" + iLastRow].Value;

            textBox2.Text = openFileDialog1.FileName;

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя

            ObjWorkExcel.Quit(); // выйти из экселя
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
