using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static List<object> mines = new List<object>();
        private static int NumberOfMines;
        private static int FirstClick = 0;
        private void SetGameDifficulty()
        {
            NumberOfMines = meniu.Difficulty;

        }

   

        private static int CountFreeSpaces(TableLayoutPanel panel, int col, int row)
        {
            //count  all the free spaces on every direction 

            int[] y_pos = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] x_pos = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int freeSpaces = 0;
            for (int i = 0; i < 8; i++)
            {

                //check all the neighbours 

                if (x_pos[i] + col >= 0 && y_pos[i] + row >= 0 && x_pos[i] + col <= 19 && y_pos[i] + row <= 19)
                {
                    if (panel.GetControlFromPosition(x_pos[i] + col, y_pos[i] + row).Tag != "mine") freeSpaces++;
                    //Console.WriteLine("u pressed on column {0} , and row {1} ", col+x_pos[i], row+y_pos[i]);

                }
            }


            return freeSpaces;


        }


        private static void UncoverCells(TableLayoutPanel panel, int col, int row)
        {

            //////Button btn = (Button)panel.GetControlFromPosition(col, row);
            //base cases
            if (col > 19 || row > 19 || col < 0 || row < 0) return;


            if (panel.GetControlFromPosition(col, row).Tag == "clearShow")
            {
                return;
            }
            if (panel.GetControlFromPosition(col, row).Tag == "mine")

            {
                return;
            }

            if (CountFreeSpaces(panel, col, row) < 8)
            {
                panel.GetControlFromPosition(col, row).Text = (8 - CountFreeSpaces(panel, col, row)).ToString();
                if (col == 19 || row == 19 || col == 0 || row == 0)
                {    
                    panel.GetControlFromPosition(col, row).Text = CheckBombs(panel, col, row).ToString();
                    if (CheckBombs(panel, col, row) == 0) { panel.GetControlFromPosition(col, row).Enabled = false; panel.GetControlFromPosition(col, row).Text = ""; }

                }

                return;
            }
            int noBombs = 8 - CountFreeSpaces(panel, col, row);
            if (noBombs > 0) panel.GetControlFromPosition(col, row).Text = noBombs.ToString();

           // panel.GetControlFromPosition(col, row).Text = "..";
            panel.GetControlFromPosition(col, row).Enabled = false;
            panel.GetControlFromPosition(col, row).Tag = "clearShow";

            UncoverCells(panel, col, row - 1);
            UncoverCells(panel, col - 1, row);
            UncoverCells(panel, col + 1, row);
            UncoverCells(panel, col, row + 1);

            UncoverCells(panel, col - 1, row - 1);
            UncoverCells(panel, col + 1, row - 1);
            UncoverCells(panel, col - 1, row + 1);
            UncoverCells(panel, col + 1, row + 1);

        }
     
        private static void ClearMap(TableLayoutPanel panel)
        {

            for (int i = 0; i < panel.RowCount; i++)
            {

                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    Button btn = (Button)panel.GetControlFromPosition(j, i);
                    btn.BackgroundImage = null;
                    btn.Enabled = true;
                    btn.Text = "";
                    btn.Tag = "clear";
                }


            }



        }
        /// <summary>
        /// A function that randomly generates a Minesweeper map 
        /// </summary>
        /// <param name="panel"></param>
        private void GenerateMap(TableLayoutPanel panel)
        {
            FirstClick = 0; 
            ClearMap(panel);
            int numMines = 50;
            int i = 0;
            while (numMines > 0)
            {
                bool isPlaced = false;
                Random rnd = new Random();
                do
                {
                    int rnd_x = rnd.Next(0, 20);
                    int rnd_y = rnd.Next(0, 20);
                    Button btn = (Button)panel.GetControlFromPosition(rnd_y, rnd_x);
                    if (btn.Tag == "clear")
                    {
                        btn.BackgroundImage = Properties.Resources.bomb;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        btn.Tag = "mine";
                        isPlaced = true;
                        mines.Add(btn);
                    }
                }
                while (isPlaced == false);
                numMines--;
            }


        }
        private static void ShowMines(TableLayoutPanel panel)
        {

            foreach (Button btn in mines)
            {

                btn.BackgroundImage = Properties.Resources.bomb;

            }
            mines.Clear();
        }

        private void mouse_click(object sender, MouseEventArgs e)
        {

            if (sender is Button)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //LEFT CLICK 
                    // mine this spot 


                            
                    Button btn = (Button)sender;
                    if (btn.Tag == "mine")
                    {
                        if (FirstClick == 0)
                        {
                            //move the mine to the left or right corner 
                            btn.BackgroundImage = null;
                            btn.Tag = "clear";
                            panel.GetControlFromPosition(19, 0).Tag = "mine";
                            panel.GetControlFromPosition(19, 0).BackgroundImage = Properties.Resources.flag;
                            FirstClick++;
                        }
                        else
                        {
                            //game is lost 
                            //uncover all the mines 
                            ShowMines(panel);
                            MessageBox.Show("You have lost the game", "Lost the game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("Restart ? ", "Restart", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            ClearMap(panel);
                            GenerateMap(panel);
                        }

                    }
                    else
                    {

                        // unconver all the empty spaces 
                        int col = panel.GetPositionFromControl(btn).Column;
                        int row = panel.GetPositionFromControl(btn).Row;
                        //Console.WriteLine("u pressed on column {0} , and row {1} " , col,row);
                        UncoverCells(panel, col, row);

                        int clearShow = 0;
                        Console.WriteLine(CountFreeSpaces(panel, col, row));


                    }


                }

                else if (e.Button == MouseButtons.Right)
                {     //RIGHT CLICK 
                      //flag the spot 

                    Button btn = (Button)sender;
                    if (btn.Name == "flag")
                    {
                        btn.BackgroundImage = null;
                        btn.Name = "";
                    }
                    else
                    {
                        btn.BackgroundImage = Properties.Resources.flag;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        btn.Name = "flag";
                    }

                }

            }




        }
        private static int CheckBombs(TableLayoutPanel panel, int col, int row)
        {


            int[] y_pos = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] x_pos = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int bombs = 0;
            for (int i = 0; i < 8; i++)
            {

                //check all the neighbours 

                if (x_pos[i] + col >= 0 && y_pos[i] + row >= 0 && x_pos[i] + col <= 19 && y_pos[i] + row <= 19)
                {
                    if (panel.GetControlFromPosition(x_pos[i] + col, y_pos[i] + row).Tag == "mine") bombs++;
                    //Console.WriteLine("u pressed on column {0} , and row {1} ", col+x_pos[i], row+y_pos[i]);

                }
            }


            return bombs;



        }
        private void button1_Click(object sender, EventArgs e)
        {
            GenerateMap(panel);
        }

      

        private void generateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateMap(panel);
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit ?", "Main menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                var form = new meniu();
                this.Hide();
                form.Show();

            }
        }

      

        private void Form1_Load_1(object sender, EventArgs e)
        {

            //dynamically adding buttons 
            for (int i = 0; i < panel.RowCount; i++)
                for (int j = 0; j < panel.ColumnCount; j++)
                {

                    Button button = new Button();
                    button.Margin = new Padding(0);
                    button.Dock = DockStyle.Fill;
                    button.AutoSize = false;
                    button.MouseUp += mouse_click;

                    panel.Controls.Add(button);

                    // button.Click += EVENT    

                    //generating mines randomly  and setting button's tag to mine 
                    label1.Text = 100.ToString();
                }

            SetGameDifficulty();
            ClearMap(panel);
            GenerateMap(panel);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = (100000 - timer1.Interval).ToString();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit ?", "Main menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                var form = new meniu();
                this.Hide();
                form.Show();

            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            


            if (MessageBox.Show("Are you sure you want to restart ?", " Restart game ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //restart the game 
                ClearMap(panel);
                GenerateMap(panel);
            }
        }
    }
}
