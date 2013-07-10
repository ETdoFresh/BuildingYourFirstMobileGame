#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using BuildingYourFirstMobileGame.Game.Game3D;
using BuildingYourFirstMobileGame.Engine.Helpers;
using BuildingYourFirstMobileGame.Engine.Objects;
using BuildingYourFirstMobileGame.Game.Game2D;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using BuildingYourFirstMobileGame.Game.Scenes;
#endregion

namespace BuildingYourFirstMobileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
            : base()
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
            // TODO: Add your initialization logic here
            SceneManager.RenderContext.GraphicsDevice = graphics.GraphicsDevice;

            SceneManager.AddGameScene(new Game2D());
            SceneManager.AddGameScene(new Game3D());
            SceneManager.AddGameScene(new GameCollision2D());
            SceneManager.AddGameScene(new GameCollision3D());
            SceneManager.AddGameScene(new MenuScene());

            SceneManager.SetActiveScene("Menu");
            SceneManager.Initialize();

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
            SceneManager.RenderContext.SpriteBatch = spriteBatch;

            // TODO: use this.Content to load your game content here
            SceneManager.LoadContent(Content);
            Extensions.LoadContent(Content);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHelper.isKeyPressed(Keys.Escape) || InputHelper.IsMousePressed(InputHelper.MouseButton.Right))
            {
                if (SceneManager.ActiveScene.SceneName == "Menu") Exit();
                else SceneManager.SetActiveScene("Menu");
            }

            // TODO: Add your update logic here
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SceneManager.Draw();

            base.Draw(gameTime);
        }
    }
}
