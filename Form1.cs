using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace GameToWorkWith
{
    public partial class Form1 : Form
    {
        public House house;
        public PumpingStation pump;
        public Farm farm;
        Game game;
        Buildings building = Buildings.None;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game(CreateGraphics());
            Thread tr = new Thread(game.Run);
            tr.IsBackground = true;
            tr.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            building = Buildings.House;
            game.pen.Color = Color.Brown;
            game.People += 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            building = Buildings.Farm;
            game.pen.Color = Color.Green;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            building = Buildings.Pump;
            game.pen.Color = Color.Aqua;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            label1.Text = (e.X + "  " + e.Y).ToString();
            if (e.Button == MouseButtons.Right)
            {
                building = Buildings.None;
            }
            else
            {
                bool temp = false;
                if (building != Buildings.None)
                {
                    //for (int i = 0; i < game.tiles.Count; i++)
                    //{
                    //    if (game.tiles[i] is Building)
                    //    {
                    //        for (int u = 0; u < 20; u++)
                    //        {
                    //            if (e.X == game.tiles[i].block.X + u)
                    //            {
                    //                temp = true;
                    //            }
                    //        }
                    //        for (int u = 0; u < 20; u++)
                    //        {
                    //            if (e.Y == game.tiles[i].block.Y + u)
                    //            {
                    //                temp = true;
                    //            }
                    //        }
                    //    }
                    //}
                    for(int i=0;i<game.tiles.Count;i++)
                    {
                        if (game.tiles[i].block.Contains(new PointF(e.X, e.Y)))
                        {
                            temp = true;
                        }
                    }
                    if (temp != true)
                    {
                        PointF point = new PointF(e.X, e.Y);
                        game.graphics.FillRectangle(game.pen.Brush, e.X, e.Y, 40, 40);
                        if (building == Buildings.Farm)
                        {
                            game.tiles.Add(new Farm(100, 500, point, Color.Green, 5, GameToWorkWith.Size.Normal,1));
                        }
                        if (building == Buildings.House)
                        {
                            game.tiles.Add(new House(10, point, Color.Brown, GameToWorkWith.Size.Normal,1));
                        }
                        if (building == Buildings.Pump)
                        {
                            game.tiles.Add(new PumpingStation(100, 500, point, Color.Aqua, 5, GameToWorkWith.Size.Normal,1));
                        }
                    }
                }
                label2.Text = game.CountBuildings().ToString();
                label3.Text = game.Water.ToString();
                label4.Text = game.Food.ToString();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.Dispose();
            Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           game.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            game.Load(openFileDialog1.FileName);
        }
    }
}
