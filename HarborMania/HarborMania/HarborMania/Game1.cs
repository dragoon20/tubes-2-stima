using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace HarborMania
{
    enum GameState
    {
        MainMenu, ChooseLevel, ChoosePlay, HumanPlay, ComputerPlay, Help
    };

    enum GameType
    {
        Human, Computer
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Vector2 resolution = new Vector2(800,480);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState _GameState;
        GameType play;
        bool touchflag;
        int touchTimer;

        Sea map;
        List<Boat> boats;
        Texture2D mainMenu;
        Texture2D menuButton;
        SpriteFont font1;
        int level;

        Rectangle menuPlay;
        Rectangle menuHelp;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            graphics.PreferredBackBufferHeight = (int) resolution.Y;
            graphics.PreferredBackBufferWidth = (int) resolution.X;

            // Bagian konfigurasi developer
            // Set GameState jadi menu utama
            _GameState = GameState.MainMenu;
            touchflag = false;
            menuPlay = new Rectangle(30, 40, 300, 61);
            menuHelp = new Rectangle(30, 118, 300, 61);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainMenu = Content.Load<Texture2D>("MainScreen");
            menuButton = Content.Load<Texture2D>("menu_button");
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            switch (_GameState)
            {
                case GameState.MainMenu:
                {
                    // Bagian Menu Utama
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                    spriteBatch.Draw(menuButton, menuPlay, Color.White);
                    spriteBatch.DrawString(font1, "Mulai Permainan", new Vector2(menuPlay.X+70,menuPlay.Y+10), Color.White);
                    spriteBatch.Draw(menuButton, menuHelp, Color.White);
                    spriteBatch.DrawString(font1, "Petunjuk", new Vector2(menuHelp.X + 70, menuHelp.Y + 10), Color.White);
                    spriteBatch.End();
                    break;
                }
                case GameState.Help:
                {
                    // Bagian Help
                    GraphicsDevice.Clear(Color.Tomato);
                    break;
                }
                case GameState.ChoosePlay:
                {
                    // Bagian Pilih jenis permainan
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                    spriteBatch.Draw(menuButton, menuPlay, Color.White);
                    spriteBatch.DrawString(font1, "Player Main", new Vector2(menuPlay.X + 70, menuPlay.Y + 10), Color.White);
                    spriteBatch.Draw(menuButton, menuHelp, Color.White);
                    spriteBatch.DrawString(font1, "Computer Main", new Vector2(menuHelp.X + 70, menuHelp.Y + 10), Color.White);
                    spriteBatch.End();
                    break;
                }
                case GameState.ChooseLevel:
                {
                    // Bagian Pilih Level
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                    spriteBatch.End();
                    break;
                }
                case GameState.HumanPlay:
                {
                    // Bagian Player
                    break;
                }
                case GameState.ComputerPlay:
                {
                    // Bagian Computer
                    break;
                }
                default: break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                switch (_GameState)
                {
                    case GameState.MainMenu:
                    {
                        // Bagian Menu Utama
                        this.Exit();
                        break;
                    }
                    case GameState.Help:
                    {
                        // Bagian Help
                        _GameState = GameState.MainMenu;
                        break;
                    }
                    case GameState.ChoosePlay:
                    {
                        // Bagian Pilih jenis permainan
                        _GameState = GameState.MainMenu;
                        break;
                    }
                    case GameState.ChooseLevel:
                    {
                        // Bagian Pilih Level
                        _GameState = GameState.ChoosePlay;
                        break;
                    }
                    case GameState.HumanPlay:
                    {
                        // Bagian Player
                        _GameState = GameState.ChooseLevel;
                        break;
                    }
                    case GameState.ComputerPlay:
                    {
                        // Bagian Computer
                        _GameState = GameState.ChooseLevel;
                        break;
                    }
                    default: break;
                }
            }
            else
            {
                // TODO: Add your update logic here
                switch (_GameState)
                {
                    case GameState.MainMenu:
                    {
                        // Bagian Menu Utama
                        TouchCollection touchStateMain = TouchPanel.GetState();
                        if ((!touchflag)&&(touchStateMain.Count > 0))
                        {
                            // Kalau ditouch
                            touchflag = true;
                            touchTimer = 5;
                            foreach (TouchLocation t in touchStateMain)
                            {
                                if (((t.Position.X >= 50) && ((t.Position.X <= 350))) &&
                                    ((t.Position.Y >= 50) && ((t.Position.Y <= 111))))
                                {
                                    // Jika menyentuh menu tertentu
                                    _GameState = GameState.ChoosePlay;
                                }
                                else if (((t.Position.X >= 50) && ((t.Position.X <= 350))) &&
                                     ((t.Position.Y >= 128) && ((t.Position.Y <= 189))))
                                {
                                    // Jika menyentuh menu tertentu
                                    _GameState = GameState.Help;
                                }
                            }
                        }
                        else
                        {
                            if (touchTimer == 0)
                            {
                                touchflag = false;
                            }
                            else
                            {
                                touchTimer--;
                            }
                        }
                        break;
                    }
                    case GameState.Help:
                    {
                        // Bagian Help
                        break;
                    }
                    case GameState.ChoosePlay:
                    {
                        // Bagian Pilih jenis permainan
                        TouchCollection touchStateMain = TouchPanel.GetState();
                        if ((!touchflag) && (touchStateMain.Count > 0))
                        {
                            // Kalau ditouch
                            touchflag = true;
                            touchTimer = 5;
                            foreach (TouchLocation t in touchStateMain)
                            {
                                if (((t.Position.X >= 50) && ((t.Position.X <= 350))) &&
                                    ((t.Position.Y >= 50) && ((t.Position.Y <= 111))))
                                {
                                    // Jika menyentuh menu tertentu
                                    play = GameType.Human;
                                }
                                else if (((t.Position.X >= 50) && ((t.Position.X <= 350))) &&
                                     ((t.Position.Y >= 128) && ((t.Position.Y <= 189))))
                                {
                                    // Jika menyentuh menu tertentu
                                    play = GameType.Computer;
                                }
                            }
                            _GameState = GameState.ChooseLevel;
                        }
                        else
                        {
                            if (touchTimer == 0)
                            {
                                touchflag = false;
                            }
                            else
                            {
                                touchTimer--;
                            }
                        }
                        break;
                    }
                    case GameState.ChooseLevel:
                    {
                        // Bagian Pilih Level
                        break;
                    }
                    case GameState.HumanPlay:
                    {
                        // Bagian Player
                        break;
                    }
                    case GameState.ComputerPlay:
                    {
                        // Bagian Computer
                        break;
                    }
                    default: break;
                }
            }

            base.Update(gameTime);
        }
    }
}
