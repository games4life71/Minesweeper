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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static List<object> mines = new List<object>();
        private static int NumberOfMines; 

        private void SetGameDifficulty()
        {
            NumberOfMines = meniu.Difficulty;

        }

        private static void HideMap(TableLayoutPanel panel )
        {



        }
        
        private  static int CountFreeSpaces(TableLayoutPanel panel , int col , int row)
        {
                //count  all the free spaces on every direction 

            int[] y_pos = { -1, -1, -1, 0, 0, 1, 1, 1};
            int[] x_pos = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int freeSpaces = 0;

            for (int i = 0; i < 8; i++)
            {
                
                //check all the neighbours 

               if(x_pos[i]+col >= 0 && y_pos[i]+row>=0 && x_pos[i] + col <=19 && y_pos[i] + row<=19 )
                {
                    if (panel.GetControlFromPosition(x_pos[i] + col, y_pos[i] + row).Tag == "clear") freeSpaces++;
                        //Console.WriteLine("u pressed on column {0} , and row {1} ", col+x_pos[i], row+y_pos[i]);
                }


            }
            return freeSpaces;
        }

        private static void ShowFreeSpaces(TableLayoutPanel panel, int col, int row)
        {

             
            Queue<Button> q = new Queue<Button>();
            panel.GetControlFromPosition(col, row).Tag = "clearSHOW";
            q.Enqueue((Button)panel.GetControlFromPosition(col, row));
            while (q.Count != 0)
            { 
                //enqueue the first clear space and show it  
                Button btn = q.Dequeue();
                
                //check all the adjiacent cells 

                int[] y_pos = { -1, -1, -1, 0, 0, 1, 1, 1,};
                int[] x_pos = { -1, 0, 1, -1, 1, -1, 0, 1 };

                for (int i = 0; i < 8; i++)
                {
                    if (x_pos[i] + col >= 0 && y_pos[i] + row >= 0 && x_pos[i] + col <= 19 && y_pos[i] + row <= 19)
                    {
                        if (panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]).Tag == "clear" && CountFreeSpaces(panel, col + x_pos[i], row + y_pos[i]) == 8)
                        {
                            Console.WriteLine(col + x_pos[i] + "pozitie");
                            q.Enqueue((Button)panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]));
                            panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]).Tag = "clearSHOW";

                        }
                    }
                }
               
             
            }

        }           
        
      private void Form1_Load(object sender, EventArgs e)
        {
            

            //dynamically adding buttons 
            for (int i = 0; i < panel.RowCount; i++)
                for (int j = 0; j < panel.ColumnCount; j++)
                {

                    Button button = new Button();
                    button.Margin = new Padding(0);
                    button.Dock = DockStyle.Fill;
                    button.AutoSize = false;
                    button.MouseUp+= mouse_click;
                    panel.Controls.Add(button);
                    
                    // button.Click += EVENT    

                    //generating mines randomly  and setting button's tag to mine 

                }
           
            SetGameDifficulty();
            ClearMap(panel);
            GenerateMap(panel);
        }

        private static void ClearMap(TableLayoutPanel panel)
        {

            for(int i =  0; i<panel.RowCount;i++)
            {

                for(int j = 0; j<panel.ColumnCount;j++)
                {
                    Button btn = (Button)panel.GetControlFromPosition(j, i);
                    btn.BackgroundImage = null;
                    btn.Tag = "clear";
                }


            }



        }
        /// <summary>
        /// A function that randomly generates a Minesweeper map 
        /// </summary>
        /// <param name="panel"></param>
        private  void GenerateMap(TableLayoutPanel panel)
        {
            ClearMap(panel);
            int numMines = NumberOfMines;
            int i = 0;
             while(numMines>0 )
            {
            bool isPlaced = false; 
                Random rnd = new Random();
                do
                {
                    int rnd_x = rnd.Next(0, 20);
                    int rnd_y = rnd.Next(0, 20);
                    Button btn =(Button)panel.GetControlFromPosition(rnd_y, rnd_x); 
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
        private static void ShowMines(TableLayoutPanel panel )
        {

            foreach (Button btn  in mines)
            {

                btn.BackgroundImage = Properties.Resources.bomb;
                
            }
            mines.Clear();
        }

        private void mouse_click (object sender  ,MouseEventArgs e )
        {

           if (sender is Button )
            {
                if (e.Button == MouseButtons.Left)
                {
                    //LEFT CLICK 
                    // mine this spot 
                  
                    
                    Button btn = (Button)sender;
                    if (btn.Tag == "mine")
                    {
                        //game is lost 
                        //uncover all the mines 
                        ShowMines(panel);
                        MessageBox.Show("You have lost the game",  "Lost the game", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                       
                    }
                    else
                    {

                        // unconver all the empty spaces 
                        int col = panel.GetPositionFromControl(btn).Column;
                        int row = panel.GetPositionFromControl(btn).Row;
                        //Console.WriteLine("u pressed on column {0} , and row {1} " , col,row);
                        ShowFreeSpaces(panel, col, row); 
                        Console.WriteLine(CountFreeSpaces(panel ,col , row ));

                        
                    }


                }

                else if (e.Button == MouseButtons.Right)
                {     //RIGHT CLICK 
                    //flag the spot 
                   
                    Button btn = (Button)sender;
                    btn.BackgroundImage = Properties.Resources.flag;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                }

            }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateMap(panel); 
        }

        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void generateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateMap(panel);
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to quit ?" , "Main menu" , MessageBoxButtons.YesNo , MessageBoxIcon.Question) == DialogResult.Yes)
            {

                var form = new meniu();
                this.Hide();
                form.Show();

            }
        }

        private void restartGameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowMines(panel);
             
              if ( MessageBox.Show("Are you sure you want to restart ?", " Restart game ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes )
                {
                //restart the game 
                ClearMap(panel);
                GenerateMap(panel);
               }
        }
    } 
    }
