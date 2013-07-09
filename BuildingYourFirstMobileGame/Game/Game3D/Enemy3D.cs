using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using BuildingYourFirstMobileGame.Engine.Objects;

namespace BuildingYourFirstMobileGame.Game.Game3D
{
    class Enemy3D : GameObject3D
    {
        private GameAnimatedModel _enemyModel;

        private int _direction = 1; //Right = 1 / Left = -1
        private int _appearDelay;
        private int _totalAppearTime;
        private Random _rand;

        private const int Speed = 30; // px/sec
        private const int MinAppearDelay = 4000; // ms
        private const int MaxAppearDelay = 6000; // ms

        private int _dropDelay;
        private int _totalDropTime;
        private const int MinDropDelay = 3000;
        private const int MaxDropDelay = 7000;

        private Rock3D _rock;

        public override void Initialize()
        {
            base.Initialize();

            _enemyModel = new GameAnimatedModel("Game3D/Enemy");
            AddChild(_enemyModel);
            _enemyModel.AnimationComplete += EnemyAnimationComplete;

            _rock = new Rock3D();
            _rock.Initialize();

            _rand = new Random();
            _appearDelay = _rand.Next(MinAppearDelay, MaxAppearDelay);
            _dropDelay = _rand.Next(MinDropDelay, MaxDropDelay);
        }

        private void EnemyAnimationComplete(string animationName)
        {
            if(animationName.Equals("Drop"))
            {
                _enemyModel.SetAnimationSpeed(1f);
                _enemyModel.PlayAnimation("Fly");
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _enemyModel.PlayAnimation("Fly");
            _rock.LoadContent(contentManager);

            Translate(new Vector3(-480, 150, -100));
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            _totalAppearTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

            _rock.Update(renderContext);

            if (_totalAppearTime >= _appearDelay)
            {
                var enemyPos = LocalPosition;
                enemyPos.X +=
                    (float)((Speed) * renderContext.GameTime.ElapsedGameTime.TotalSeconds) *
                    _direction;

                Translate(enemyPos);

                var projVec = renderContext.GraphicsDevice.Viewport.Project(enemyPos, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

                if ((_direction == 1 && projVec.X >= renderContext.GraphicsDevice.Viewport.Width + 70) ||
                    (_direction == -1 && projVec.X <= -180))
                {
                    _direction *= -1;
                    _appearDelay = _rand.Next(MinAppearDelay, MaxAppearDelay);
                    _totalAppearTime = 0;
                }

                if (_rock.CanDrop)
                {
                    _totalDropTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

                    if (_totalDropTime >= _dropDelay)
                    {
                        _rock.Drop();

                        _enemyModel.SetAnimationSpeed(0.6f);
                        _enemyModel.PlayAnimation("Drop", false);
                        _totalDropTime = 0;
                        _dropDelay = _rand.Next(MinDropDelay, MaxDropDelay);
                    }
                }
            }

            if (!_rock.IsFalling || _rock.CanDrop)
                _rock.Translate(_enemyModel.GetBoneTransform("Rock_Position").Translation);
        }

        public override void Draw(RenderContext renderContext)
        {
            _rock.Draw(renderContext);

            base.Draw(renderContext);
        }
    }
}
