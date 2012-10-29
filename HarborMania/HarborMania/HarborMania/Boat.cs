using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    class Boat
    {
        int height;
        int weight;
        int posX; //posX adalah posisi X dari boat head
        int posY; //posY adalah posisi Y dari boat head
        int arah; //arah adalah arah dari boat head
        Texture2D texture; //masih blm tau ini utk apa

        public Boat(Texture2D texture_, int height_, int weight_, int posX_, int posY_, int arah_)
        {
            texture = texture_;
            height = height_;
            weight = weight_;
            posX = posX_;
            posY = posY_;
            arah = arah_;
        }

        public void Draw (SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(texture, position, color);
            spriteBatch.End();
        }

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
        }

    }
}

