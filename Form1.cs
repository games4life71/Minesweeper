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

        private static int NumberOfMines; 

        private void SetGameDifficulty()
        {
            NumberOfMines = meniu.Difficulty;

        }

        private static void HideMap(TableLayoutPanel panel )
        {



        }
        
        private  static int CheckFreeSpaces(TableLayoutPanel panel , int col , int row)
        {

            int[] y_pos = { -1, -1, -1, 0, 0, 1, 1, 1};
            int[] x_pos = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int freeSpaces = 0;
            
            for (int i = 0; i<8;i++)
            {
                if (panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]).Tag == "clear") freeSpaces++;
                //count  all the free spaces 
                
            }
            return freeSpaces;
        }

        private static void ShowFreeSpaces(TableLayoutPanel panel, int col, int row)
        {

            //add to queue all the neighbours recursively and show them 
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
                    if(panel.GetControlFromPosition(col+x_pos[i], row+y_pos[i]).Tag == "clear")
                    {
                        q.Enqueue((Button)panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]));
                        panel.GetControlFromPosition(col + x_pos[i], row + y_pos[i]).Tag = "clearSHOW";

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
            Console.WriteLine(NumberOfMines);
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
                        Console.WriteLine("ASAS");
                        btn.BackgroundImage = Properties.Resources.bomb;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                       
                        btn.Tag = "mine";
                        isPlaced = true;
                    }
                }
                while (isPlaced == false);
                numMines--;
            }


        }

        private void mouse_click (object sender  ,MouseEventArgs e )
        {

           if (sender is Button )
            {
                if (e.Button == MouseButtons.Left)
                {

                    // mine this spot 
                    Console.WriteLine("left");
                    
                    Button btn = (Button)sender;
                    if (btn.Tag == "mine")
                    {
                        //game is lost 
                        //uncover all the mines 
                        MessageBox.Show("You have lost the game",  "Lost the game", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        //restart the game and generate the map again 


                    }


                }

                else if (e.Button == MouseButtons.Right)
                {
                    //flag the spot 
                    Console.WriteLine("right")  ;
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
    } 
    }
