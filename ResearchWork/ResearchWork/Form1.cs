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
        public object[,] stressesCash;
        public int iLastRow;
        private Task FillStressesCashTask;
        internal List<Area> areas;

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
            FillStressesCashTask.Wait();
            double initialDamage = 0;
            int stepsCount = 0;
            int cyclesPerStepCount = 0;

            try
            {
                initialDamage = Convert.ToDouble(textBox1.Text);
                cyclesPerStepCount = Convert.ToInt32(textBox3.Text);
                stepsCount = Convert.ToInt32(textBox4.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Некорректный ввод!");
                return;
            }

            areas.ForEach(x => x.Damage = initialDamage); 

            Calculation.TimeСycle(areas, stepsCount, cyclesPerStepCount, out string Result); // надо проверить, как передается ShortCrackTotalProbability
  
            richTextBox1.AppendText(Result);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }


        private void ReviewButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            FillStressesCashTask = Task.Run(() => FillStressesCash(openFileDialog1));
            //FillStressesCashTask.Wait();

            textBox2.Text = openFileDialog1.FileName;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void FillStressesCash(OpenFileDialog openFileDialog1)
        {
            Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(openFileDialog1.FileName); //открыть файл
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист

            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку

            iLastRow = ObjWorkSheet.Cells[ObjWorkSheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;  //последняя заполненная строка в столбце А
            stressesCash = (object[,])ObjWorkSheet.Range["A1:O" + iLastRow].Value;

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя

            ObjWorkExcel.Quit(); // выйти из экселя

            areas = new List<Area>();

            Calculation.Systematizing(areas, stressesCash, iLastRow); // нужно попробовать работать не со списком, а с очередью какой-нибудь или стеком
            // или просто удалять элемент из списка...
            areas.ForEach(area => area.Volume = ((1.021 * Math.Pow(10, -6)) / areas.Count));
            areas.ForEach(area => area.StructuralElementsCount = Math.Round(area.Volume * 4000 * Math.Pow(10, 9), 0)); // 4000 * 1000 - структурных элементов  в 1 м^3

            MakeButtonEnabled(button1);
        }

        private void MakeButtonEnabled(Button button)
        {
            if (button.InvokeRequired)
            {
                button.Invoke(new Action(() =>
                {
                    button.Enabled = true;
                }));
            }
            else
            {
                button.Enabled = true;
            }
        }

        private void ToRichTextBox(string text)
        {
            if (InvokeRequired)
            {
                
                Invoke((Action<string>)ToRichTextBox, text);
            }
            else
                richTextBox1.AppendText(text);
        }

        internal void ShowResult(List<Area> areas, int cycleCount)
        {
            richTextBox1.Text += "Text";

            //richTextBox1.AppendText(cycleCount.ToString());
        }
    }
}
