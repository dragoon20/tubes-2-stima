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
        public enum Orientation
        {
            Top, Right, Bottom, Left
        };

        //int height;
        //int width;
        //int posX; //posX adalah posisi X dari boat head
        //int posY; //posY adalah posisi Y dari boat head
        //int arah; //arah adalah arah dari boat head
        Vector2 position;
        Vector2 size;
        Orientation arah;
        Texture2D texture; //masih blm tau ini utk apa

        public Orientation Arah
        {
            get { return arah; }
            set { arah = value; }
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

        public Boat(Game game) : base(game)
        {
            position = new Vector2();
            size = new Vector2();
        }

        public Boat(Game game, Vector2 Position, Vector2 Size, Texture2D Texture) : base(game)
        {
            texture = Texture;
            position = new Vector2 (Position.X * 80, Position.Y * 80);
            size = new Vector2(Size.X * 80, Size.Y * 80);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (arah == Orientation.Left)
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X+160, (int)position.Y+80, (int)size.X, (int)size.Y), null, Color.White, (float)3.14159265358979323846264338327950288, new Vector2(), new SpriteEffects(), 0);
            }
            else if (arah == Orientation.Bottom)
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X+80, (int)position.Y+160, (int)size.X, (int)size.Y), null, Color.White, (float)3.14159265358979323846264338327950288, new Vector2(), new SpriteEffects(), 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
            }
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