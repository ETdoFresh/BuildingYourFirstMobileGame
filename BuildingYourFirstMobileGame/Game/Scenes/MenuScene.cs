using BuildingYourFirstMobileGame.Engine.Objects;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Game.Scenes
{
    class MenuScene : GameScene
    {
        public MenuScene() : base("Menu") { }

        public override void Initialize()
        {
            GameButton btnGame2D = new GameButton("Buttons/Scene2DButton", true);
            btnGame2D.Translate(TouchPanel.DisplayWidth / 2f, TouchPanel.DisplayHeight / 2f - 150);
            btnGame2D.PivotPoint = new Vector2(62, 20);
            btnGame2D.Scale(2, 2);
            btnGame2D.OnClick += Game2D_OnClick;
            AddSceneObject(btnGame2D);

            GameButton btnGame3D = new GameButton("Buttons/Scene3DButton", true);
            btnGame3D.Translate(0, 50);
            btnGame3D.PivotPoint = new Vector2(62, 20);
            btnGame3D.OnClick += Game3D_OnClick;
            btnGame2D.AddChild(btnGame3D);

            GameButton btnGameCollision2D = new GameButton("Buttons/Collision2DButton", true);
            btnGameCollision2D.Translate(0, 50);
            btnGameCollision2D.PivotPoint = new Vector2(62, 20);
            btnGameCollision2D.OnClick += GameCollision2D_OnClick;
            btnGame3D.AddChild(btnGameCollision2D);

            GameButton btnGameCollision3D = new GameButton("Buttons/Collision3DButton", true);
            btnGameCollision3D.Translate(0, 50);
            btnGameCollision3D.PivotPoint = new Vector2(62, 20);
            btnGameCollision3D.OnClick += GameCollision3D_OnClick;
            btnGameCollision2D.AddChild(btnGameCollision3D);

            base.Initialize();
        }

        private void Game2D_OnClick()
        {
            SceneManager.SetActiveScene("Game2D");
        }

        private void Game3D_OnClick()
        {
            SceneManager.SetActiveScene("Game3D");
        }

        private void GameCollision2D_OnClick()
        {
            SceneManager.SetActiveScene("GameCollision2D");
        }

        private void GameCollision3D_OnClick()
        {
            SceneManager.SetActiveScene("GameCollision3D");
        }
    }
}
