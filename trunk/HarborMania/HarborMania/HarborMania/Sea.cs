using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    /// <summary>
        /// Class Sea is used for saving node nodes, width per tile, and finish position.
        /// <list type="Properties">
            /// <item>
                /// <description>Tile is a matrix to contain all the nodes status.</description>
            /// </item>
            /// <item>
                /// <description>WidthPerTile is used to limit the width per tile.</description>
            /// </item>
            /// <item>
                /// <description>HeightPerTile is used to limit the heigth per tile.</description>
            /// </item>
            /// <item>
                /// <description>totalNodeX is used to describe total X axis Node.</description>
            /// </item>
            /// <item>
                /// <description>totalNodeY is used to describe total Y axis Node.</description>
            /// </item>
            ///  <item>
                /// <description>mapstatus is used to map tile status to texture2D.</description>
            /// </item>
            /// <item>
                /// <description>path is the file location of the map.</description>
            /// </item>
        /// </list>
    /// </summary>
    class Sea : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Node[][] Tile;
        int WidthPerTile;
        int HeightPerTile;
        int totalNodeX;
        int totalNodeY;
        int outPosY;
        Texture2D seaTile;
        Texture2D woodTile;
        Double time;

        Dictionary<int, Texture2D> mapstatus;
        String path;

        /// <summary>
            /// A method overriden to initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
            /// Getter and Setter for element outPos.
        /// </summary>
        public Vector2 outPos
        {
            get { return new Vector2(totalNodeX, outPosY); }
        }

        public Double TIME
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
            /// Getter Node at x = i, y = j.
        /// </summary>
        /// <param name="i">The absis.</param>
        /// <param name="j">The ordinat.</param>
        /// <returns>Node at x = i, y = j.</returns>
        public Node getNode(int i, int j)
        {
            return Tile[i][j];
        }

        /// <summary>
            /// Constructor for Class Sea.
        /// </summary>
        /// <param name="game">The Game class for DrawableGameComponent.</param>
        public Sea(Game game) : base(game)
        {
            WidthPerTile = 80;
            HeightPerTile = 80;
            totalNodeX = 6;
            totalNodeY = 6;
            Tile = new Node[totalNodeY][];
            for (int i = 0; i < totalNodeY; i++)
            {
                Tile[i] = new Node[totalNodeX];
                for (int j = 0; j < totalNodeX; j++)
                {
                    Tile[i][j] = new Node();
                }
            }
        }

        /// <summary>
            /// Constructor for Class Sea.
        /// </summary>
        /// <param name="game">The Game class for DrawableGameComponent.</param>
        /// <param name="map">The Sea to be copied.</param>
        public Sea(Game game, Sea map)
            : base(game)
        {
            path = map.path;
            WidthPerTile = map.WidthPerTile;
            HeightPerTile = map.HeightPerTile;
            totalNodeX = map.totalNodeX;
            totalNodeY = map.totalNodeY;
            Tile = new Node[totalNodeY][];
            for (int i = 0; i < totalNodeY; i++)
            {
                Tile[i] = new Node[totalNodeX];
                for (int j = 0; j < totalNodeX; j++) 
                {
                    Tile[i][j] = new Node(map.getNode(i,j));
                }
            }
        }

        /// <summary>
            /// Constructor for Class Sea.
        /// </summary>
        /// <param name="game">The Game class for DrawableGameComponent.</param>
        /// <param name="W">The width per tile for the map.</param>
        /// <param name="H">The heigth per tile for the map.</param>
        /// <param name="totalNodeX_">Total absis x for the map.</param>
        /// <param name="totalNodeY_">Total ordinat y for the map.</param>
        /// <param name="Path">The path to load the level.</param>
        public Sea(Game game, int W, int H, int totalNodeX_, int totalNodeY_, String Path) : base(game)
        {
            path = Path;
            WidthPerTile = W;
            HeightPerTile = H;
            totalNodeX = totalNodeX_;
            totalNodeY = totalNodeY_;
            Tile = new Node[totalNodeY][];
            for (int i = 0; i < totalNodeY; i++)
            {
                Tile[i] = new Node[totalNodeX];
                for (int j = 0; j < totalNodeX; j++)
                {
                    Tile[i][j] = new Node();
                }
            }
        }

        /// <summary>
            /// Getter for Status of Node at x = j and y = i.
        /// </summary>
        /// <param name="i">The absis of the node.</param>
        /// <param name="j">The ordinat of the node.</param>
        /// <returns></returns>
        public int GetStatus(int i, int j)
        {
            return Tile[j][i].StatusSebenarnya;
        }

        /// <summary>
            /// Setter for Status of Node at x = j and y = i to status.
        /// </summary>
        /// <param name="i">The absis of the node.</param>
        /// <param name="j">The ordinat of the node.</param>
        /// <param name="status">The status to be set.</param>
        public void SetStatus(int i, int j, int status)
        {
            Tile[j][i].StatusSebenarnya = status;
        }

        /// <summary>
            /// Load Content of the map with the boat.
        /// </summary>
        /// <param name="boats">List of boats to be returned.</param>
        public void LoadContent(out List<Boat> boats)
        {
            List<Boat> localboats = new List<Boat>();

            seaTile = Game.Content.Load<Texture2D>("Tile/Sea");
            woodTile = Game.Content.Load<Texture2D>("Tile/Wood");

            Stream streampath = TitleContainer.OpenStream(path);
            StreamReader loadpath = new StreamReader(streampath);

            mapstatus = new Dictionary<int, Texture2D>();

            for (int i = 0; i < totalNodeY; ++i)
            {
                String line = loadpath.ReadLine();
                String[] splitline = line.Split(' ');
                for (int j = 0; j < totalNodeX; ++j)
                {
                    Tile[i][j].Status = Convert.ToInt16(splitline[j]);
                    Tile[i][j].StatusSebenarnya = 0;
                }
            }

            String templine = loadpath.ReadLine();
            outPosY = Convert.ToInt32(templine);


            templine = loadpath.ReadLine();
            time = Convert.ToDouble(templine);
            Debug.WriteLine("time = " + time);


            templine = loadpath.ReadLine();
            int totalcar = Convert.ToInt32(templine);

            for (int i = 0; i < totalcar; ++i)
            {
                templine = loadpath.ReadLine();
                String[] splitline = templine.Split(' ');

                int x = Convert.ToInt32(splitline[0]), y = Convert.ToInt32(splitline[1]);
                int w = Convert.ToInt32(splitline[2]), h = Convert.ToInt32(splitline[3]);

                if (w == 1)
                {
                    for (int j = 0; j < h; ++j)
                        Tile[y + j][x].StatusSebenarnya = 1;
                }
                else if (h == 1)
                {
                    for (int j = 0; j < w; ++j)
                        Tile[y][x + j].StatusSebenarnya = 1;
                }
                localboats.Add(new Boat(Game, new Vector2(x, y), 
                                new Vector2(w, h), WidthPerTile, HeightPerTile,
                                Game.Content.Load<Texture2D>(splitline[5])));
                switch (splitline[4])
                {
                    case "Top":
                    {
                        localboats.ElementAt(i).Arah = Boat.Orientation.Top;
                        break;
                    }
                    case "Right":
                    {
                        localboats.ElementAt(i).Arah = Boat.Orientation.Right;
                        break;
                    }
                    case "Bottom":
                    {
                        localboats.ElementAt(i).Arah = Boat.Orientation.Bottom;
                        break;
                    }
                    case "Left":
                    {
                        localboats.ElementAt(i).Arah = Boat.Orientation.Left;
                        break;
                    }
                    default: break;
                }
                localboats.ElementAt(i).Initialize();
            }

            templine = loadpath.ReadLine();
            while (templine != null)
            {
                String[] splitline = templine.Split(' ');
                if (splitline.Count() > 1)
                    mapstatus.Add(Convert.ToInt32(splitline[0]), Game.Content.Load<Texture2D>(splitline[1]));
                templine = loadpath.ReadLine();
            }

            boats = localboats;
        }

        /// <summary>
            /// A method overriden to update the DrawableGameComponent.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Method for drawing the sea to the screen.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch to be used for Drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < totalNodeY; ++i)
            {
                for (int j = 0; j < totalNodeX; ++j)
                {
                    spriteBatch.Draw(mapstatus[Tile[i][j].Status], new Rectangle(j * WidthPerTile, i * HeightPerTile, WidthPerTile, HeightPerTile), Color.White);
                }
            }

            for (int i = 0; i < totalNodeY; ++i)
            {
                if (i==outPosY)
                    spriteBatch.Draw(seaTile, new Rectangle(totalNodeX * WidthPerTile, i * HeightPerTile, WidthPerTile, HeightPerTile), Color.White);
                else
                    spriteBatch.Draw(woodTile, new Rectangle(totalNodeX * WidthPerTile, i * HeightPerTile, WidthPerTile, HeightPerTile), Color.White);
            }
            spriteBatch.End();
        }
    }
}