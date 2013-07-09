using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using BuildingYourFirstMobileGame.Game.Game3D;
using BuildingYourFirstMobileGame.Engine.Objects;

namespace BuildingYourFirstMobileGame.Game.Scenes
{
    class GameCollision3D : GameScene
    {
        private Hero3D _hero;
        private GameAnimatedModel _staticHero;

        private GameSprite _background;

        public GameCollision3D() : base("GameCollision3D") { }

        public override void Initialize()
        {
            _background = new GameSprite("Game2D/Background");
            _background.DrawInFrontOf3D = false;
            AddSceneObject(_background);

            _hero = new Hero3D();
            _hero.DrawBoundingBox = true;
            AddSceneObject(_hero);

            _staticHero = new GameAnimatedModel("Game3D/Vampire");
            _staticHero.CreateBoundingBox(25, 50, 25, new Vector3(0, 25, 0));
            _staticHero.SetAnimationSpeed(1.0f);
            AddSceneObject(_staticHero);
            _staticHero.Translate(150, -147, -100);
            _staticHero.DrawBoundingBox = true;

            // Tick the camera
            AddSceneObject(SceneManager.RenderContext.Camera);

            base.Initialize();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _staticHero.PlayAnimation("Run");
        }

        public override void Update(RenderContext renderContext)
        {
            if (_hero.HitTest(_staticHero))
                Debug.WriteLine("We have collision");
            else
                Debug.WriteLine("NO collision");
            base.Update(renderContext);
        }
    }
}
