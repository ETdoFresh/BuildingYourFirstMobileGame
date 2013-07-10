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
        SpriteBatch _spriteBatch;

        Texture2D _background, _enemy, _hero;

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
        public void Initialize()
        {
            // TODO: Add your initialization logic here
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
            _background = Content.Load<Texture2D>("Game2D/Background");
            _enemy = Content.Load<Texture2D>("Game2D/Enemy");
            _hero = Content.Load<Texture2D>("Game2D/Hero");
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
            _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_enemy, new Vector2(10, 10), Color.White);
            _spriteBatch.Draw(_hero, new Vector2(10, 348), Color.White);
            _spriteBatch.End();
        }
    }
}
