using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class Node : Microsoft.Xna.Framework.GameComponent
    {
        int status; //0 kosong, 1 isi, 2 dll
        Vector2 size;
        Vector2 position;
        //GraphicsDevice gd;

        public int Status
        {
            get { return status; }
            set { status = value; }
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

        public Node(Game game)
            : base(game)
        {
            position = new Vector2();
            size = new Vector2();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

    }
}