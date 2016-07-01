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

        private readonly Score score;

        private readonly Texture2D[] textureTiles;

        private Rectangle clickableArea = new Rectangle(240, 40, 525, 525);

        private Point fistClickedTileCoordinates;

        private Texture2D grid;

        private Texture2D hintButton;

        private bool isFirstClick;

        private MouseState mouseState;

        private BejeweledPresenter presenter;

        private MouseState prevMouseState = Mouse.GetState();

        private SpriteFont scoreFont;

        private SpriteBatch spriteBatch;

        public BejeweledView()
        {
            this.textureTiles = new Texture2D[8];
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 800;
            this.Content.RootDirectory = "Content";
            this.score = new Score();
            this.assetManager = new AssetManager(this.Content);
        }

        public event EventHandler OnLoad;

        public event EventHandler<TileEventArgs> OnTileClicked;

        public int[,] Tiles { get; set; }

        public void DetectGameBoardClick()
        {
            if (this.mouseState.LeftButton == ButtonState.Pressed
                && this.prevMouseState.LeftButton == ButtonState.Released)
            {
                // We now know the left mouse button is down and it wasn't down last frame
                // so we've detected a click
                // Now find the position 
                var mousePos = new Point(this.mouseState.X, this.mouseState.Y);
                if (this.clickableArea.Contains(mousePos))
                {
                    var indexY = (int)Math.Floor((double)(this.mouseState.X - 240) / 65);
                    var indexX = (int)Math.Floor((double)(this.mouseState.Y - 40) / 65);
                    if (this.isFirstClick)
                    {
                        this.fistClickedTileCoordinates = new Point(indexX, indexY);
                        this.isFirstClick = false;
                    }
                    else
                    {
                        if (this.OnTileClicked != null)
                        {
                            this.OnTileClicked(
                                this, 
                                new TileEventArgs(
                                    this.Tiles[this.fistClickedTileCoordinates.X, this.fistClickedTileCoordinates.Y],
                                    this.fistClickedTileCoordinates.X, 
                                    this.fistClickedTileCoordinates.Y, 
                                    this.Tiles[indexX, indexY],
                                    indexX, 
                                    indexY));
                        }

                        this.isFirstClick = true;
                    }
                }
            }

            // Store the mouse state so that we can compare it next frame
            // with the then current mouse state
            this.prevMouseState = this.mouseState;
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
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.DrawString(
                this.scoreFont,
                "Score: " + GlobalScore.globalScore,
                new Vector2(30, 120), 
                Color.GreenYellow);
            this.spriteBatch.Draw(this.hintButton, new Vector2(60, 430), null, Color.White);
            this.spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
            this.DrawGameBoard();

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
                    this.spriteBatch.Draw(
                        this.textureTiles[this.Tiles[i, j]],
                        new Vector2(y, x),
                        null,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.5f,
                        SpriteEffects.None,
                        0);
                    y += 65;
                }

                x += 65;
            }
            this.spriteBatch.End();
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
            this.textureTiles[7] = this.Content.Load<Texture2D>(@"emptyTrans");
            this.grid = this.Content.Load<Texture2D>(@"boardFinal");
            this.scoreFont = this.Content.Load<SpriteFont>("scoreFont");
            this.hintButton = this.Content.Load<Texture2D>(@"hintButton");

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

            // TODO: Add your update logic here
            base.Update(gameTime);
        }
    }
}