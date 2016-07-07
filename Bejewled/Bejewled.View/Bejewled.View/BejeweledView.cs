namespace Bejewled.View
{
    using System;
    using System.Windows.Forms;

    using Bejewled.Model;
    using Bejewled.Model.EventArgs;
    using Bejewled.Model.Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BejeweledView : Game, IView
    {
        private readonly AssetManager assetManager;

        private readonly GraphicsDeviceManager graphics;

        private readonly Texture2D[] textureTiles;

        private Rectangle clickableArea = new Rectangle(240, 40, 525, 525);

        private int counter;

        private float elapsed;

        private Point fistClickedTileCoordinates;

        private int frameH;

        private int frameV;

        private Texture2D grid;

        private Texture2D hintButton;

        private Rectangle hintClickableArea = new Rectangle(80, 450, 50, 50);

        private bool isFirstClick;

        private bool isMuted;

        private MouseState mouseState;

        private BejeweledPresenter presenter;

        private MouseState prevMouseState = Mouse.GetState();

        private SpriteFont scoreFont;

        private Texture2D soundButton;

        private Rectangle sourceRectangle;

        private SpriteBatch spriteBatch;

        private Rectangle tileRect;

        public BejeweledView()
        {
            this.textureTiles = new Texture2D[15];
            this.graphics = new GraphicsDeviceManager(this)
                                {
                                    PreferredBackBufferHeight = 600,
                                    PreferredBackBufferWidth = 800
                                };
            this.Content.RootDirectory = "Content";
            this.assetManager = new AssetManager(this.Content);
        }

        public event EventHandler OnLoad;

        public event EventHandler<TileEventArgs> OnSecondTileClicked;

        public event EventHandler OnExplosionFinished;

        public event EventHandler<TileEventArgs> OnFirstTileClicked;

        public event EventHandler OnHintClicked;

        public int[,] Tiles { get; set; }

        public string Score { get; set; }

        public void DisplayGameEndMessage()
        {
            var result = MessageBox.Show("GAME OVER", "You lost this game", MessageBoxButtons.RetryCancel);
            if (result == DialogResult.Retry)
            {
                this.RestartGame();
            }
            if (result == DialogResult.Cancel)
            {
                Environment.Exit(0);
            }
        }

        public void DrawScore()
        {
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.DrawString(this.scoreFont, "Score: " + this.Score, new Vector2(30, 120), Color.GreenYellow);
            this.spriteBatch.End();
        }

        public void DrawGameBoard()
        {
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            float x = 50;
            for (var i = 0; i < this.Tiles.GetLength(0); i++)
            {
                float y = 250;
                for (var j = 0; j < this.Tiles.GetLength(1); j++)
                {
                    if (this.Tiles[i, j] == 7)
                    {
                        this.spriteBatch.Draw(
                            this.textureTiles[this.Tiles[i, j]],
                            new Vector2(y, x),
                            this.sourceRectangle,
                            Color.White,
                            0f,
                            Vector2.Zero,
                            0.5f,
                            SpriteEffects.None,
                            0);
                    }
                    else
                    {
                        this.spriteBatch.Draw(
                            this.textureTiles[this.Tiles[i, j]],
                            new Vector2(y, x),
                            new Rectangle(0, 0, 100, 100),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            0.5f,
                            SpriteEffects.None,
                            0);
                    }

                    y += 65;
                }

                x += 65;
            }
            this.spriteBatch.End();
        }

        private void DetectHintClick()
        {
            // We now know the left mouse button is down and it wasn't down last frame
            // so we've detected a click
            // Now find the position 
            var mousePos = new Point(this.mouseState.X, this.mouseState.Y);

            if (this.OnHintClicked != null)
            {
                this.OnHintClicked(this, EventArgs.Empty);
            }
        }

        private void RestartGame()
        {
            var onOnLoad = this.OnLoad;
            if (onOnLoad != null) onOnLoad(this, EventArgs.Empty);
        }

        public void DetectGameBoardClick()
        {
            if (this.mouseState.LeftButton == ButtonState.Pressed
                && this.prevMouseState.LeftButton == ButtonState.Released)
            {
                // We now know the left mouse button is down and it wasn't down last frame
                // so we've detected a click
                // Now find the position 
                var mousePos = new Point(this.mouseState.X, this.mouseState.Y);
                if (this.hintClickableArea.Contains(mousePos))
                {
                    this.DetectHintClick();
                }
                if (this.clickableArea.Contains(mousePos))
                {
                    var indexY = (int)Math.Floor((double)(this.mouseState.X - 240) / 65);
                    var indexX = (int)Math.Floor((double)(this.mouseState.Y - 40) / 65);
                    if (this.isFirstClick)
                    {
                        this.fistClickedTileCoordinates = new Point(indexX, indexY);
                        this.FirstTileClicked();
                        this.isFirstClick = false;
                    }
                    else
                    {
                        this.SecondTileClicked(indexX, indexY);

                        this.isFirstClick = true;
                    }
                }
            }

            // Store the mouse state so that we can compare it next frame
            // with the then current mouse state
            this.prevMouseState = this.mouseState;
        }

        private void FirstTileClicked()
        {
            if (this.OnFirstTileClicked != null)
            {
                this.OnFirstTileClicked(
                    this,
                    new TileEventArgs(
                        this.Tiles[this.fistClickedTileCoordinates.X, this.fistClickedTileCoordinates.Y],
                        this.fistClickedTileCoordinates.X,
                        this.fistClickedTileCoordinates.Y));
            }
        }

        private void SecondTileClicked(int indexX, int indexY)
        {
            if (this.OnSecondTileClicked != null)
            {
                this.OnSecondTileClicked(
                    this,
                    new TileEventArgs(
                        this.Tiles[this.fistClickedTileCoordinates.X, this.fistClickedTileCoordinates.Y],
                        this.fistClickedTileCoordinates.X,
                        this.fistClickedTileCoordinates.Y,
                        this.Tiles[indexX, indexY],
                        indexX,
                        indexY));
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.grid, Vector2.Zero, Color.White);
            this.spriteBatch.End();
            var scale = 0.5f;
            this.DrawScore();
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.hintButton, new Vector2(60, 430), null, Color.White);
            this.spriteBatch.Draw(this.soundButton, new Vector2(0, 0), null, Color.White);
            this.spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
            this.DrawGameBoard();
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
            this.presenter = new BejeweledPresenter(this, new GameBoard());
            this.tileRect = new Rectangle(0, 0, 100, 100);
            this.IsMouseVisible = true;
            this.fistClickedTileCoordinates = new Point(0, 0);
            this.isFirstClick = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.textureTiles[0] = this.Content.Load<Texture2D>(@"redgemTrans");
            this.textureTiles[1] = this.Content.Load<Texture2D>(@"greengemTrans");
            this.textureTiles[2] = this.Content.Load<Texture2D>(@"bluegemTrans");
            this.textureTiles[3] = this.Content.Load<Texture2D>(@"yellowgemTrans");
            this.textureTiles[4] = this.Content.Load<Texture2D>(@"purplegemTrans");
            this.textureTiles[5] = this.Content.Load<Texture2D>(@"whitegemTrans");
            this.textureTiles[6] = this.Content.Load<Texture2D>(@"rainbowTrans");
            this.textureTiles[7] = this.Content.Load<Texture2D>(@"explosion");
            this.textureTiles[10] = this.Content.Load<Texture2D>(@"bluegemTransClicked");
            this.textureTiles[9] = this.Content.Load<Texture2D>(@"greengemTransClicked");
            this.textureTiles[12] = this.Content.Load<Texture2D>(@"purplegemTransClicked");
            this.textureTiles[14] = this.Content.Load<Texture2D>(@"rainbowTransClicked");
            this.textureTiles[8] = this.Content.Load<Texture2D>(@"redgemTransClicked");
            this.textureTiles[13] = this.Content.Load<Texture2D>(@"whitegemTransClicked");
            this.textureTiles[11] = this.Content.Load<Texture2D>(@"yellowgemTransClicked");
            this.grid = this.Content.Load<Texture2D>(@"boardFinal");
            this.scoreFont = this.Content.Load<SpriteFont>("scoreFont");
            this.hintButton = this.Content.Load<Texture2D>(@"hintButton");
            this.soundButton = this.Content.Load<Texture2D>(@"soundButton");
            if (this.OnLoad != null)
            {
                this.OnLoad(this, EventArgs.Empty);
            }

            this.assetManager.PlayMusic("snd_music");

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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.mouseState = Mouse.GetState();
            this.DetectGameBoardClick();
            this.ExcuteAnimation(gameTime);
            if (this.CheckIfSoundButtonIsPressed())
            {
                /*if (this.isMuted)
                {
                    this.assetManager.PlayMusic("snd_music");
                    this.isMuted = false;
                }
                else
                {*/
                this.isMuted = true;
                this.assetManager.Mute();
                /*}*/
            }
            // TODO: Add your update logic here            
            base.Update(gameTime);
        }

        private bool CheckIfSoundButtonIsPressed()
        {
            var rect = new Rectangle(0, 0, this.soundButton.Width, this.soundButton.Height);
            return this.mouseState.LeftButton == ButtonState.Pressed
                   && rect.Contains(this.mouseState.X, this.mouseState.Y);
        }

        private void ExcuteAnimation(GameTime gameTime)
        {
            this.elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsed >= 28f)
            {
                if (this.counter % 25 == 0)
                {
                    this.frameV = 0;
                    this.frameH = 0;
                    this.counter = 0;
                    this.OnExplosionFinished(this, EventArgs.Empty);
                }
                if (this.frameH >= 12)
                {
                    this.frameH = 0;
                    this.frameV++;
                }
                else
                {
                    this.frameH++;
                }
                this.counter++;
                this.elapsed = 0f;
                this.sourceRectangle = new Rectangle(this.frameH / 4 * 100, this.frameV * 100, 100, 100);
            }
            this.sourceRectangle = new Rectangle(this.frameH / 4 * 100, this.frameV * 100, 100, 100);
        }
    }
}