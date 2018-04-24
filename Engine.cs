using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace GameGraphics
{
    public enum Shape
    {
        Rectangle, Ellipse
    }
    public class Tile
    {
        public Color color;
        public RectangleF block;
        public int Layer;
        public Shape shape;
        public Tile(PointF Location, Color color, float Width, float Height, int layer, Shape shape)
        {
            this.color = color;
            block = new RectangleF(Location.X, Location.Y, Width, Height);
            Layer = layer;
            this.shape = shape;
        }
    }
    public class Engine
    {
        public Graphics graphics;
        public Pen pen = new Pen(Color.Black);
        public List<Tile> tiles = new List<Tile>();
        public Engine(Graphics graphics)
        {
            this.graphics = graphics;
        }
        public void Render()
        {
            try
            {
                if (tiles.Count > 0)
                {
                    int BiggestLayer = 0;
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (tiles[i].Layer > BiggestLayer)
                        {
                            BiggestLayer = tiles[i].Layer;
                        }
                    }
                    graphics.Clear(Color.YellowGreen);
                    for (int layer = 0; layer <= BiggestLayer; layer++)
                    {
                        for (int i = 0; i < tiles.Count; i++)
                        {

                            if (tiles[i].Layer == layer)
                            {
                                if (tiles[i].shape == Shape.Ellipse)
                                {
                                    pen.Color = Color.Blue;
                                    graphics.FillEllipse(pen.Brush, tiles[i].block);
                                }
                                if (tiles[i].shape == Shape.Rectangle)
                                {
                                    pen.Color = tiles[i].color;
                                    graphics.FillRectangle(pen.Brush, tiles[i].block);
                                }
                            }
                        }//for
                    }//for
                }//if
            }//try
            catch (ArgumentException)
            {

            }
        }
        public void Dispose()
        {
            graphics.Dispose();
            pen.Dispose();
        }
    }
}