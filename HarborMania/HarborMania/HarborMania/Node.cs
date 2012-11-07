using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class Node
    {
        int status; //gambar
        int statussebenarnya; //0 kosong, 1 isi, 2 dll
        Vector2 size;
        Vector2 position;
        //GraphicsDevice gd;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public int StatusSebenarnya
        {
            get { return statussebenarnya; }
            set { statussebenarnya = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Node()
        {
            position = new Vector2();
            size = new Vector2();
        }

        public Node(Node n)
        {
            status = n.status;
            statussebenarnya = n.statussebenarnya;
            size = n.size;
            position = n.position;
        }
    }
}