using System;
using System.Collections.Generic;
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
        Node [][] Tile;
        int WidthPerTile;
        int HeightPerTile;
        int totalNodeX;
        int totalNodeY;
        Dictionary<int, Texture2D> mapstatus;

        public override void Initialize()
        {
            base.Initialize();
        }

        public Sea (Game1 game) : base(game)
        {
            WidthPerTile = 100;
            HeightPerTile = 100;
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

        public Sea (Game1 game, int W, int H, int totalNodeX_, int totalNodeY_) : base(game)
        {
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

        public void LoadContent(int level, String path)
        {
            Stream streampath = TitleContainer.OpenStream(path);
            StreamReader loadpath = new StreamReader(streampath);

            for (int i = 0; i < totalNodeY; ++i)
            {
                String line = loadpath.ReadLine();
                String [] splitline = line.Split(' ');
                for (int j = 0; j < totalNodeX; ++j)
                {
                    Tile[i][j].Status = Convert.ToInt16(splitline[j]);
                }
            }

            String templine = loadpath.ReadLine();
            while (templine != null)
            {
                String[] splitline = templine.Split(' ');
                if (splitline.Count() > 0)
                    mapstatus.Add(Convert.ToInt32(splitline[0]), Game.Content.Load<Texture2D>(splitline[1]));
                templine = loadpath.ReadLine();
            }
        }
    }
}
