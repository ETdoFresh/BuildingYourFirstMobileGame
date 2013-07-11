using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Source.Engine;
using Source.Engine.Helper;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using Source.Game.Scenes;
using System;

namespace Source
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PortableGame
    {
        Microsoft.Xna.Framework.Game _game;
        ContentManager _content;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private ContentManager Content { get { return _content; } }

        public PortableGame(Microsoft.Xna.Framework.Game game, ContentManager content, GraphicsDeviceManager graphics)
        {
            _game = game;
            _content = content;
            _graphics = graphics;
            SceneManager.MainGame = this;
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
            SceneManager.RenderContext.GraphicsDevice = _graphics.GraphicsDevice;

            SceneManager.AddGameScene(new MenuScene());
            SceneManager.AddGameScene(new LevelScene());

            SceneManager.SetActiveScene("Menu");
            SceneManager.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(SpriteBatch spriteBatch)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = spriteBatch;
            SceneManager.RenderContext.SpriteBatch = spriteBatch;

            // TODO: use this.Content to load your game content here
            SceneManager.LoadContent(Content);
            Extensions.LoadContent(Content);
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
            if (InputHelper.isKeyPressed(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputHelper.Update(gameTime);
            SceneManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SceneManager.Draw();
        }

        public void Exit()
        {
            if (SceneManager.ActiveScene.SceneName == "Menu") _game.Exit();
            else SceneManager.SetActiveScene("Menu");
        }

        public event Action OnResetRenderState = delegate { };

        public void ResetRenderState()
        {
            OnResetRenderState();
        }
    }
}
