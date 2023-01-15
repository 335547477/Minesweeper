/*
 * Author: Kinjal Padhiar
 * File Name: Game1.cs
 * Project Name: PadhiarK_MP2
 * Creation Date: April 19th, 2021
 * Modified Date: May 1st, 2021
 * Description: A replica of Google Minesweeper
 */
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Helper;

namespace PadhiarK_MP2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private StreamWriter outFile;
        private StreamReader inFile;

        Random rnd = new Random();
        SpriteFont text;

        public static int row = 8;
        public static int col = 10;
        public static int size = 45;
        Tile[,] tiles = new Tile[row, col];
        int locx = 0;
        int locy = size;

        int hasMineCount = 0;

        int mineCount = 10;
        int screenWidth;
        int screenHeight;
        int num;

        MouseState mouse;
        MouseState prevmb;

        Texture2D bgEasy;
        Texture2D bgMed;
        Texture2D bgHard;
        Rectangle bgEasyRect;
        Rectangle bgMedRect;
        Rectangle bgHardRect;


        Texture2D flagImg;
        Rectangle flagRect;
        int flagCount = 10;
        int startFlagCount = 10;
        int correctFlag = 0;

        Texture2D tileDark;
        Texture2D tileLight;

        Texture2D dropDownImg;
        bool dropDown = false;
        Texture2D easyButImg;
        Texture2D mediumButImg;
        Texture2D hardButImg;
        Texture2D checkImg;
        Texture2D watchImg;
        Texture2D exitImg;
        Texture2D soundOnImg;
        Texture2D soundOffImg;
        bool sound = true;

        Texture2D rulesLeftImg;
        Texture2D rulesRightImg;
        Rectangle rulesRectLeft;
        Rectangle rulesRectRight;
        bool rulesLeft;
        bool rulesRight;
        Timer rulesTimer;


        Rectangle dropDownRect;
        Rectangle dropDownRectE;
        Rectangle dropDownRectM;
        Rectangle dropDownRectH;
        Rectangle easyButRect;
        Rectangle mediumButRect;
        Rectangle hardButRect;
        Rectangle checkRect;
        Rectangle watchRect;
        Rectangle exitRect;
        Rectangle soundRect;


        Texture2D hudBar;
        Rectangle hudRect;
        Texture2D[] mines = new Texture2D[8];
        Texture2D[] mineNums = new Texture2D[8];

        int difficulty;
        int gameState;

        Timer gameTimer;
        const int TARGETTIME = 10000;
        Vector2 timerTextLoc;

        double winTime = 0;
        string winTimeString = "";
        double bestWinTime = 0;
        int result = NONE;

        const int WIN = 0;
        const int LOSE = 1;
        const int NONE = 2;

        const int EASY = 0;
        const int MEDIUM = 1;
        const int HARD = 2;

        const int GAMEPLAY = 0;
        const int TRYAGAIN = 1;
        const int EXIT = 2;

        Texture2D gameOverNoTimeImg;
        Texture2D gameOverPlayAgainImg;
        Texture2D gameOverResultsImg;
        Texture2D gameOverTryAgainImg;
        Texture2D gameOverWinResultsImg;
        Texture2D gameOverShadowImg;
        bool drawEndGame;

        Rectangle gameOverNoTimeRect;
        Rectangle gameOverPlayAgainRect;
        Rectangle gameOverResultsRect;
        Rectangle gameOverTryAgainRect;
        Rectangle gameOverWinResultsRect;
        Rectangle gameOverShadowRect;

        SoundEffect loseSnd;
        SoundEffect winSnd;
        SoundEffect clearFlagSnd;
        SoundEffect placeFlagSnd;
        SoundEffect largeClearSnd;
        SoundEffect smallClearSnd;
        SoundEffect mineSnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            difficulty = EASY;
            gameState = GAMEPLAY;
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 450;
            graphics.PreferredBackBufferHeight = 405;
            graphics.ApplyChanges();
    
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            IsMouseVisible = true;

            ReadBestTime();
           
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
            text = Content.Load<SpriteFont>("Fonts/TextFont");

            bgEasy = Content.Load<Texture2D>("Images/Background/board_easy");
            bgEasyRect = new Rectangle(0, size, bgEasy.Width, bgEasy.Height);

            bgMed = Content.Load<Texture2D>("Images/Background/board_med");
            bgMedRect = new Rectangle(0, size, bgMed.Width, bgMed.Height);

            bgHard = Content.Load<Texture2D>("Images/Background/board_hard");
            bgHardRect = new Rectangle(0, size, bgHard.Width, bgHard.Height);

            hudBar = Content.Load<Texture2D>("Images/Sprites/HUDBar");
            hudRect = new Rectangle(0, 0, screenWidth, size);

            tileDark = Content.Load<Texture2D>("Images/Sprites/Clear_Dark");
            tileLight = Content.Load<Texture2D>("Images/Sprites/Clear_Light");

            dropDownImg = Content.Load<Texture2D>("Images/Sprites/DropDown");
            easyButImg = Content.Load<Texture2D>("Images/Sprites/EasyButton");
            mediumButImg = Content.Load<Texture2D>("Images/Sprites/MedButton");
            hardButImg = Content.Load<Texture2D>("Images/Sprites/HardButton");
            checkImg = Content.Load<Texture2D>("Images/Sprites/Check");

            dropDownRect = new Rectangle(30, 6 + easyButImg.Height, dropDownImg.Width, dropDownImg.Height);
            dropDownRectE = new Rectangle(30, 6 + easyButImg.Height, dropDownImg.Width, dropDownImg.Height/3);
            dropDownRectM = new Rectangle(30, 6 + easyButImg.Height + (dropDownImg.Height / 3), dropDownImg.Width , dropDownImg.Height / 3);
            dropDownRectH = new Rectangle(30, 6 + easyButImg.Height + ((dropDownImg.Height / 3)*2), dropDownImg.Width , dropDownImg.Height / 3);

            easyButRect = new Rectangle(30, 6, easyButImg.Width, easyButImg.Height);
            mediumButRect = new Rectangle(30, 6, mediumButImg.Width, mediumButImg.Height);
            hardButRect = new Rectangle(30, 6, hardButImg.Width, mediumButImg.Height);

            flagImg = Content.Load<Texture2D>("Images/Sprites/flag");
            flagRect = new Rectangle(screenWidth / 2 - (size + size/2), 8, flagImg.Width / 4,flagImg.Height / 4);

            watchImg = Content.Load<Texture2D>("Images/Sprites/Watch");
            watchRect = new Rectangle(screenWidth / 2 , 8, flagImg.Width / 4, flagImg.Height / 4);

            gameTimer = new Timer(TARGETTIME, false);
            timerTextLoc = new Vector2(screenWidth / 2 + (size / 2 + 10), 10);

            soundOnImg = Content.Load<Texture2D>("Images/Sprites/SoundOn");
            soundOffImg = Content.Load<Texture2D>("Images/Sprites/SoundOff");
            soundRect = new Rectangle(screenWidth- size*2, 12, soundOnImg.Width/2, soundOnImg.Height/2);

            rulesLeftImg = Content.Load<Texture2D>("Images/Sprites/Instructions Left");
            rulesRightImg = Content.Load<Texture2D>("Images/Sprites/Instructions Right");
            rulesRectLeft = new Rectangle(screenWidth /4,screenHeight / 4, rulesLeftImg.Width/2, rulesLeftImg.Height/2);
            rulesRectRight = new Rectangle(screenWidth / 4, screenHeight / 4, rulesLeftImg.Width / 2, rulesLeftImg.Height / 2);
            rulesTimer = new Timer(1000,true);

            exitImg = Content.Load<Texture2D>("Images/Sprites/Exit");
            exitRect = new Rectangle(screenWidth - size, size/2-7, exitImg.Width/8, exitImg.Height/8);

            gameOverNoTimeImg = Content.Load<Texture2D>("Images/Sprites/GameOver_NoTime");
            gameOverPlayAgainImg = Content.Load<Texture2D>("Images/Sprites/GameOver_PlayAgain");
            gameOverResultsImg = Content.Load<Texture2D>("Images/Sprites/GameOver_Results");
            gameOverWinResultsImg = Content.Load<Texture2D>("Images/Sprites/GameOver_WinResults");
            gameOverTryAgainImg = Content.Load<Texture2D>("Images/Sprites/GameOver_TryAgain");
            gameOverShadowImg = Content.Load<Texture2D>("Images/Sprites/GameOverBoardShadow");

            gameOverResultsRect = new Rectangle((screenWidth / 5)-20, size, gameOverResultsImg.Width, gameOverResultsImg.Height);
            gameOverNoTimeRect = new Rectangle(screenWidth/4, screenHeight/2-size, gameOverNoTimeImg.Width*9-size*2-40, gameOverNoTimeImg.Height);
            gameOverTryAgainRect = new Rectangle((screenWidth / 5) - 20, size+ gameOverResultsImg.Height, gameOverResultsImg.Width, gameOverTryAgainImg.Height);
            gameOverWinResultsRect = new Rectangle((screenWidth / 5) - 20, size, gameOverWinResultsImg.Width, gameOverWinResultsImg.Height);
            gameOverPlayAgainRect = new Rectangle((screenWidth / 5) - 20, size+ gameOverWinResultsImg.Height, gameOverWinResultsImg.Width, gameOverPlayAgainImg.Height);
            gameOverShadowRect = new Rectangle(0, 0, screenWidth, screenHeight);

            loseSnd = Content.Load<SoundEffect>("Audio/Music/Lose");
            winSnd = Content.Load<SoundEffect>("Audio/Music/Win");
            largeClearSnd = Content.Load<SoundEffect>("Audio/Sounds/LargeClear");
            smallClearSnd = Content.Load<SoundEffect>("Audio/Sounds/SmallClear");
            mineSnd = Content.Load<SoundEffect>("Audio/Sounds/Mine");
            clearFlagSnd = Content.Load<SoundEffect>("Audio/Sounds/ClearFlag");
            placeFlagSnd = Content.Load<SoundEffect>("Audio/Sounds/PlaceFlag");
            SoundEffect.MasterVolume = 0.75f;

            for (int i = 0; i < mines.Length; i++)
            {
                mines[i] = Content.Load<Texture2D>("Images/Sprites/Mine" + (i + 1).ToString());
            }

            for (int i = 0; i < mineNums.Length; i++)
            {
                mineNums[i] = Content.Load<Texture2D>("Images/Sprites/" + (i + 1).ToString());
            }

            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                   
                    tiles[r, c] = new Tile(locx, locy, size, false, false);
                    locx += size;

                }

                locx = 0;
                locy += size;
            }

            for (int i = 0; i < mineCount; i++)
            {
                int num1 = rnd.Next(row);
                int num2 = rnd.Next(col);
                tiles[num1, num2].SetMine(true);
            }

            CountTouchedMines();
            DrawMine();
            


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            gameTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
            rulesTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (rulesTimer.GetTimeRemaining() == 0)
            {
                int num = rnd.Next(0, 2);

                if (num == 0)
                {
                    rulesLeft = true;
                }
                else
                {
                    rulesRight = true;
                }
                
            }
           

            prevmb = mouse;
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && !(prevmb.LeftButton == ButtonState.Pressed)
                &&  dropDown != true)
            {
                if (result == NONE)
                {
                    gameTimer.Activate();
                    for (int r = 0; r < tiles.GetLength(0); r++)
                    {
                        for (int c = 0; c < tiles.GetLength(1); c++)
                        {
                            if (tiles[r, c].GetRect().Contains(mouse.X, mouse.Y) && tiles[r, c].HasMine() == true)
                            {
                                UpdateAllMines();
                                result = LOSE;
                                if (sound == true)
                                {
                                    loseSnd.CreateInstance().Play();
                                }
                                gameTimer.Deactivate();
                                winTime = gameTimer.GetTimePassed();
                                winTimeString = (gameTimer.GetTimePassedAsString((int)Timer.INFINITE_TIMER)).PadLeft(3, '0');
                                WriteBestTime();
                                drawEndGame = true;
                            }

                            else if (tiles[r, c].GetRect().Contains(mouse.X, mouse.Y))
                            {
                                ClearZeros(r, c);
                            }
                        }
                    }

                    if (exitRect.Contains(mouse.X, mouse.Y))
                    {
                        gameState = EXIT;
                    }

                    if (soundRect.Contains(mouse.X, mouse.Y))
                    {
                        if (sound == true)
                        {
                            sound = false;
                        }
                        else
                        {
                            sound = true;
                        }
                    }

                    if (flagCount == 0)
                    {
                        for (int r = 0; r < tiles.GetLength(0); r++)
                        {
                            for (int c = 0; c < tiles.GetLength(1); c++)
                            {
                                if (tiles[r, c].HasMine() && tiles[r, c].HasFlag())
                                {
                                    correctFlag++;
                                    tiles[r, c].HitMine(false);
                                }
                            }
                        }

                        if (correctFlag == startFlagCount)
                        {
                            result = WIN;
                            if (sound == true)
                            {
                                winSnd.CreateInstance().Play();
                            }
                            gameTimer.Deactivate();
                            winTime = gameTimer.GetTimePassed();
                            winTimeString = gameTimer.GetTimePassedAsString((int)Timer.INFINITE_TIMER).PadLeft(3, '0');
                            WriteBestTime();
                        }
                        else
                        {
                            result = LOSE;
                            if (sound == true)
                            {
                                loseSnd.CreateInstance().Play();
                            }
                            gameTimer.Deactivate();
                            winTime = gameTimer.GetTimePassed();
                            winTimeString = gameTimer.GetTimePassedAsString((int)Timer.INFINITE_TIMER).PadLeft(3, '0');
                            WriteBestTime();
                        }


                    }

                }
                if ((result == LOSE && gameOverTryAgainRect.Contains(mouse.X, mouse.Y)) ||
                        (result == WIN && gameOverPlayAgainRect.Contains(mouse.X, mouse.Y)))
                {
                    result = NONE;
                    ResetGame();

                }
            }

            if (mouse.RightButton == ButtonState.Pressed && !(prevmb.RightButton == ButtonState.Pressed) && dropDown != true
                && result == NONE)
            {
                for (int r = 0; r < tiles.GetLength(0); r++)
                {
                    for (int c = 0; c < tiles.GetLength(1); c++)
                    {
                        if (tiles[r, c].GetRect().Contains(mouse.X, mouse.Y) && tiles[r,c].GetState() != true)
                        {
                            if (tiles[r, c].HasFlag())
                            {
                                tiles[r, c].SetFlag(false);
                                if (sound == true)
                                {
                                    clearFlagSnd.CreateInstance().Play();
                                }
                                flagCount++;
                            }
                            else
                            {
                                tiles[r, c].SetFlag(true);
                                if (sound == true)
                                {
                                    placeFlagSnd.CreateInstance().Play();
                                }
                                flagCount--;
                            }
                        }
                    }
                }
            }

            UpdateDropDown();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch(gameState)
            {
                case GAMEPLAY:
                    switch (difficulty)
                    {
                        case EASY:
                            spriteBatch.Draw(bgEasy, bgEasyRect, Color.White);
                            GamePlay(spriteBatch);
                            break;
                        case MEDIUM:
                            spriteBatch.Draw(bgMed, bgMedRect, Color.White);
                            GamePlay(spriteBatch);
                            break;
                        case HARD:
                            spriteBatch.Draw(bgHard, bgHardRect, Color.White);
                            GamePlay(spriteBatch);
                            break;
                    }
                    break;
                case EXIT:
                    Exit();
                    break;
            }

    
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void ResetGame ()
        {
            switch (difficulty)
            {
                case EASY:
                    graphics.PreferredBackBufferWidth = 450;
                    graphics.PreferredBackBufferHeight = 405; 
                    graphics.ApplyChanges();

                    screenWidth = graphics.GraphicsDevice.Viewport.Width;
                    screenHeight = graphics.GraphicsDevice.Viewport.Height;

                    LoadContent();

                    row = 8;
                    col = 10;
                    tiles = new Tile[row, col];
                    size = 45;
                    locx = 0;
                    locy = size;
                    startFlagCount = 10;
                    flagCount = startFlagCount;
                    mineCount = 10;

                    break;

                case MEDIUM:
                    graphics.PreferredBackBufferWidth = 540;
                    graphics.PreferredBackBufferHeight = 450; 
                    graphics.ApplyChanges();

                    screenWidth = graphics.GraphicsDevice.Viewport.Width;
                    screenHeight = graphics.GraphicsDevice.Viewport.Height;
                    LoadContent();

                    row = 14;
                    col = 18;
                    tiles = new Tile[row, col];
                    size = 30;
                    locx = 0;
                    locy = size;
                    startFlagCount = 40;
                    flagCount = startFlagCount;
                    mineCount = 40;

                    break;

                case HARD:
                    graphics.PreferredBackBufferWidth = 600;
                    graphics.PreferredBackBufferHeight = 525; 
                    graphics.ApplyChanges();

                    screenWidth = graphics.GraphicsDevice.Viewport.Width;
                    screenHeight = graphics.GraphicsDevice.Viewport.Height;
                    LoadContent();

                    row = 20;
                    col = 24;
                    tiles = new Tile[row, col];
                    size = 25;
                    locx = 0;
                    locy = size;
                    startFlagCount = 99;
                    flagCount = startFlagCount;
                    mineCount = 99;

                    break;
            }

            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {

                    tiles[r, c] = new Tile(locx, locy, size, false, false);
                    locx += size;

                }

                locx = 0;
                locy += size;
            }

            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    tiles[r, c].SetFlag(false);
                    tiles[r, c].HitMine(false);
                }
            }

            for (int i = 0; i < mineCount; i++)
            {
                int num1 = rnd.Next(row);
                int num2 = rnd.Next(col);
                tiles[num1, num2].SetMine(true);
            }

            CountTouchedMines();
            DrawMine();
        }

        public void UpdateDropDown ()
        {

            if ((mouse.LeftButton == ButtonState.Pressed && !(prevmb.LeftButton == ButtonState.Pressed)) ||
              (mouse.RightButton == ButtonState.Pressed && !(prevmb.RightButton == ButtonState.Pressed)))
            {
                rulesTimer.Deactivate();
                rulesLeft = false;
                rulesRight = false;

                if (easyButRect.Contains(mouse.X, mouse.Y))
                {
                    gameTimer.ResetTimer(false);
                    ResetGame();
                    CloseDropDown();
                    

                }
                else if (mediumButRect.Contains(mouse.X, mouse.Y))
                {
                    difficulty = MEDIUM;
                    gameTimer.ResetTimer(false);
                    ResetGame();
                    CloseDropDown();
                    

                }
                else if (hardButRect.Contains(mouse.X, mouse.Y))
                {
                    difficulty = HARD;
                    gameTimer.ResetTimer(false);
                    ResetGame();
                    CloseDropDown();
                    
                }
                DropDown();

            }
        }
        public void CloseDropDown()
        {
            if (dropDown == true)
            {
                dropDown = false;
            }
            else
            {
                dropDown = true;
            }
        }

        public void DropDown ()
        {
            if (dropDownRectE.Contains(mouse.X, mouse.Y) && dropDown == true)
            { 
                difficulty = EASY;
                checkRect = new Rectangle(35, 14 + easyButImg.Height, checkImg.Width, checkImg.Height);
                CloseDropDown();
                
            }
            else if (dropDownRectM.Contains(mouse.X, mouse.Y) && dropDown == true)
            {
                difficulty = MEDIUM;
                checkRect = new Rectangle(35, 36 + easyButImg.Height, checkImg.Width, checkImg.Height);
                CloseDropDown();
            }
            else if (dropDownRectH.Contains(mouse.X, mouse.Y) && dropDown == true)
            {
                difficulty = HARD;
                checkRect = new Rectangle(35, 60 + easyButImg.Height, checkImg.Width, checkImg.Height);
                CloseDropDown();
            }
        }

        public void EndGame (SpriteBatch spriteBatch)
        {
            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    tiles[r, c].SetState(false);
                }
            }

            spriteBatch.Draw(gameOverShadowImg, gameOverShadowRect, Color.White*0.10f);
            spriteBatch.Draw(exitImg, exitRect, Color.White);

            if (result == LOSE)
            {
                spriteBatch.Draw(gameOverResultsImg, gameOverResultsRect, Color.White);
                spriteBatch.Draw(gameOverNoTimeImg, gameOverNoTimeRect, Color.White);
                spriteBatch.Draw(gameOverTryAgainImg, gameOverTryAgainRect, Color.White);
            }
            else if (result == WIN)
            {
                spriteBatch.Draw(gameOverWinResultsImg, gameOverWinResultsRect, Color.White);
                spriteBatch.DrawString(text,winTimeString, new Vector2 ((screenWidth / 5) - 20 + size + size/2, gameOverWinResultsImg.Height/2 + 15), Color.White);
                spriteBatch.DrawString(text, bestWinTime.ToString().PadLeft(3, '0'), new Vector2((screenWidth / 5) - 20 + size*4 + size/2, gameOverWinResultsImg.Height / 2 + 15), Color.White);
                spriteBatch.Draw(gameOverPlayAgainImg, gameOverPlayAgainRect, Color.White);
            }

        }

        public void WriteBestTime ()
        {
            try
            {
                outFile = File.CreateText("BestTime.txt");

                if (winTime > bestWinTime)
                {
                    bestWinTime = (int) winTime;
                    
                }
                outFile.WriteLine((int) bestWinTime);

            }
            catch (Exception e)
            {

            }
            finally
            {
                if (outFile != null)
                {
                    outFile.Close();
                }
            }
        }

        public void ReadBestTime ()
        {
            try
            {
                inFile = File.OpenText("BestTime.txt");
                bestWinTime = Double.Parse(inFile.ReadLine());
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (inFile != null)
                {
                    inFile.Close();
                }
            }


        }
        public void GamePlay(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(hudBar, hudRect, Color.White);
            spriteBatch.Draw(flagImg, flagRect, Color.White);
            spriteBatch.Draw(watchImg, watchRect, Color.White);

            if (rulesRight == true)
            {
                spriteBatch.Draw(rulesRightImg, rulesRectRight, Color.White);
                rulesTimer.ResetTimer(true);
            }
            else if (rulesLeft == true)
            {
                spriteBatch.Draw(rulesLeftImg, rulesRectLeft, Color.White);
                rulesTimer.ResetTimer(true);
            }
            

            if (sound == true)
            {
                spriteBatch.Draw(soundOnImg, soundRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(soundOffImg, soundRect, Color.White);
            }


            spriteBatch.Draw(exitImg, exitRect, Color.White);
            spriteBatch.DrawString(text, flagCount.ToString(), new Vector2(screenWidth / 2 - (size - 10), 10), Color.White);
            spriteBatch.DrawString(text, (gameTimer.GetTimePassedAsString((int)Timer.INFINITE_TIMER)).PadLeft(3, '0'), timerTextLoc, Color.White);
            switch (difficulty)
            {
                case MEDIUM:
                    spriteBatch.Draw(mediumButImg, mediumButRect, Color.White);
                    if (dropDown == true)
                    {
                        spriteBatch.Draw(dropDownImg, dropDownRect, Color.White);
                        spriteBatch.Draw(checkImg, checkRect, Color.White);
                    }
                    break;
                case HARD:
                    spriteBatch.Draw(hardButImg, hardButRect, Color.White);
                    if (dropDown == true)
                    {
                        spriteBatch.Draw(dropDownImg, dropDownRect, Color.White);
                        spriteBatch.Draw(checkImg, checkRect, Color.White);
                    }
                    break;
                 default:
                    spriteBatch.Draw(easyButImg, easyButRect, Color.White);
                    if (dropDown == true)
                    {
                        spriteBatch.Draw(dropDownImg, dropDownRect, Color.White);
                        spriteBatch.Draw(checkImg, checkRect, Color.White);
                    }
                    break;
            }


            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    if (tiles[r, c].GetState() == true && tiles[r, c] != null && tiles[r, c].HasMine() != true
                        && dropDown != true)
                    {
                        if (r % 2 == 0)
                        {
                            if (c % 2 == 0)
                            {
                                tiles[r, c].DrawTile(spriteBatch, tileDark);
                                if (tiles[r, c].GetAdjMineNum() != 0)
                                {
                                    tiles[r, c].DrawTile(spriteBatch, mineNums[tiles[r, c].GetAdjMineNum() - 1]);
                                }
                            }
                            else
                            {
                                tiles[r, c].DrawTile(spriteBatch, tileLight);
                                if (tiles[r, c].GetAdjMineNum() != 0)
                                {
                                    tiles[r, c].DrawTile(spriteBatch, mineNums[tiles[r, c].GetAdjMineNum() - 1]);
                                }
                            }
                        }
                        else
                        {
                            if (c % 2 == 0)
                            {
                                tiles[r, c].DrawTile(spriteBatch, tileLight);
                                if (tiles[r, c].GetAdjMineNum() != 0)
                                {
                                    tiles[r, c].DrawTile(spriteBatch, mineNums[tiles[r, c].GetAdjMineNum() - 1]);
                                }
                            }
                            else
                            {
                                tiles[r, c].DrawTile(spriteBatch, tileDark);
                                if (tiles[r, c].GetAdjMineNum() != 0)
                                {
                                    tiles[r, c].DrawTile(spriteBatch, mineNums[tiles[r, c].GetAdjMineNum() - 1]);
                                }
                            }
                        }

                    }

                    if (tiles[r, c].IfHitMine())
                    {
                       tiles[r, c].DrawTile(spriteBatch, tiles[r, c].GetMineImg());
                       if (drawEndGame == true)
                       {
                            EndGame(spriteBatch);
                        } 
                    }


                    if (tiles[r, c].HasFlag() && tiles[r, c].GetState() != true)
                    {
                        tiles[r, c].DrawTile(spriteBatch, flagImg);
                    }

                    if (result == WIN || result == LOSE)
                    {
                        EndGame(spriteBatch);
                    }
                }
            }
        }

        public void UpdateAllMines ()
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col].HasMine())
                    {
                        tiles[row, col].HitMine(true);
                    }

                }
            }

        }
        public void CountTouchedMines()
        {
            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    if (!(r == 0) && !(r == row - 1) && !(c == 0) && !(c == col - 1))
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (i != 0 || j != 0)
                                {
                                    if (tiles[r + i, c + j].HasMine())
                                    {
                                        hasMineCount++;
                                    }
                                }
                                
                            }
                        }
                    }
                    else if (r == 0 && (c != 0 && c != col - 1))
                    {
                        if (tiles[r, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (r == row - 1 && (c != 0 && c != col - 1))
                    {
                        if (tiles[r, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (c == 0 && (r != 0 && r != row - 1))
                    {
                        if (tiles[r, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (c == col - 1 && (r != 0 && r != row - 1))
                    {
                        if (tiles[r, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (r == 0 && c == 0)
                    {
                        if (tiles[r + 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (r == row - 1 && c == 0)
                    {
                        if (tiles[r - 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c + 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else if (r == 0 && c == col - 1)
                    {
                        if (tiles[r + 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r + 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }
                    else
                    {
                        if (tiles[r - 1, c].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                        if (tiles[r - 1, c - 1].HasMine())
                        {
                            hasMineCount++;
                        }
                    }

                    tiles[r, c].SetMineNum(hasMineCount);
                    hasMineCount = 0;
                }

            }
        }

        public void DrawMine ()
        {
            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    num = rnd.Next(mines.Length);

                    if (tiles[r,c].HasMine())

                    switch (num)
                    {
                        case 1:
                            tiles[r,c].SetMineImg(mines[1]);
                            break;
                        case 2:
                            tiles[r, c].SetMineImg(mines[2]);
                            break;
                        case 3:
                            tiles[r, c].SetMineImg(mines[3]);
                            break;
                        case 4:
                            tiles[r, c].SetMineImg(mines[4]);
                            break;
                        case 5:
                            tiles[r, c].SetMineImg(mines[5]);
                            break;
                        case 6:
                            tiles[r, c].SetMineImg(mines[6]);
                            break;
                        case 7:
                            tiles[r, c].SetMineImg(mines[7]);
                            break;
                        default:
                            tiles[r, c].SetMineImg(mines[0]);
                            break; 
                    }
                }
            }
        }

        public void ClearZeros(int r, int c)
        {
            if (tiles[r,c].GetAdjMineNum() > 0)
            {
                tiles[r, c].SetState(true);
                if (sound == true)
                {
                    smallClearSnd.CreateInstance().Play();
                }
            }
            else if (tiles[r, c].GetAdjMineNum() == 0 )
            {
                
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if ((r+i >= 0 && r+i <= row-1) && (c+j >= 0 && c+j <= col-1))
                        {
                            if (tiles[r + i, c + j].GetState() != true)
                            {
                                tiles[r + i, c + j].SetState(true);

                                if (sound == true)
                                {
                                    largeClearSnd.CreateInstance().Play();
                                }
                                
                                ClearZeros(r + i, c + j);
                            }
                        }
                        else
                        {
                            tiles[r, c].SetState(true);
                            if (sound == true)
                            {
                                smallClearSnd.CreateInstance().Play();
                            }
                            continue;
                        }
                            
                    }

                }
            }
                
        }

    }

}
    

