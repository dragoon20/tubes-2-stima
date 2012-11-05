using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

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
        Vector2 resolution = new Vector2(800, 480);

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
        Texture2D levelbox;

        Texture2D boat12;
        Texture2D boat21;
        Texture2D boat13;
        Texture2D boat31;
        Texture2D boatPlayer;
        Texture2D nextButton;
        Texture2D gameTitle;
        Texture2D clue;
        Texture2D seaTile;
        Texture2D woodTile;

        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;
        Dictionary<int, string> mapLevel;
        int level;

        Rectangle menuPlay;
        Rectangle menuHelp;
        Rectangle menuHuman;
        Rectangle menuComputer;

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
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            graphics.PreferredBackBufferWidth = (int)resolution.X;

            // Bagian konfigurasi developer
            // Set GameState jadi menu utama
            _GameState = GameState.MainMenu;
            touchflag = false;
            menuPlay = new Rectangle(30, 40, 300, 61);
            menuHelp = new Rectangle(30, 118, 300, 61);
            menuHuman = new Rectangle(30, 95, 300, 61);
            menuComputer = new Rectangle(30, 173, 300, 61);
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

            // TODO: use this.Content to load your game content here
            mainMenu = Content.Load<Texture2D>("MainScreen");
            menuButton = Content.Load<Texture2D>("menu_button");
            levelbox = Content.Load<Texture2D>("Rect");

            boat12 = Content.Load<Texture2D>("boat12");
            boat21 = Content.Load<Texture2D>("boat21");
            boat13 = Content.Load<Texture2D>("boat13");
            boat31 = Content.Load<Texture2D>("boat31");
            boatPlayer = Content.Load<Texture2D>("BoatPlayer");
            nextButton = Content.Load<Texture2D>("Next");
            gameTitle = Content.Load<Texture2D>("HarborMania");
            seaTile = Content.Load<Texture2D>("Sea");
            woodTile = Content.Load<Texture2D>("Wood");
            clue = Content.Load<Texture2D>("Clue_1");

            font1 = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            font3 = Content.Load<SpriteFont>("SpriteFont3");
            //LoadLevel();
        }

        public void LoadLevel()
        {
            Stream streampath = TitleContainer.OpenStream("level.txt");
            StreamReader loadpath = new StreamReader(streampath);

            String templine = loadpath.ReadLine();
            while (templine != null)
            {
                String[] splitline = templine.Split(' ');
                if (splitline.Count() > 0)
                    mapLevel.Add(Convert.ToInt32(splitline[0]), splitline[1]);
                templine = loadpath.ReadLine();
            }
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
                        spriteBatch.DrawString(font1, "Mulai Permainan", new Vector2(menuPlay.X + 70, menuPlay.Y + 10), Color.White);
                        spriteBatch.Draw(menuButton, menuHelp, Color.White);
                        spriteBatch.DrawString(font1, "Petunjuk", new Vector2(menuHelp.X + 70, menuHelp.Y + 10), Color.White);
                        spriteBatch.End();
                        break;
                    }
                case GameState.Help:
                    {
                        // Bagian Help
                        GraphicsDevice.Clear(Color.AntiqueWhite);
                        spriteBatch.Begin();
                        for (int i = 0; i < 7; i++)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                if ((i==6) && (j==4))
                                    spriteBatch.Draw(woodTile, new Vector2(i*80, j*80), Color.White);
                                else
                                    spriteBatch.Draw(seaTile, new Vector2(i*80, j*80), Color.White);
                            }
                        }
                        spriteBatch.Draw(boat21, new Vector2(320, 160), Color.White);
                        spriteBatch.Draw(boat12, new Vector2(400, 240), Color.White);
                        spriteBatch.Draw(boat13, new Vector2(0, 0), Color.White);
                        spriteBatch.Draw(boatPlayer, new Vector2(0, 320), Color.White);
                        spriteBatch.Draw(gameTitle, new Vector2(560, 0), Color.White);
                        spriteBatch.Draw(clue, new Vector2(560, 80), Color.White);
                        spriteBatch.Draw(nextButton, new Vector2(720, 400), Color.White);

                        spriteBatch.End();
                        break;
                    }
                case GameState.ChoosePlay:
                    {
                        // Bagian Pilih jenis permainan
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                        spriteBatch.DrawString(font3, "Jenis Permainan", new Vector2(40, 30), Color.DarkSlateBlue);
                        spriteBatch.Draw(menuButton, menuHuman, Color.White);
                        spriteBatch.DrawString(font1, "Player Main", new Vector2(menuHuman.X + 70, menuHuman.Y + 10), Color.White);
                        spriteBatch.Draw(menuButton, menuComputer, Color.White);
                        spriteBatch.DrawString(font1, "Computer Main", new Vector2(menuComputer.X + 70, menuComputer.Y + 10), Color.White);
                        spriteBatch.End();
                        break;
                    }
                case GameState.ChooseLevel:
                    {
                        // Bagian Pilih Level
                        spriteBatch.Begin();
                        spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                        spriteBatch.DrawString(font3, "Pilih Level", new Vector2(40, 30), Color.DarkSlateBlue);
                        for (int j = 0; j < 3; ++j)
                        {
                            for (int i = 0; i < 6; ++i)
                            {
                                spriteBatch.Draw(levelbox, new Rectangle(60 + i * 115, 110 + j * 110, 100, 100), Color.White);
                                int digit = 0;
                                int temp = ((j * 6) + (i + 1));
                                while (temp >= 10)
                                {
                                    temp /= 10;
                                    digit++;
                                }
                                spriteBatch.DrawString(font2, ((j * 6) + (i + 1)).ToString(), new Vector2(98 - (digit * 14) + i * 115, 135 + j * 110), Color.White);
                            }
                        }
                        spriteBatch.End();
                        break;
                    }
                case GameState.HumanPlay:
                    {
                        // Bagian Player
                        GraphicsDevice.Clear(Color.AliceBlue);
                        break;
                    }
                case GameState.ComputerPlay:
                    {
                        // Bagian Computer
                        GraphicsDevice.Clear(Color.BurlyWood);
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
        int helpCount = 0;
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
                            if ((!touchflag) && (touchStateMain.Count > 0))
                            {
                                // Kalau ditouch
                                touchflag = true;
                                touchTimer = 5;
                                foreach (TouchLocation t in touchStateMain)
                                {
                                    if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= menuPlay.X) && ((t.Position.X <= menuPlay.X + menuPlay.Width))) &&
                                        ((t.Position.Y >= menuPlay.Y) && ((t.Position.Y <= menuPlay.Y + menuPlay.Height))))
                                    {
                                        // Jika menyentuh menu tertentu
                                        _GameState = GameState.ChoosePlay;
                                    }
                                    else if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= menuHelp.X) && ((t.Position.X <= menuHelp.X + menuHelp.Width))) &&
                                         ((t.Position.Y >= menuHelp.Y) && ((t.Position.Y <= menuHelp.Y + menuHelp.Height))))
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
                            //Vector2 v = new Vector2(0,0);
                            TouchCollection touchStateMain = TouchPanel.GetState();
                            if ((!touchflag) && (touchStateMain.Count > 0))
                            {
                                // Kalau di-touch
                                touchflag = true;
                                touchTimer = 5;
                                foreach (TouchLocation t in touchStateMain)
                                {
                                    if ((t.State == TouchLocationState.Pressed) && (t.Position.X >= 720) && (t.Position.X <= 800) && (t.Position.Y >= 400) && (t.Position.Y <= 480))
                                    {
                                        helpCount++;
                                        spriteBatch.Begin();
                                        if (helpCount == 1)
                                        {
                                            clue = Content.Load<Texture2D>("Clue_2");
                                            //320, 400
                                            spriteBatch.Draw(clue, new Vector2(0,0), Color.White);
                                        }
                                        if (helpCount == 2)
                                        {
                                            clue = Content.Load<Texture2D>("Clue_3");
                                            //240, 320
                                            spriteBatch.Draw(clue, new Vector2(80,80), Color.White);
                                        }
                                        spriteBatch.End();
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
                                    if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= menuHuman.X) && ((t.Position.X <= menuHuman.X + menuHuman.Width))) &&
                                        ((t.Position.Y >= menuHuman.Y) && ((t.Position.Y <= menuHuman.Y + menuHuman.Height))))
                                    {
                                        // Jika menyentuh menu tertentu
                                        play = GameType.Human;
                                        _GameState = GameState.ChooseLevel;
                                    }
                                    else if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= menuComputer.X) && ((t.Position.X <= menuComputer.X + menuComputer.Width))) &&
                                         ((t.Position.Y >= menuComputer.Y) && ((t.Position.Y <= menuComputer.Y + menuComputer.Height))))
                                    {
                                        // Jika menyentuh menu tertentu
                                        play = GameType.Computer;
                                        _GameState = GameState.ChooseLevel;
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
                    case GameState.ChooseLevel:
                        {
                            // Bagian Pilih Level
                            TouchCollection touchStateMain = TouchPanel.GetState();
                            if ((!touchflag) && (touchStateMain.Count > 0))
                            {
                                foreach (TouchLocation t in touchStateMain)
                                {
                                    for (int j = 0; j < 3; ++j)
                                    {
                                        for (int i = 0; i < 6; ++i)
                                        {
                                            if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= 60 + i * 115) && ((t.Position.X <= 60 + i * 115 + 100))) &&
                                            ((t.Position.Y >= 110 + j * 110) && ((t.Position.Y <= 110 + j * 110 + 100))))
                                            {
                                                // Jika menyentuh menu tertentu
                                                level = j * 6 + i;
                                                if (play == GameType.Human)
                                                    _GameState = GameState.HumanPlay;
                                                else if (play == GameType.Computer)
                                                    _GameState = GameState.ComputerPlay;

                                            }
                                        }
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