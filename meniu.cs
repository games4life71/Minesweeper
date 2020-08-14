using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class meniu : Form
    {
        public meniu()
        {
            InitializeComponent();
        }
        public  static  int Difficulty; 

        private void lblDificultate_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Difficulty = 10;
            Console.WriteLine("dif 10");
            var form = new  Form1();
            form.Show();
            this.Hide(); 
              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Difficulty = 15;
            var form =  new Form1();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Difficulty = 20;
            var form =  new Form1();
            form.Show();
            this.Hide();
        }
    }
}
