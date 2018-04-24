
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using GameGraphics;


namespace GameToWorkWith
{
    public enum Size
    {
        Large, Big, Normal, Small
    }

    public enum Buildings
    {
        House, Farm, Pump, None
    }
    public class Building : Tile
    {
        public int Capacity;
        public Size size;
        protected static RectangleF CountSize(Size size)
        {
            if (size == Size.Normal)
            {
                return new RectangleF(0, 0, 40, 40);
            }
            if (size == Size.Small)
            {
                return new RectangleF(0, 0, 20, 20);
            }
            if (size == Size.Large)
            {
                return new RectangleF(0, 0, 80, 80);
            }
            if (size == Size.Big)
            {
                return new RectangleF(0, 0, 60, 60);
            }
            else
            {
                throw (new ApplicationException("USize is not set"));
            }
        }
        public Building(PointF Location, Color color, int capacity, Size size, int Layer) : base(Location, color, CountSize(size).Width, CountSize(size).Height, Layer, Shape.Rectangle)
        {
            this.color = color;
            Capacity = capacity;
            this.size = size;
            if (this.size == Size.Normal)
            {
                block = new RectangleF(Location.X, Location.Y, 40, 40);
            }
            if (this.size == Size.Small)
            {
                block = new RectangleF(Location.X, Location.Y, 20, 20);
            }
            if (this.size == Size.Large)
            {
                block = new RectangleF(Location.X, Location.Y, 80, 80);
            }
            if (this.size == Size.Big)
            {
                block = new RectangleF(Location.X, Location.Y, 60, 60);
            }
        }
    }
    public class Lake : Tile
    {
        public Lake(RectangleF block, int Layer) : base(new PointF(block.X, block.Y), Color.Blue, block.Width, block.Height, Layer, Shape.Ellipse)
        {
            this.block = block;
        }
    }
    public class House : Building
    {
        public House(int capacity, PointF location, Color color, Size size, int Layer) : base(location, color, capacity, size, Layer)
        {
        }
    }
    public class Farm : Building
    {

        public float ProduceOfFood;
        public float RequiredMoney;

        public Farm(float produceOfFood, float requiredMoney, PointF location, Color color, int capacity, Size size, int Layer) : base(location, color, capacity, size, Layer)
        {
            ProduceOfFood = produceOfFood;
            RequiredMoney = requiredMoney;
        }
    }
    public class PumpingStation : Building
    {
        public float ProduceOfWater;
        public float RequiredMoney;
        public PumpingStation(int produceWater, int requiredMoney, PointF location, Color color, int capacity, Size size, int Layer) : base(location, color, capacity, size, Layer)
        {
            ProduceOfWater = produceWater;
            RequiredMoney = requiredMoney;
        }
    }
    public class Game:Engine
    {
        public int Layers = 0;
        public float Food;
        public float Water;
        public int People;
        public float Money;
        public Game(Graphics graphics):base(graphics)
        {
            Food = 1000;
            Water = 1000;
            People = 5;
        }
        public void GenWorld(Size size)
        {
            Random r = new Random();
            if (size == Size.Normal)
            {
                pen.Color = Color.Blue;
                for (int i = 0; i < 15; i++)
                {
                    int x = r.Next(666);
                    int y = r.Next(450);
                    graphics.FillEllipse(pen.Brush, x, y, 40, 40);
                    tiles.Add(new Lake(new RectangleF(x, y, 40, 40), 0));
                }
            }

        }
        public int CountBuildings()
        {
            int t = 0;
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].GetType() == typeof(Building))
                {
                    t++;
                }
            }
            return t;
        }
        public void Count()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].GetType() == typeof(Building))
                {
                    if (tiles[i].GetType() == typeof(Farm))
                    {
                        Farm f = tiles[i] as Farm;
                        Food += f.ProduceOfFood;
                    }
                    if (tiles[i].GetType() == typeof(PumpingStation))
                    {
                        PumpingStation p = tiles[i] as PumpingStation;
                        Water += p.ProduceOfWater;
                    }
                }
            }
            for (int i = 0; i < People; i++)
            {
                Food -= 40;
                Water -= 90;
            }
            if (Food < 0 && Water < 0)
            {

            }
        }
        public void Load(string filename)
        {
            tiles.Clear();
            graphics.Clear(Color.YellowGreen);
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "basic")
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        string loc = node.ChildNodes[i].Attributes.GetNamedItem("location").Value;
                        if (loc.IndexOf(".") > 0)
                        {
                            int x = Convert.ToInt32(loc.Substring(0, loc.IndexOf(".")));
                            int y = Convert.ToInt32(loc.Substring(loc.IndexOf(".") + 1));
                            PointF p = new PointF(x, y);
                            if (node.ChildNodes[i].Attributes.GetNamedItem("type").Value == "House")
                            {
                                tiles.Add(new House(15, p, Color.Brown, Size.Normal, Convert.ToInt32(node.ChildNodes[i].Attributes.GetNamedItem("layer").Value)));
                            }
                            if (node.ChildNodes[i].Attributes.GetNamedItem("type").Value == "PumpS")
                            {
                                tiles.Add(new PumpingStation(100, 500, p, Color.Aqua, 5, Size.Normal, Convert.ToInt32(node.ChildNodes[i].Attributes.GetNamedItem("layer").Value)));
                            }
                            if (node.ChildNodes[i].Attributes.GetNamedItem("type").Value == "Farm")
                            {
                                tiles.Add(new Farm(100, 500, p, Color.Green, 5, Size.Normal, Convert.ToInt32(node.ChildNodes[i].Attributes.GetNamedItem("layer").Value)));
                            }
                            if (node.ChildNodes[i].Attributes.GetNamedItem("type").Value == "Lake")
                            {
                                tiles.Add(new Lake(new RectangleF(p.X, p.Y, 40, 40), Convert.ToInt32(node.ChildNodes[i].Attributes.GetNamedItem("layer").Value)));
                            }
                        }
                        else
                        {
                            throw (new ApplicationException(loc.IndexOf(".").ToString() + "Save file corrupted"));
                        }
                    }//for
                }
                if (node.Name == "additional")
                {
                    foreach (XmlNode data in node)
                    {
                        if (data.Name == "statistics")
                        {
                            for (int i = 0; i < data.ChildNodes.Count; i++)
                            {
                                if (data.ChildNodes[i].Name == "Food")
                                {
                                    Food = Convert.ToInt64(data.ChildNodes[i].Value);
                                }
                                if (data.ChildNodes[i].Name == "Water")
                                {
                                    Water = Convert.ToInt64(data.ChildNodes[i].Value);
                                }
                                if (data.ChildNodes[i].Name == "People")
                                {
                                    People = Convert.ToInt32(data.ChildNodes[i].Value);
                                }
                            }
                        }
                    }
                }
            }
        }//load
        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("save");
            XmlElement basic = doc.CreateElement("basic");
            XmlElement add = doc.CreateElement("additional");
            XmlElement stats = doc.CreateElement("statistics");
            XmlElement f = doc.CreateElement("Food");
            XmlElement w = doc.CreateElement("Water");
            XmlElement p = doc.CreateElement("People");
            XmlElement date = doc.CreateElement("Date");
            date.InnerText = DateTime.Now.ToString();
            XmlElement tile;
            XmlAttribute loc;
            XmlAttribute type;
            XmlAttribute layer;
            for (int i = 0; i < tiles.Count; i++)
            {
                tile = doc.CreateElement("tile");
                loc = doc.CreateAttribute("location");
                type = doc.CreateAttribute("type");
                layer = doc.CreateAttribute("layer");
                if (tiles[i].GetType() == typeof(Lake))
                {
                    type.InnerText = "Lake";
                    loc.InnerText = tiles[i].block.X.ToString() + "." + tiles[i].block.Y.ToString();
                    layer.InnerText = tiles[i].Layer.ToString();
                    tile.Attributes.Append(layer);
                    tile.Attributes.Append(type);
                    tile.Attributes.Append(loc);
                    basic.AppendChild(tile);
                }
                if (tiles[i] is Building)
                {
                    if (tiles[i].GetType() == typeof(House))
                    {
                        type.InnerText = "House";
                        loc.InnerText = tiles[i].block.X.ToString() + "." + tiles[i].block.Y.ToString();
                        layer.InnerText = tiles[i].Layer.ToString();
                        tile.Attributes.Append(layer);
                        tile.Attributes.Append(type);
                        tile.Attributes.Append(loc);
                        basic.AppendChild(tile);
                    }
                    if (tiles[i].GetType() == typeof(PumpingStation))
                    {
                        type.InnerText = "PumpS";
                        loc.InnerText = tiles[i].block.X.ToString() + "." + tiles[i].block.Y.ToString();
                        layer.InnerText = tiles[i].Layer.ToString();
                        tile.Attributes.Append(layer);
                        tile.Attributes.Append(type);
                        tile.Attributes.Append(loc);
                        basic.AppendChild(tile);
                    }
                    if (tiles[i].GetType() == typeof(Farm))
                    {
                        type.InnerText = "Farm";
                        loc.InnerText = tiles[i].block.X.ToString() + "." + tiles[i].block.Y.ToString();
                        layer.InnerText = tiles[i].Layer.ToString();
                        tile.Attributes.Append(layer);
                        tile.Attributes.Append(type);
                        tile.Attributes.Append(loc);
                        basic.AppendChild(tile);
                    }
                }
            }
            f.InnerText = Food.ToString();
            w.InnerText = Water.ToString();
            p.InnerText = People.ToString();
            stats.AppendChild(f);
            stats.AppendChild(w);
            stats.AppendChild(p);
            add.AppendChild(stats);
            root.AppendChild(date);
            root.AppendChild(basic);
            root.AppendChild(add);
            doc.AppendChild(root);
            doc.Save("save.save");

        }
    }

    static class Logic
    {
      
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
};//namespace
