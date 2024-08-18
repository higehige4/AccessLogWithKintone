using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            LoadConfig();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }


        private void LoadConfig()
        {

            textBox1.Text = ConfigurationManager.AppSettings["ktDomain"];
            textBox2.Text = ConfigurationManager.AppSettings["ktLoginId"];
            textBox3.Text = ConfigurationManager.AppSettings["ktLoginPassword"];
            textBox4.Text = ConfigurationManager.AppSettings["AppId_Member"];
            textBox5.Text = ConfigurationManager.AppSettings["AppId_LicenseList"];
            textBox6.Text = ConfigurationManager.AppSettings["AppId_InoutList"];
            textBox7.Text = ConfigurationManager.AppSettings["InText"];
            textBox8.Text = ConfigurationManager.AppSettings["OutText"];
        }

        private void SaveConfig()
        {
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            config.AppSettings.Settings["ktDomain"].Value = textBox1.Text;
            config.AppSettings.Settings["ktLoginId"].Value = textBox2.Text;
            config.AppSettings.Settings["ktLoginPassword"].Value = textBox3.Text;
            config.AppSettings.Settings["AppId_Member"].Value = textBox4.Text;
            config.AppSettings.Settings["AppId_LicenseList"].Value = textBox5.Text;
            config.AppSettings.Settings["AppId_InoutList"].Value = textBox6.Text;
            config.AppSettings.Settings["InText"].Value = textBox7.Text;
            config.AppSettings.Settings["OutText"].Value = textBox8.Text;

            config.Save();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            SaveConfig();

            Application.Restart();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            form4.ShowDialog();
        }
    }
}
