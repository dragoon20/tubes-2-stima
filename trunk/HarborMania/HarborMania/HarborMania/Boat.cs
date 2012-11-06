using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class Boat : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //int height;
        //int width;
        //int posX; //posX adalah posisi X dari boat head
        //int posY; //posY adalah posisi Y dari boat head
        //int arah; //arah adalah arah dari boat head
        Vector2 position;
        Vector2 size;
        Texture2D texture; //masih blm tau ini utk apa

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

        public Boat(Game game) : base(game)
        {
            position = new Vector2();
            size = new Vector2();
        }

        public Boat(Game game, Vector2 Position, Vector2 Size, Texture2D Texture) : base(game)
        {
            texture = Texture;
            position = Position;
            size = Size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle((int)position.X * 80, (int)position.Y * 80, (int)size.X * 80, (int)size.Y * 80), Color.White);
            spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadContent()
        {
            if ((size.X == 1) && (size.Y == 1))
            {
                texture = Game.Content.Load<Texture2D>("");
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /*
        public void Move(int jarak)
        {
            if (arah == 0) //atas 
            {
                posY -= jarak;
            }
            else
                if (arah == 1) //kanan
                {
                    posX = +jarak;
                }
                else
                    if (arah == 2) //bawah
                    {
                        posY = +jarak;
                    }
                    else
                        if (arah == 3) //kiri
                        {
                            posX -= jarak;
                        }
        } */

    }
}