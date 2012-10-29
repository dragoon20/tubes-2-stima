using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace HarborMania
{
    class Node
    {
        int status; //0 kosong, 1 isi
        Texture2D gambar;
        GraphicsDevice gd;

        public Node(int status_)
        {
            status = status_;
        }

        public Node()
        {
            status = 0;
            /*
            gd = Graphic
            using(FileStream stream = File.OpenRead("sea-1.png")) {
                gambar = Texture2D.FromStream(gd, stream);
            } */
        }

        public void setStatus(int status_)
        {
            status = status_;
        }

        public int getStatus()
        {
            return status;
        }
    }
}