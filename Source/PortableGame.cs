using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.Game2D;

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

        RenderContext _renderContext;
        Model _hero;
        Matrix _view, _projection;

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

            _view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.Up);
            _projection = Matrix.CreateOrthographic(800, 480, 0.1f, 300);
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

            _hero = Content.Load<Model>("Game3D/Vampire");
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
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            var transforms = new Matrix[_hero.Bones.Count];
            _hero.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _hero.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = _view;
                    effect.Projection = _projection;
                    effect.World = transforms[mesh.ParentBone.Index];
                }

                mesh.Draw();
            }
        }
    }
}
