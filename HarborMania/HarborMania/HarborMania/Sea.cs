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

        Dictionary<int, Texture2D> mapstatus;
        String path;

        public override void Initialize()
        {
            base.Initialize();
        }

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
                    Tile[i][j] = new Node(game);
                }
            }
        }

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
                    Tile[i][j] = new Node(game);
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

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
            int totalcar = Convert.ToInt32(templine);

            for (int i = 0; i < totalcar; ++i)
            {
                templine = loadpath.ReadLine();
                String[] splitline = templine.Split(' ');

                int x = Convert.ToInt32(splitline[0]), y = Convert.ToInt32(splitline[1]);
                int w = Convert.ToInt32(splitline[2]), h = Convert.ToInt32(splitline[3]);

                Tile[y][x].StatusSebenarnya = 1;
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
                                new Vector2(w, h), 
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < totalNodeY; ++i)
            {
                for (int j = 0; j < totalNodeX; ++j)
                {
                    spriteBatch.Draw(mapstatus[Tile[i][j].Status],new Rectangle(j*80, i*80, 80, 80),Color.White);
                }
            }

            for (int i = 0; i < totalNodeY; ++i)
            {
                if (i==outPosY)
                    spriteBatch.Draw(seaTile, new Rectangle(6 * 80, i * 80, 80, 80), Color.White);
                else
                    spriteBatch.Draw(woodTile, new Rectangle(6 * 80, i * 80, 80, 80), Color.White);
            }
            spriteBatch.End();
        }
    }
}