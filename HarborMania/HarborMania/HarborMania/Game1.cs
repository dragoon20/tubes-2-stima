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
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);

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
        Texture2D prevButton;
        Texture2D seaTile;
        Texture2D woodTile;

        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;
        SpriteFont font4;
        Dictionary<int, string> mapLevel;
        
        Rectangle menuPlay;
        Rectangle menuHelp;
        Rectangle menuHuman;
        Rectangle menuComputer;

        int helpCount = 0;
        int level;
        string displayTime = "";

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

            boat12 = Content.Load<Texture2D>("Boat/Boat12");
            boat21 = Content.Load<Texture2D>("Boat/Boat21");
            boat13 = Content.Load<Texture2D>("Boat/Boat13");
            boat31 = Content.Load<Texture2D>("Boat/Boat31");
            boatPlayer = Content.Load<Texture2D>("Boat/BoatPlayer");
            nextButton = Content.Load<Texture2D>("Next");
            prevButton = Content.Load<Texture2D>("Previous");
            seaTile = Content.Load<Texture2D>("Tile/Sea");
            woodTile = Content.Load<Texture2D>("Tile/Wood");

            font1 = Content.Load<SpriteFont>("Font/SpriteFont1");
            font2 = Content.Load<SpriteFont>("Font/SpriteFont2");
            font3 = Content.Load<SpriteFont>("Font/SpriteFont3");
            font4 = Content.Load<SpriteFont>("Font/SpriteFont4");
            
            LoadLevel();
        }

        public void LoadLevel()
        {
            Stream streampath = TitleContainer.OpenStream("level.txt");
            StreamReader loadpath = new StreamReader(streampath);

            mapLevel = new Dictionary<int,string>();

            String templine = loadpath.ReadLine();
            while (templine != null)
            {
                String[] splitline = templine.Split(' ');

                if (splitline.Count() > 1)
                    mapLevel.Add(Convert.ToInt32(splitline[0]), splitline[1].ToString());
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
                            if (i == 6)
                            {
                                if (j == 4)
                                    spriteBatch.Draw(seaTile, new Vector2(i * 80, j * 80), Color.White);
                                else
                                    spriteBatch.Draw(woodTile, new Vector2(i * 80, j * 80), Color.White);
                            }
                            else
                                spriteBatch.Draw(seaTile, new Vector2(i * 80, j * 80), Color.White);
                        }
                    }

                    spriteBatch.DrawString(font1, "HARBOR MANIA", new Vector2(600, 10), Color.BurlyWood);
                    spriteBatch.Draw(nextButton, new Vector2(720, 400), Color.White);
                    spriteBatch.Draw(prevButton, new Vector2(640, 400), Color.White);
                    spriteBatch.Draw(boat13, new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(font1, "Boat moved", new Vector2(600, 180), Color.BurlyWood);
                    spriteBatch.DrawString(font1, "Time", new Vector2(600, 280), Color.BurlyWood);
                    displayTime = String.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                    spriteBatch.DrawString(font1, "" + displayTime, new Vector2(600, 320), Color.Tomato);

                    if (helpCount < 3)
                    {
                        spriteBatch.Draw(boat21, new Vector2(320, 160), Color.White);
                        spriteBatch.DrawString(font1, "0", new Vector2(600, 220), Color.Tomato);
                    }

                    if (helpCount < 4) { spriteBatch.Draw(boat12, new Vector2(400, 240), Color.White); }
                    if (helpCount < 5) { spriteBatch.Draw(boatPlayer, new Vector2(0, 320), Color.White); }

                    if (helpCount == 0)
                    {
                        spriteBatch.DrawString(font1, "Drag your wood", new Vector2(560, 80), Color.Chocolate);
                        spriteBatch.DrawString(font1, "boat to the harbor!", new Vector2(560, 120), Color.Chocolate);
                    }
                    else
                        if (helpCount == 1)
                        {
                            spriteBatch.DrawString(font4, "Oops! This ship is", new Vector2(200, 300), Color.Black);
                            spriteBatch.DrawString(font4, "blocking your way", new Vector2(200, 320), Color.Black);
                            spriteBatch.DrawString(font4, "Why don't you move it?", new Vector2(200, 340), Color.Black);
                        }
                        else
                            if (helpCount == 2)
                            {
                                spriteBatch.DrawString(font4, "But before that, ", new Vector2(140, 160), Color.Black);
                                spriteBatch.DrawString(font4, "you've got to move", new Vector2(140, 180), Color.Black);
                                spriteBatch.DrawString(font4, "this ship first.", new Vector2(140, 200), Color.Black);
                            }
                            else if (helpCount == 3) spriteBatch.DrawString(font1, "1", new Vector2(600, 220), Color.Tomato);
                            else if (helpCount == 4) spriteBatch.DrawString(font1, "2", new Vector2(600, 220), Color.Tomato);

                    if (helpCount >= 3) spriteBatch.Draw(boat21, new Vector2(160, 160), Color.White);
                    if (helpCount >= 4) spriteBatch.Draw(boat12, new Vector2(400, 80), Color.White);
                    if (helpCount >= 5)
                    {
                        spriteBatch.Draw(boatPlayer, new Vector2(400, 320), Color.White);
                        spriteBatch.DrawString(font1, "3", new Vector2(600, 220), Color.Tomato);
                    }
                    if (helpCount == 6)
                    {
                        spriteBatch.DrawString(font4, "There you go..., ", new Vector2(80, 320), Color.Black);
                        spriteBatch.DrawString(font4, "Now that you've understand", new Vector2(80, 340), Color.Black);
                        spriteBatch.DrawString(font4, "how to play this game.", new Vector2(80, 360), Color.Black);
                        spriteBatch.DrawString(font4, "Let's start now!", new Vector2(80, 380), Color.Black);
                    }
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
                    for (int j = 0; j <= mapLevel.Count/6; ++j)
                    {
                        int tempbatas = (j == (mapLevel.Count/6)) ? mapLevel.Count % 6 : 6;
                        for (int i = 0; i < tempbatas; ++i)
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
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font1, "Level" + level, new Vector2(600, 20), Color.DarkBlue);
                    spriteBatch.DrawString(font1, "Time", new Vector2(600, 280), Color.BurlyWood);
                    displayTime = String.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                    spriteBatch.DrawString(font1, "" + displayTime, new Vector2(600, 320), Color.Tomato);

                    spriteBatch.End();

                    map.Draw(spriteBatch);
                    break;
                }
                case GameState.ComputerPlay:
                {
                    // Bagian Computer
                    GraphicsDevice.Clear(Color.FloralWhite);
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font1, "Level" + level, new Vector2(600, 20), Color.DarkBlue);
                    spriteBatch.DrawString(font1, "Time", new Vector2(600, 280), Color.BurlyWood);
                    displayTime = String.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                    spriteBatch.DrawString(font1, "" + displayTime, new Vector2(600, 320), Color.Tomato);

                    spriteBatch.End();
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
            timeSpan += gameTime.ElapsedGameTime; //update timer

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
                                    helpCount = 0;
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
                                    if (helpCount == 7)
                                        _GameState = GameState.MainMenu; 
                                    else
                                    if ((t.State == TouchLocationState.Pressed) && (t.Position.X >= 640) && (t.Position.X <= 720) && (t.Position.Y >= 400) && (t.Position.Y <= 480))
                                    {
                                        if (helpCount > 0) 
                                            helpCount--;
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
                                for (int j = 0; j <= mapLevel.Count/6; ++j)
                                {
                                    int tempbatas = (j == (mapLevel.Count/6)) ? mapLevel.Count % 6 : 6;
                                    for (int i = 0; i < tempbatas; ++i)
                                    {
                                        if ((t.State == TouchLocationState.Pressed) && ((t.Position.X >= 60 + i * 115) && ((t.Position.X <= 60 + i * 115 + 100))) &&
                                        ((t.Position.Y >= 110 + j * 110) && ((t.Position.Y <= 110 + j * 110 + 100))))
                                        {
                                            // Jika menyentuh menu tertentu
                                            level = j * 6 + i + 1; //ditambah 1 karena level mulai dari 1, bukan dari 0
                                            map = new Sea(this, 80, 80, 6, 6, mapLevel[level]);
                                            map.Initialize();
                                            map.LoadContent(out boats);

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