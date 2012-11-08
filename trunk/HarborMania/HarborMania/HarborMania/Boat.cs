using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarborMania
{
    /// <summary>
        /// Class Boat is used for saving boat condition.
        /// <list type="Properties">
            /// <item>
                /// <description>Position is used for marking position of boat from top, left.</description>
            /// </item>
            /// <item>
                /// <description>Size is used for the value of width(X) and height(Y) of boat.</description>
            /// </item>
            /// <item>
                /// <description>Arah is used for saving the orientation of the boat.</description>
            /// </item>
            /// <item>
                /// <description>Texture is used for saving the image for boat.</description>
            /// </item>
        /// </list>
    /// </summary>
    class Boat : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Orientation
        {
            Top, Right, Bottom, Left
        };

        Vector2 position;
        Vector2 size;
        Orientation arah;
        Texture2D texture;

        /// <summary>
            /// Getter and Setter for element arah.
        /// </summary>
        public Orientation Arah
        {
            get { return arah; }
            set { arah = value; }
        }

        /// <summary>
            /// Getter and Setter for element size.
        /// </summary>
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
            /// Getter and Setter for element position.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
            /// Constructor for Boat class.
        /// </summary>
        /// <param name="game">Game class for this Drawable Game Component.</param>
        /// <param name="boat">Boat to be copied.</param>
        public Boat(Game game, Boat boat): base(game)
        {
            position = boat.position;
            size = boat.size;
            arah = boat.arah;
        }

        /// <summary>
            /// Constructor for Boat class.
        /// </summary>
        /// <param name="game">Game class for this Drawable Game Component.</param>
        public Boat(Game game) : base(game)
        {
            position = new Vector2();
            size = new Vector2();
        }

        /// <summary>
            /// Constructor for Boat class.
        /// </summary>
        /// <param name="game">Game class for this Drawable Game Component.</param>
        /// <param name="Position">Position of the boat.</param>
        /// <param name="Size">Size of the boat.</param>
        /// <param name="Texture">Texture of the boat.</param>
        /// <param name="w">The width per tile.</param>
        /// <param name="h">The height per tile.</param>
        public Boat(Game game, Vector2 Position, Vector2 Size, int w, int h, Texture2D Texture) : base(game)
        {
            texture = Texture;
            position = new Vector2 (Position.X * w, Position.Y * h);
            size = new Vector2(Size.X * w, Size.Y * h);
        }

        /// <summary>
            /// Method for drawing the boat to the screen.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch to be used for Drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (arah == Orientation.Left)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(position.X+size.X), (int)(position.Y+size.Y), (int)size.X, (int)size.Y), null, Color.White, (float)3.14159265358979323846264338327950288, new Vector2(), new SpriteEffects(), 0);
            }
            else if (arah == Orientation.Bottom)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(position.X+size.X), (int)(position.Y+size.Y), (int)size.X, (int)size.Y), null, Color.White, (float)3.14159265358979323846264338327950288, new Vector2(), new SpriteEffects(), 0);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
            }
            spriteBatch.End();
        }

        /// <summary>
            /// A method overriden to initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
            /// A method overriden to update the DrawableGameComponent.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
            /// An method overriden to print Boat as String.
        /// </summary>
        /// <returns>String of Boat position and size.</returns>
        public override string ToString()
        {
            return "((" + position.X + "," + position.Y + "),(" + size.X + "," + size.Y + "))";
        }
    }
}