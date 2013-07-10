using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.Game2D;
using Source.Game3D;

namespace Source
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PortableGame
    {
        Game _game;
        ContentManager _content;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        RenderContext _renderContext;
        GameModel _hero;
        Camera _camera;

        GameSprite _background;

        private ContentManager Content { get { return _content; } }

        public PortableGame(Game game, ContentManager content, GraphicsDeviceManager graphics)
        {
            _game = game;
            _content = content;
            _graphics = graphics;
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
            _renderContext = new RenderContext();

            _camera = new Camera();
            _camera.Position = new Vector3(0, 0, 20);
            _renderContext.Camera = _camera;

            _hero = new GameModel("Game3D/Vampire");
            _hero.Position = new Vector3(0, -147, -100);

            _background = new GameSprite("Game2D/Background");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(SpriteBatch spriteBatch)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = spriteBatch;

            // TODO: use this.Content to load your game content here         
            _renderContext.SpriteBatch = _spriteBatch;
            _renderContext.GraphicsDevice = _graphics.GraphicsDevice;

            _hero.LoadContent(Content);
            _background.LoadContent(Content);
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
            if (Mouse.GetState().RightButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            // TODO: Add your update logic here
            _renderContext.GameTime = gameTime;
            _camera.Update(_renderContext);
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
            _spriteBatch.End();

            //_graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            //_graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //_graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            _hero.Draw(_renderContext);
        }
    }
}
