using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarborMania
{
    class Sea : Microsoft.Xna.Framework.GameComponent
    {
        Node [][] Tile;
        int WidthPerTile;
        int HeightPerTile;
        int totalNodeX;
        int totalNodeY;

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
    }
}
