using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Source
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PortableGame
    {
        Game _game;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        GameSprite _background, _enemy, _hero;
        RenderContext _renderContext;

        public PortableGame(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize(GraphicsDeviceManager graphics)
        {
            // TODO: Add your initialization logic here
            _renderContext = new RenderContext();
            _graphics = graphics;

            _background = new GameSprite("Game2D/Background");
            _enemy = new GameSprite("Game2D/Enemy");
            _hero = new GameSprite("Game2D/Hero");

            _enemy.Position = new Vector2(10, 10);
            _hero.Position = new Vector2(10, 348);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            // TODO: use this.Content to load your game content here         
            _renderContext.SpriteBatch = _spriteBatch;
            _renderContext.GraphicsDevice = _graphics.GraphicsDevice;

            _background.LoadContent(Content);
            _enemy.LoadContent(Content);
            _hero.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                _game.Exit();

            // TODO: Add your update logic here
            _renderContext.GameTime = gameTime;
            _enemy.Update(_renderContext);
            _hero.Update(_renderContext);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _background.Draw(_renderContext);
            _enemy.Draw(_renderContext);
            _hero.Draw(_renderContext);
            _spriteBatch.End();
        }
    }
}
