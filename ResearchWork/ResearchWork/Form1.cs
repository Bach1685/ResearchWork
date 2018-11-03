using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ResearchWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           // button1.Click += button1_Click;
            openFileDialog1.Filter = "Text files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
          //  saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(openFileDialog1.FileName); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку

            int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;  //последняя заполненная строка в столбце А
            var arrData = (object[,])ObjWorkSheet.Range["A1:O" + iLastRow].Value;



            List<Area> area = new List<Area>();

            Methods.Systematizing(area, arrData, iLastRow);






           


            //for (int i = 0; i < 5703; i++)
            //{
            //    area.Add(new Area() { });
            //    area[i].RadialStress = (double)arrData[i, 1];
            //    richTextBox1.Text = "" + area[i].RadialStress;
            //}
            // создать объект польз. типа и добавляем его потом в цикле в список 

            //for (int i = 0; i < iLastRow; i++) //по всем колонкам
            //{
            //    Area newArea = new Area();
            //    newArea.RadialStress = arrData[i+1,1];
            //    area.Add(newArea);
            //}


            //for (int i = 0; i < 5703; i++)
            //{
            //    area.Add(new Area());
            //    int iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;
            //    area[i].RadialStress = (object)ObjWorkSheet.Range["A1:Z" + iLastRow].Value;
            //    richTextBox1.Text = "" + area[i].RadialStress;
            //}

            //foreach(RadialStress in area)
            //{

            //}area.Add(new Area() { });
            // area.Add(new Area());
            // area[0].RadialStress = (double)arrData[1, 1];



            //  ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя

            //Console.WriteLine(arrData[2, 2]);

            // textBox1.Text = "" + arrData[5700, 1];
            // textBox1.Text = "" + area[0].RadialStress;

            ObjWorkExcel.Quit(); // выйти из экселя












            // получаем выбранный файл
          //  string filename = openFileDialog1.FileName;
            // читаем файл в строку
           // string fileText = System.IO.File.ReadAllText(filename);
           // textBox1.Text = fileText;

          //  richTextBox1.Text = System.IO.File.ReadAllText(filename, Encoding.GetEncoding(1251));
          //  MessageBox.Show("Файл открыт");
        }
    }
}
