using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
        MainMenu, ChooseLevel, ChoosePlay, HumanPlay, ComputerPlay, Help, Finish, GameOver
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
        int moveCount = 0;

        // Bantu User Interface Player
        bool touchflag;
        int touchTimer;
        int lockboat;
        int offx;
        int offy;
        int minscroll;
        int maxscroll;
        int countLoader;
        //TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);

        TimeSpan timeSpan = TimeSpan.FromMilliseconds(180000);

        Sea map;
        List<Boat> boats;

        // Bagian user interface
        Texture2D mainMenu;
        Texture2D menuButton;
        Texture2D levelbox;
        Rectangle menuPlay;
        Rectangle menuHelp;
        Rectangle menuHuman;
        Rectangle menuComputer;

        // Bantu help
        Texture2D boat12;
        Texture2D boat21;
        Texture2D boat13;
        Texture2D boat31;
        Texture2D boatPlayer;
        Texture2D nextButton;
        Texture2D prevButton;
        Texture2D seaTile;
        Texture2D woodTile;

        // Font
        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;
        SpriteFont font4;
        
        Dictionary<int, string> mapLevel;        
        int helpCount = 0;
        string displayTime = "";
        string finishTime = "";
        Boolean startCoutingTime = false;
        int posAwalX, posAkhirX, posAwalY, posAkhirY;

        // DFS dan BFS structure
        Dictionary<String, bool> mapboat;
        List<BoatPath> bestpath;
        int bestmove;
        int state;
        Vector2 posiawal;
        Vector2 posiakhir;
        int moveTimer;
        Thread loading;

        // Bagian Setting
        int speed;
        int flagpath;
        int level;

        /// <summary>
            /// This is constructor for Game1 class.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(250000);

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

            // Setting
            flagpath = 0;
        }

        /// <summary>
            /// This is initialize method for class Game1.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
            /// This is used for loading content for class Game1.
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

        /// <summary>
            /// This is used for loading level and maps for Game1.
        /// </summary>
        public void LoadLevel()
        {
            Stream streampath = TitleContainer.OpenStream("Level.txt");
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
            /// This is called when the game want to unload its content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
            /// This is used for printing listboat.
        /// </summary>
        private String ListBoatToString(List<Boat> listboat)
        {
            String s = "";
            foreach (Boat b in listboat)
            {
                s += b.ToString();
            }
            return s;
        }

        /// <summary>
            /// This is called to fill best path for solution.
        /// </summary>
        /// <param name="flagpass">A flag, 0 for DFS, 1 for BFS.</param>
        public void getPath(Object flagpass)
        {
            int flag = (int)flagpass;
            mapboat = new Dictionary<String, bool>();
            List<Boat> listboatf = new List<Boat>();
            foreach (Boat boat in boats)
            {
                listboatf.Add(new Boat(this,boat));
            }
            List<BoatPath> boatpath = new List<BoatPath>();
            mapboat.Add(ListBoatToString(listboatf), false);
            bestmove = -1;

            if (flag == 0)
            {
                bool cek = true;
                Stack<KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>> stackDFS = new Stack<KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>>();
                stackDFS.Push(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(new KeyValuePair<List<Boat>, List<BoatPath>>(listboatf, boatpath), new Sea(this, map)));

                while ((cek)&&(stackDFS.Count() > 0))
                {
                    KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea> pair = stackDFS.Pop();
                    List<Boat> listboat = pair.Key.Key;
                    List<BoatPath> listpath = pair.Key.Value;
                    Sea localmap = pair.Value;

                    if ((int)(listboat.ElementAt(0).Position.X / 80) + (int)(listboat.ElementAt(0).Size.X / 80) == map.outPos.X)
                    {
                        bestmove = listpath.Count();
                        bestpath = listpath;
                        cek = false;
                    }
                    if (cek)
                    {
                        for (int i = 0; i < listboat.Count(); ++i)
                        {
                            Boat tempboat = listboat.ElementAt(i);
                            if ((tempboat.Arah == Boat.Orientation.Left) || (tempboat.Arah == Boat.Orientation.Right))
                            {
                                int x = (int)(tempboat.Position.X / 80);
                                int y = (int)(tempboat.Position.Y / 80);
                                int sizex = (int)(tempboat.Size.X / 80);
                                if ((x > 0) && (localmap.GetStatus(x - 1, y) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2((x - 1) * 80, y * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x - 1, y, 1);
                                        localmap.SetStatus(x + sizex - 1, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2((x - 1) * 80, y * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        stackDFS.Push(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                                        new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this,localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x - 1, y, 0);
                                    localmap.SetStatus(x + sizex - 1, y, 1);
                                }
                                if ((x + sizex - 1 < 5) && (localmap.GetStatus(x + sizex, y) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2((x + 1) * 80, y * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x + sizex, y, 1);
                                        localmap.SetStatus(x, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2((x + 1) * 80, y * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        stackDFS.Push(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x + sizex, y, 0);
                                    localmap.SetStatus(x, y, 1);
                                }
                            }
                            else if ((tempboat.Arah == Boat.Orientation.Top) || (tempboat.Arah == Boat.Orientation.Bottom))
                            {
                                int x = (int)(tempboat.Position.X / 80);
                                int y = (int)(tempboat.Position.Y / 80);
                                int sizey = (int)(tempboat.Size.Y / 80);
                                if ((y > 0) && (localmap.GetStatus(x, y - 1) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, (y - 1) * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x, y - 1, 1);
                                        localmap.SetStatus(x, y + sizey - 1, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2(x * 80, (y - 1) * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        stackDFS.Push(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x, y - 1, 0);
                                    localmap.SetStatus(x, y + sizey - 1, 1);
                                }
                                if ((y + sizey - 1 < 5) && (localmap.GetStatus(x, y + sizey) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, (y + 1) * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x, y + sizey, 1);
                                        localmap.SetStatus(x, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2(x * 80, (y + 1) * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        stackDFS.Push(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x, y + sizey, 0);
                                    localmap.SetStatus(x, y, 1);
                                }
                            }
                        }
                    }
                }
            }
            else if (flag == 1)
            {
                bool cek = true;
                Queue<KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>> queueBFS = new Queue<KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>>();
                queueBFS.Enqueue(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(new KeyValuePair<List<Boat>, List<BoatPath>>(listboatf, boatpath), new Sea(this, map)));

                while ((cek) && (queueBFS.Count() > 0))
                {
                    KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea> pair = queueBFS.Dequeue();
                    List<Boat> listboat = pair.Key.Key;
                    List<BoatPath> listpath = pair.Key.Value;
                    Sea localmap = pair.Value;

                    if ((int)(listboat.ElementAt(0).Position.X / 80) + (int)(listboat.ElementAt(0).Size.X / 80) == map.outPos.X)
                    {
                        bestmove = listpath.Count();
                        bestpath = listpath;
                        cek = false;
                    }
                    if (cek)
                    {
                        for (int i = 0; i < listboat.Count(); ++i)
                        {
                            Boat tempboat = listboat.ElementAt(i);
                            if ((tempboat.Arah == Boat.Orientation.Left) || (tempboat.Arah == Boat.Orientation.Right))
                            {
                                int x = (int)(tempboat.Position.X / 80);
                                int y = (int)(tempboat.Position.Y / 80);
                                int sizex = (int)(tempboat.Size.X / 80);
                                if ((x > 0) && (localmap.GetStatus(x - 1, y) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2((x - 1) * 80, y * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x - 1, y, 1);
                                        localmap.SetStatus(x + sizex - 1, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2((x - 1) * 80, y * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        queueBFS.Enqueue(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                                        new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x - 1, y, 0);
                                    localmap.SetStatus(x + sizex - 1, y, 1);
                                }
                                if ((x + sizex - 1 < 5) && (localmap.GetStatus(x + sizex, y) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2((x + 1) * 80, y * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x + sizex, y, 1);
                                        localmap.SetStatus(x, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2((x + 1) * 80, y * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        queueBFS.Enqueue(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x + sizex, y, 0);
                                    localmap.SetStatus(x, y, 1);
                                }
                            }
                            else if ((tempboat.Arah == Boat.Orientation.Top) || (tempboat.Arah == Boat.Orientation.Bottom))
                            {
                                int x = (int)(tempboat.Position.X / 80);
                                int y = (int)(tempboat.Position.Y / 80);
                                int sizey = (int)(tempboat.Size.Y / 80);
                                if ((y > 0) && (localmap.GetStatus(x, y - 1) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, (y - 1) * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x, y - 1, 1);
                                        localmap.SetStatus(x, y + sizey - 1, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2(x * 80, (y - 1) * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        queueBFS.Enqueue(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x, y - 1, 0);
                                    localmap.SetStatus(x, y + sizey - 1, 1);
                                }
                                if ((y + sizey - 1 < 5) && (localmap.GetStatus(x, y + sizey) == 0))
                                {
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, (y + 1) * 80);
                                    bool tempbool = false;
                                    if ((!(mapboat.TryGetValue(ListBoatToString(listboat), out tempbool))) || (tempbool))
                                    {
                                        if (!tempbool)
                                            mapboat.Add(ListBoatToString(listboat), false);
                                        else
                                            mapboat[ListBoatToString(listboat)] = false;
                                        localmap.SetStatus(x, y + sizey, 1);
                                        localmap.SetStatus(x, y, 0);
                                        List<BoatPath> templistpath = listpath.ToList();
                                        templistpath.Add(new BoatPath(i, new Vector2(x * 80, y * 80), new Vector2(x * 80, (y + 1) * 80)));
                                        List<Boat> templistboat = new List<Boat>();
                                        foreach (Boat boat in listboat)
                                        {
                                            templistboat.Add(new Boat(this, boat));
                                        }
                                        queueBFS.Enqueue(new KeyValuePair<KeyValuePair<List<Boat>, List<BoatPath>>, Sea>(
                                            new KeyValuePair<List<Boat>, List<BoatPath>>(templistboat, templistpath), new Sea(this, localmap)));
                                    }
                                    listboat.ElementAt(i).Position = new Vector2(x * 80, y * 80);
                                    localmap.SetStatus(x, y + sizey, 0);
                                    localmap.SetStatus(x, y, 1);
                                }
                            }
                        }
                    }
                }
            }
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
                    else if (helpCount == 3) {
                        spriteBatch.DrawString(font1, "1", new Vector2(600, 220), Color.Tomato);
                    }
                    else if (helpCount == 4)
                    {
                        spriteBatch.DrawString(font1, "2", new Vector2(600, 220), Color.Tomato);
                    }

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
                    if (loading != null)
                    {
                        Texture2D rect = new Texture2D(graphics.GraphicsDevice, 240, 80);
                        Color[] data = new Color[240 * 80];
                        for (int i = 0; i < data.Length; ++i) data[i] = Color.DarkSlateBlue;
                        rect.SetData(data);

                        spriteBatch.Draw(rect, new Vector2(300, 200), Color.DarkSlateBlue);
                        String loader = "Loading ";
                        for (int l = 0; l < countLoader/5; ++l)
                        {
                            loader += ".";
                        }
                        countLoader++;
                        if (countLoader > 19)
                            countLoader = 0;
                        spriteBatch.DrawString(font3, loader, new Vector2(330, 213), Color.White);
                    }
                    spriteBatch.End();
                    moveCount = 0;
                    break;
                }
                case GameState.HumanPlay:
                {
                    // Bagian Player
                    GraphicsDevice.Clear(Color.AliceBlue);
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font1, "Level" + level, new Vector2(600, 20), Color.DarkBlue);
                    
                    spriteBatch.DrawString(font1, "Move", new Vector2(600, 180), Color.BurlyWood);
                    spriteBatch.DrawString(font1, ""+moveCount, new Vector2(600, 220), Color.Tomato);

                    spriteBatch.DrawString(font1, "Time", new Vector2(600, 280), Color.BurlyWood);
                    displayTime = String.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                    spriteBatch.DrawString(font1, "" + displayTime, new Vector2(600, 320), Color.Tomato);

                    spriteBatch.End();
                    map.Draw(spriteBatch);

                    foreach(Boat boat in boats)
                    {
                        boat.Draw(spriteBatch);
                    }

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

                    map.Draw(spriteBatch);

                    foreach (Boat boat in boats)
                    {
                        boat.Draw(spriteBatch);
                    }
                    break;
                }

                case GameState.Finish:
                {
                    GraphicsDevice.Clear(Color.FloralWhite);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font1, "Congrats! You've finished Level " + level, new Vector2(40, 20), Color.DarkBlue);
                    spriteBatch.DrawString(font1, "Move : " + moveCount, new Vector2(40, 50), Color.DarkBlue);
                    spriteBatch.DrawString(font1, "Time : " + finishTime, new Vector2(40, 80), Color.DarkBlue);
                    spriteBatch.Draw(nextButton, new Vector2(720, 400), Color.White);
                    spriteBatch.End();
                    break;
                }

                case GameState.GameOver:
                {
                    GraphicsDevice.Clear(Color.FloralWhite);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font1, "GAME OVER " + level, new Vector2(40, 20), Color.Tomato);
                    spriteBatch.DrawString(font1, "You are too slow. Let's try again", new Vector2(40, 50), Color.DarkBlue);
                    spriteBatch.Draw(nextButton, new Vector2(720, 400), Color.White);
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
            if ((startCoutingTime == true) && (timeSpan.TotalSeconds > 0))
                timeSpan -= gameTime.ElapsedGameTime; //update timer
            
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
                    case GameState.Finish:
                    {
                        // Bagian Finish sebuah level
                        _GameState = GameState.ChooseLevel;
                        break;
                    }
                    case GameState.GameOver:
                    {
                        // Bagian Game Over
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
                        startCoutingTime = false;

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
                            if (startCoutingTime == false)
                            {
                                startCoutingTime = true;
                                timeSpan = TimeSpan.FromMilliseconds(180000);
                            }
                            touchflag = true;
                            touchTimer = 5;
                            foreach (TouchLocation t in touchStateMain)
                            {
                                if ((t.State == TouchLocationState.Pressed) && (t.Position.X >= 720) && (t.Position.X <= 800) && (t.Position.Y >= 400) && (t.Position.Y <= 480))
                                {
                                    helpCount++;
                                    if (helpCount == 7)
                                        _GameState = GameState.MainMenu; 
                                }
                                else
                                if ((t.State == TouchLocationState.Pressed) && (t.Position.X >= 640) && (t.Position.X <= 720) && (t.Position.Y >= 400) && (t.Position.Y <= 480))
                                {
                                    if (helpCount > 0)
                                        helpCount--;
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
                        startCoutingTime = false;
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
                        if (loading != null)
                        {
                            if (loading.ThreadState == ThreadState.Stopped)
                            {
                                state = 0;
                                moveTimer = 0;
                                speed = 10;
                                posiawal = bestpath.ElementAt(state).PosisiAwal;
                                posiakhir = bestpath.ElementAt(state).PosisiAkhir;
                                _GameState = GameState.ComputerPlay;
                                loading = null;
                            }
                        }
                        else
                        {
                            startCoutingTime = false;
                            TouchCollection touchStateMain = TouchPanel.GetState();
                            if ((!touchflag) && (touchStateMain.Count > 0))
                            {
                                foreach (TouchLocation t in touchStateMain)
                                {
                                    for (int j = 0; j <= mapLevel.Count / 6; ++j)
                                    {
                                        int tempbatas = (j == (mapLevel.Count / 6)) ? mapLevel.Count % 6 : 6;
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

                                                timeSpan = TimeSpan.FromMilliseconds(1000);

                                                if (play == GameType.Human)
                                                {
                                                    lockboat = -1;
                                                    _GameState = GameState.HumanPlay;
                                                }
                                                else if (play == GameType.Computer)
                                                {
                                                    loading = new Thread(new ParameterizedThreadStart(getPath));
                                                    loading.Start(flagpath);

                                                    countLoader = 0;
                                                }
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
                        }
                        break;
                    }
                    case GameState.HumanPlay:
                    {
                        // Bagian Player
						if (startCoutingTime == false)
                        {
                            startCoutingTime = true;
                            timeSpan = TimeSpan.FromMilliseconds(180000);
                        }
                        if (timeSpan.TotalSeconds == 0) {
                            _GameState = GameState.GameOver;
                            //Debug.WriteLine("game over!");  
                        }
                        TouchCollection touchStateMain = TouchPanel.GetState();
                        if ((!touchflag) && (touchStateMain.Count > 0))
                        {
                            foreach (TouchLocation t in touchStateMain)
                            {
                                if (t.State == TouchLocationState.Released)
                                {
                                    if (lockboat != -1)
                                    {
                                        boats.ElementAt(lockboat).Position = new Vector2((int)((boats.ElementAt(lockboat).Position.X + 40) / 80) * 80, (int)((boats.ElementAt(lockboat).Position.Y + 40) / 80) * 80);
                                        
                                        if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Left) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Right))
                                        {
                                            for (int k = 0; k < (int)(boats.ElementAt(lockboat).Size.X / 80); ++k)
                                            {
                                                map.SetStatus((int)(boats.ElementAt(lockboat).Position.X / 80) + k, (int)(boats.ElementAt(lockboat).Position.Y / 80), 1);
                                            }

                                            posAkhirX = (int)(boats.ElementAt(lockboat).Position.X / 80);
                                            int temp = Math.Abs(posAkhirX - posAwalX);
                                            moveCount += temp;
                                        }
                                        else if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Top) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Bottom))
                                        {
                                            for (int k = 0; k < (int)(boats.ElementAt(lockboat).Size.Y / 80); ++k)
                                            {
                                                map.SetStatus((int)(boats.ElementAt(lockboat).Position.X / 80), (int)(boats.ElementAt(lockboat).Position.Y / 80) + k, 1);
                                            }

                                            posAkhirY = (int)(boats.ElementAt(lockboat).Position.Y / 80);
                                            int temp = Math.Abs(posAkhirY - posAwalY);
                                            moveCount += temp;
                                        }
                                        if (lockboat == 0)
                                        {
                                            posAkhirX += (int)(boats.ElementAt(lockboat).Size.Y / 80);
                                            if (posAkhirX == 5)
                                            {
                                                Debug.WriteLine("finish");
                                                TimeSpan waktuAwal = TimeSpan.FromMilliseconds(180000);
                                                waktuAwal = waktuAwal - timeSpan;
                                                finishTime = String.Format("{0}:{1:D2}", waktuAwal.Minutes, waktuAwal.Seconds);
                                                for (int i = 0; i < 10000000; i++);
                                                _GameState = GameState.Finish;
                                            }
                                        }
                                        lockboat = -1;
                                    }
                                }
                                else if (t.State == TouchLocationState.Moved)
                                {
                                    if (lockboat == -1)
                                    {
                                        bool cek = true;
                                        int count = 0;
                                        foreach (Boat boat in boats)
                                        {
                                            if ((cek) && ((t.Position.X >= boat.Position.X) && (t.Position.X <= boat.Position.X + boat.Size.X)) &&
                                                ((t.Position.Y >= boat.Position.Y) && (t.Position.Y <= boat.Position.Y + boat.Size.Y)))
                                            {
                                                posAwalX = (int)(boat.Position.X) / 80; 
                                                posAwalY = (int)(boat.Position.Y) / 80; 

                                                cek = false;
                                                lockboat = count;
                                                offx = (int)(boat.Position.X - t.Position.X); 
                                                offy = (int)(boat.Position.Y - t.Position.Y); 
                                                minscroll = -1;
                                                maxscroll = 6;
                                                if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Left) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Right))
                                                {
                                                    for (int k = 0; k < (int)(boats.ElementAt(lockboat).Size.X / 80); ++k)
                                                    {
                                                        map.SetStatus((int)(boats.ElementAt(lockboat).Position.X / 80) + k, (int)(boats.ElementAt(lockboat).Position.Y / 80), 0);
                                                    }
                                                }
                                                else if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Top) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Bottom))
                                                {
                                                    for (int k = 0; k < (int)(boats.ElementAt(lockboat).Size.Y / 80); ++k)
                                                    {
                                                        map.SetStatus((int)(boats.ElementAt(lockboat).Position.X / 80), (int)(boats.ElementAt(lockboat).Position.Y / 80) + k, 0);
                                                    }
                                                }
                                            }
                                            count++;
                                        }
                                    }
                                    else
                                    {
                                        int max = 480;
                                        if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Left) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Right))
                                        {
                                            if ((t.Position.X + offx >= 0) && (t.Position.X + offx <= (max - boats.ElementAt(lockboat).Size.X)))
                                            {
                                                if (((int)((t.Position.X + offx + 40) / 80) > minscroll) && ((int)((t.Position.X + offx + boats.ElementAt(lockboat).Size.X - 40) / 80) < maxscroll) &&
                                                    (map.GetStatus((int)((t.Position.X + offx + 40) / 80), (int)(boats.ElementAt(lockboat).Position.Y / 80)) == 0) &&
                                                    (map.GetStatus((int)((t.Position.X + offx + boats.ElementAt(lockboat).Size.X - 40) / 80), (int)(boats.ElementAt(lockboat).Position.Y / 80)) == 0))
                                                    boats.ElementAt(lockboat).Position = new Vector2(t.Position.X + offx, boats.ElementAt(lockboat).Position.Y);
                                                else if (t.Position.X + offx < boats.ElementAt(lockboat).Position.X)
                                                {
                                                    minscroll = (int)((t.Position.X + offx + 40) / 80);
                                                }
                                                else if (t.Position.X + offx > boats.ElementAt(lockboat).Position.X)
                                                {
                                                    maxscroll = (int)((t.Position.X + offx + 40) / 80);
                                                }
                                            }
                                        }
                                        else if ((boats.ElementAt(lockboat).Arah == Boat.Orientation.Top) || (boats.ElementAt(lockboat).Arah == Boat.Orientation.Bottom))
                                        {
                                            if ((t.Position.Y + offy >= 0) && (t.Position.Y + offy <= (max - boats.ElementAt(lockboat).Size.Y)))
                                            {
                                                if (((int)((t.Position.Y + offy + 40) / 80) > minscroll) && ((int)((t.Position.Y + offy + boats.ElementAt(lockboat).Size.Y - 40) / 80) < maxscroll) && 
                                                    (map.GetStatus((int)(boats.ElementAt(lockboat).Position.X / 80), (int)(t.Position.Y + offy + 40) / 80) == 0) &&
                                                    (map.GetStatus((int)(boats.ElementAt(lockboat).Position.X / 80), (int)(t.Position.Y + offy + boats.ElementAt(lockboat).Size.Y - 40) / 80) == 0))
                                                    boats.ElementAt(lockboat).Position = new Vector2(boats.ElementAt(lockboat).Position.X, t.Position.Y + offy);
                                                else if (t.Position.Y + offy < boats.ElementAt(lockboat).Position.Y)
                                                    minscroll = (int)((t.Position.Y + offy + 40) / 80);
                                                else if (t.Position.Y + offy > boats.ElementAt(lockboat).Position.Y)
                                                    maxscroll = (int)((t.Position.Y + offy + boats.ElementAt(lockboat).Size.Y - 40) / 80);
                                            }
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
                    case GameState.ComputerPlay:
                    {
                        // Bagian Computer
                        if (state < bestpath.Count())
                        {
                            moveTimer++;
                            if (moveTimer == 1)
                            {
                                moveTimer = 0;
                                if (posiawal == posiakhir)
                                {
                                    state++;
                                    if (state < bestpath.Count())
                                    {
                                        posiawal = bestpath.ElementAt(state).PosisiAwal;
                                        posiakhir = bestpath.ElementAt(state).PosisiAkhir;
                                    }
                                }
                                else if (posiawal.Y == posiakhir.Y)
                                {
                                    if (posiawal.X < posiakhir.X)
                                        posiawal.X = (posiawal.X + speed > posiakhir.X)? posiakhir.X : posiawal.X + speed;
                                    else
                                        posiawal.X = (posiawal.X - speed < posiakhir.X) ? posiakhir.X : posiawal.X - speed;
                                }
                                else if (posiawal.X == posiakhir.X)
                                {
                                    if (posiawal.Y < posiakhir.Y)
                                        posiawal.Y = (posiawal.Y + speed > posiakhir.Y) ? posiakhir.Y : posiawal.Y + speed;
                                    else
                                        posiawal.Y = (posiawal.Y - speed < posiakhir.Y) ? posiakhir.Y : posiawal.Y - speed;
                                }
                                
                                if (state < bestpath.Count())
                                    boats.ElementAt(bestpath.ElementAt(state).Boat).Position = posiawal;
                            }
                        }
                        break;
                    }
                    case GameState.Finish:
                    {
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
                                    _GameState = GameState.ChooseLevel;
                                }
                            }
                        }
                        else
                        {
                            if (touchTimer == 0) touchflag = false;
                            else touchTimer--;
                        }
                        break;
                    }
                    case GameState.GameOver:
                    {
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
                                    _GameState = GameState.ChooseLevel;
                                }
                            }
                        }
                        else
                        {
                            if (touchTimer == 0) touchflag = false;
                            else touchTimer--;
                        }
                        break;
                    }
                    default: break;
                }
            }
            base.Update(gameTime);
        }
    }
}