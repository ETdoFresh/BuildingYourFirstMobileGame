using BuildingYourFirstMobileGame.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.SceneGraph
{
    static class SceneManager
    {
        public static Microsoft.Xna.Framework.Game MainGame { get; set; } 
        public static List<GameScene> GameScenes { get; private set; }
        public static GameScene ActiveScene { get; private set; }
        public static RenderContext RenderContext { get; private set; }

        private static GameScene _newActiveScene;

        static SceneManager()
        {
            GameScenes = new List<GameScene>();
            RenderContext = new RenderContext();
            //Default Camera
            RenderContext.Camera = new BaseCamera();
        }

        public static void AddGameScene(GameScene gameScene)
        {
            if (!GameScenes.Contains(gameScene))
                GameScenes.Add(gameScene);
        }

        public static void RemoveGameScene(GameScene gameScene)
        {
            GameScenes.Remove(gameScene);

            if (ActiveScene == gameScene) ActiveScene = null;
        }

        public static bool SetActiveScene(string name)
        {
            _newActiveScene = GameScenes.FirstOrDefault(scene => scene.SceneName.Equals(name));
            return _newActiveScene != null;
        }

        public static void Initialize()
        {
            GameScenes.ForEach(scene => scene.Initialize());
        }

        public static void LoadContent(ContentManager contentManager)
        {
            GameScenes.ForEach(scene => scene.LoadContent(contentManager));
        }

        public static void Update(GameTime gameTime)
        {
            if (_newActiveScene != null)
            {
                if (ActiveScene != null) ActiveScene.Deactivated();
                ActiveScene = _newActiveScene;
                ActiveScene.Activated();
                _newActiveScene = null;
            }

            if (ActiveScene != null)
            {
                RenderContext.GameTime = gameTime;

                ActiveScene.Update(RenderContext);
            }

            RenderContext.TouchPanelState = TouchPanel.GetState();
            InputHelper.Update(RenderContext);
        }

        public static void Draw()
        {
            if (ActiveScene != null)
            {
                //2D Before 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, false);
                RenderContext.SpriteBatch.End();

                //DRAW 3D
                //Reset Renderstate
                RenderContext.GraphicsDevice.BlendState = BlendState.Opaque;
                RenderContext.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                RenderContext.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                ActiveScene.Draw3D(RenderContext);

                //2D After 3D
                RenderContext.SpriteBatch.Begin();
                ActiveScene.Draw2D(RenderContext, true);
                RenderContext.SpriteBatch.End();
            }
        }
    }
}
