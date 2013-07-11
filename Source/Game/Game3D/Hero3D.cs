using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Game3D
{
    class Hero3D : GameObject3D
    {
        private GameAnimatedModel _hero;
        private int _direction = 1; //1 = Right / -1 = Left
        private const int Speed = 75;
        private float _animationSpeedScale = 1;

        public override void Initialize()
        {
            _hero = new GameAnimatedModel("Game3D/Vampire");
            _hero.SetAnimationSpeed(_animationSpeedScale);
            _hero.CreateBoundingBox(25, 50, 25, new Vector3(0, 25, 0));
            AddChild(_hero);
            Translate(0, -147, -100);
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _hero.PlayAnimation("Run");
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);
            var heroPos = WorldPosition;
            var projVec = renderContext.GraphicsDevice.Viewport.Project(heroPos, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

            if (_direction == 1 && projVec.X >= renderContext.GraphicsDevice.Viewport.Width)
            {
                _direction = -1;
            }
            else if (_direction == -1 &&
                projVec.X <= 0)
            {
                _direction = 1;
            }

            heroPos += Vector3.Right * (float)(Speed * renderContext.GameTime.ElapsedGameTime.TotalSeconds * _direction);
            Translate(heroPos);
            Rotate(0, 90 * _direction, 0);
        }
    }
}
