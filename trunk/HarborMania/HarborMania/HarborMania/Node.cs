using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarborMania
{
    class Node
    {
        int X;
        int Y;
        int Width;
        int Height;
        //tambahin : gambar tile per node

        public Node(int X_, int Y_, int Width_, int Height_)
        {
            X = X_;
            Y = Y_;
            Width = Width_;
            Height = Height_;
        }

        public Node()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public void updateNode(int newX, int newY, int newWidth, int newHeight)
        {
            X = newX;
            Y = newY;
            Width = newWidth;
            Height = newHeight;
        }

    }
}
