using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        int[] day_limit = {31,29,31,30,31,30,31,31,30,31,30,31};

        string selectedYear = "";
        string selectedMonth = "";
        string selectedDay = "";
        string selectedHour = "";
        string selectedMinute = "";

        public Form2()
        {
            InitializeComponent();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            
            for (int i = year - 1; i <= year + 1; i++)
                listBox1.Items.Add(i);

            for (int i = 1; i <=12 ; i++)
                listBox2.Items.Add(i);

            for (int i = 1; i <= day_limit[month - 1]; i++)
                listBox3.Items.Add(i);

            for (int i = 1; i < 24; i++)
                listBox4.Items.Add(i);

            for (int i = 0; i < 60; i+=5 )
                listBox5.Items.Add(i);

            string roundMinute = ( Math.Round(minute / 5.0 , MidpointRounding.AwayFromZero) * 5 ).ToString();

            int index = listBox5.FindString(roundMinute);


            if (index != -1)
            {
                listBox5.SetSelected(index, true);
                selectedMinute = roundMinute;
            }

            index = listBox4.FindString( hour.ToString() );
            if (index != -1)
            {
                listBox4.SetSelected(index, true);
                selectedHour = hour.ToString();
            }

            index = listBox3.FindString(day.ToString());
            if (index != -1)
            {
                listBox3.SetSelected(index, true);
                selectedDay = day.ToString();
            }

            index = listBox2.FindString(month.ToString());
            if (index != -1)
            {
                listBox2.SetSelected(index, true);
                selectedMonth = month.ToString();
            }

            index = listBox1.FindString(year.ToString());
            if (index != -1)
            {
                listBox1.SetSelected(index, true);
                selectedYear = year.ToString();
            }

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            int selectedMonth = int.Parse( listBox2.SelectedItem.ToString() );
            for (int i = 1; i <= day_limit[selectedMonth - 1]; i++)
                listBox3.Items.Add(i);

            int index = listBox3.FindString(selectedDay);
            if (index != -1)
            {
                listBox3.SetSelected(index, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {  
            DataClass.editedDate = DateTime.Parse(selectedYear + "/" + selectedMonth + "/" + selectedDay + " " + selectedHour + ":" + selectedMinute + ":00");

            this.DialogResult = DialogResult.OK;
        }
    }
}
