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
        public object[,] arrData;
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
            //if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            //    return;

            //Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            //Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(openFileDialog1.FileName); //открыть файл
            //Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

            //var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку

            //int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;  //последняя заполненная строка в столбце А
            //var arrData = (object[,])ObjWorkSheet.Range["A1:O" + iLastRow].Value;

           
            //textBox2.Text = openFileDialog1.FileName;

            List<Area> area = new List<Area>();

            double Mdestr0 = 0;

            try
            {
                Mdestr0 = Convert.ToDouble(textBox1.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Некорректный ввод!");
                return;
            }

            foreach (Area elem in area)
                elem.Mdestr0 = Convert.ToDouble(Mdestr0);

            // Task task = Task.Run(() => Methods.Systematizing(area, arrData, iLastRow));

            Methods.Systematizing(area, arrData, iLastRow);
            for (int i = 0; i < 100; i++)
            {
                richTextBox1.Text += Math.Round((double)area[i].Stress.Radial, 3) + " ";
                if ((i + 1) % 15 == 0)
                    richTextBox1.Text += "\n";
            }



            //ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя



            //ObjWorkExcel.Quit(); // выйти из экселя


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
            arrData = (object[,])ObjWorkSheet.Range["A1:O" + iLastRow].Value;

            textBox2.Text = openFileDialog1.FileName;

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя

            ObjWorkExcel.Quit(); // выйти из экселя
        }
    }
}
