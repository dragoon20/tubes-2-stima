using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace HarborMania
{
    class Node : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int status; //0 kosong, 1 isi, 2 dll
        Vector2 size;
        Vector2 position;
        Texture2D gambar;
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

        public Node(Game game) : base(game)
        {
            position = new Vector2();
            size = new Vector2();

            LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadContent()
        {
            gambar = Game.Content.Load<Texture2D>("");
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}