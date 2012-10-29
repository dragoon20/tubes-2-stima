using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarborMania
{
    class Sea
    {
        Node[10][10] Tile;
        int Width;
        int Height;

        public Sea (int W, int H) {
            Width = W;
            Height = H;
            for (int i=1; i<=Height; i++) {
                for (int j=1; j<=Width; j++) {
                    Tile[i][j] = new Node(i, j, 100, 100); //100 dan 100 adalah ukuran tinggi dan lebar masing2 Tile
                }
            }
        }


    }
}
