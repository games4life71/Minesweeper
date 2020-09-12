using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Drawing;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetGameDifficulty();
        }
        private static List<object> mines = new List<object>();
        private static List<object> flags = new List<object>();
        private static int EasterEgg;
        private static int NumberOfMines;
        public static int MapOffset;
        private static int FirstClick = 0;
        private static int MapSize;
        private static int panelCoord;
        private static int NumberOfFlags;
        private void SetGameDifficulty()
        {
            NumberOfMines = meniu.Difficulty;
            MapSize = 20 - (MapOffset * 2);
            panelCoord = MapOffset + MapSize;
            Console.WriteLine("grid size is " + panelCoord);

        }
        private static Color SetDigitColor(int digit)
        {

            switch (digit)
            {

                case 1: return Color.Black;
                case 2: return Color.DarkBlue;
                case 3: return Color.Red;
                case 4: return Color.Red;

                default: return Color.Red;

            }



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
            int gridSize = MapOffset + MapSize;
            if (col > panelCoord - 1 || row > panelCoord - 1 || col < MapOffset || row < MapOffset) return;


            if (panel.GetControlFromPosition(col, row).Tag == "clearShow")
            {
                return;
            }
            if (panel.GetControlFromPosition(col, row).Tag == "mine")

            {
                return;
            }

            if (CheckBombs(panel, col, row) > 0)
            {
                panel.GetControlFromPosition(col, row).Text = CheckBombs(panel, col, row).ToString();
                panel.GetControlFromPosition(col, row).ForeColor = SetDigitColor(CheckBombs(panel, col, row));
                panel.GetControlFromPosition(col, row).Font = new Font("Bold", 12);
                if (col == panelCoord || row == panelCoord || col == MapOffset || row == MapOffset)
                {
                    // panel.GetControlFromPosition(col, row).Text = CheckBombs(panel, col, row).ToString();
                    if (CheckBombs(panel, col, row) == 0)
                    {
                        panel.GetControlFromPosition(col, row).Enabled = false;
                        panel.GetControlFromPosition(col, row).Text = null;

                    }

                }

                return;
            }

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

            for (int i = MapOffset; i < panelCoord; i++)
            {

                for (int j = MapOffset; j < panelCoord; j++)
                {



                    Button btn = (Button)panel.GetControlFromPosition(j, i);


                    btn.BackgroundImage = null;
                    btn.Enabled = true;
                    btn.Text = "";
                    btn.Tag = "clear";
                    btn.Name = "";
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
            int numMines = NumberOfMines;
            int i = 0;
            while (numMines > 0)
            {
                bool isPlaced = false;
                Random rnd = new Random();
                do
                {
                    int rnd_x = rnd.Next(MapOffset, panelCoord - 1);
                    int rnd_y = rnd.Next(MapOffset, panelCoord - 1);
                    Button btn = (Button)panel.GetControlFromPosition(rnd_y, rnd_x);
                    if (btn.Tag == "clear")
                    {
                        //btn.BackgroundImage = Properties.Resources.bomb;
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
                    if (FirstClick == 0 && btn.Tag == "mine")
                    {
                        //move the mine to the left or right corner 
                        btn.BackgroundImage = null;
                        btn.Tag = "clear";
                        panel.GetControlFromPosition(panelCoord - 1, MapOffset).Tag = "mine";
                        panel.GetControlFromPosition(panelCoord - 1, MapOffset).BackgroundImage = Properties.Resources.bomb;
                        FirstClick++;
                    }

                    if (btn.Tag == "mine")
                    {

                        //game is lost 
                        //uncover all the mines 
                        ShowMines(panel);
                        MessageBox.Show("You have lost the game", "Lost the game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show("Restart ? ", "Restart", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ClearMap(panel);
                        GenerateMap(panel);


                    }
                    else
                    {

                        FirstClick++;
                        // unconver all the empty spaces 
                        int col = panel.GetPositionFromControl(btn).Column;
                        int row = panel.GetPositionFromControl(btn).Row;
                        //Console.WriteLine("u pressed on column {0} , and row {1} " , col,row);
                        UncoverCells(panel, col, row);

                        int clearShow = 0;
                        //Console.WriteLine(CountFreeSpaces(panel, col, row));


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
                        NumberOfFlags--;
                        label7.Text = (NumberOfMines - NumberOfFlags).ToString();
                        flags.Remove(btn);
                    }
                    else
                    {
                        btn.BackgroundImage = Properties.Resources.flag;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        btn.Name = "flag";
                        NumberOfFlags++;
                        label7.Text = (NumberOfMines - NumberOfFlags).ToString();
                        flags.Add(btn);
                    }
                    //check if all the flags coords are correct and if yes win is declared 
                    if (NumberOfFlags == NumberOfMines)
                    {

                        List<object> compare = mines.Except(flags).ToList();
                        if (compare.Count == 0)
                        {
                            MessageBox.Show("You have win the game ! Congratz !!!", "Winner chicken dinner", MessageBoxButtons.OK);
                        }


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

                if (x_pos[i] + col >= MapOffset && y_pos[i] + row >= MapOffset && x_pos[i] + col <= panelCoord - 1 && y_pos[i] + row <= panelCoord - 1)
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
            label5.Text = NumberOfMines.ToString();
            label7.Text = NumberOfMines.ToString();
            Console.WriteLine("number of diff is {0}", MapSize);

            //dynamically adding buttons 
            for (int i = MapOffset; i < panelCoord; i++)
                for (int j = MapOffset; j < panelCoord; j++)
                {
                    Button button = new Button();

                    button.Margin = new Padding(0);
                    button.Dock = DockStyle.Fill;
                    button.AutoSize = false;
                    button.MouseUp += mouse_click;
                    panel.Controls.Add(button, j, i);
                    button.Text = panel.GetPositionFromControl(button).Row.ToString();
                    //panel.RowCount = MapSize;
                    //panel.ColumnCount = MapSize;

                    // button.Click += EVENT    

                    //generating mines randomly  and setting button's tag to mine 
                    label1.Text = 100.ToString();
                }

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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            EasterEgg++;
            if (EasterEgg == 3)
            {
                ShowMines(panel);
                EasterEgg = 0;
            }
        }
    }
}
