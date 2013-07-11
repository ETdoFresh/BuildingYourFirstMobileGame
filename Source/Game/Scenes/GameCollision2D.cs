using Microsoft.Xna.Framework;
using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using Source.Game.Game2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Source.Game.Scenes
{
    class GameCollision2D : GameScene
    {
        private Hero2D _hero;
        private GameAnimatedSprite _staticHero;

        private GameSprite _background;

        public GameCollision2D() : base("GameCollision2D") { }

        public override void Initialize()
        {
            _background = new GameSprite("Game2D/Background");
            _background.DrawInFrontOf3D = false;
            AddSceneObject(_background);

            _hero = new Hero2D();
            _hero.DrawBoundingRect = true;
            AddSceneObject(_hero);

            _staticHero = new GameAnimatedSprite("Game2D/Hero_Spritesheet", 8, 80, new Point(32, 39));
            _staticHero.CreateBoundingRect(32, 39, Vector2.Zero);
            AddSceneObject(_staticHero);
            _staticHero.Translate(400, 388);
            _staticHero.PivotPoint = new Vector2(16, 39);
            _staticHero.DrawBoundingRect = true;

            _staticHero.PlayAnimation(true);

            // Tick the camera
            AddSceneObject(SceneManager.RenderContext.Camera);

            base.Initialize();
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