using BuildingYourFirstMobileGame.Engine.Objects;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Game.Game2D
{
    class Enemy2D : GameObject2D
    {
        private GameSprite _enemySprite;
        private GameSprite _enemyWithoutRockSprite;

        private int _direction = 1; //Right = 1 / Left = -1
        private float _wobbleSpeed;
        private float _totalWobbleSpeed;
        private int _appearDelay;
        private int _totalAppearTime;
        private Random _rand;

        private const int Speed = 30; // px/sec
        private const float WobbleInterval = 1.0f; // s/pass
        private const int MinAppearDelay = 4000; // ms
        private const int MaxAppearDelay = 6000; // ms

        private int _dropDelay;
        private int _totalDropTime;
        private const int MinDropDelay = 3000;
        private const int MaxDropDelay = 7000;

        private Rock2D _rock;

        public override void Initialize()
        {
            base.Initialize();

            _enemySprite = new GameSprite("Game2D/Enemy");
            AddChild(_enemySprite);

            _enemyWithoutRockSprite = new GameSprite("Game2D/Enemy_NoRock");
            AddChild(_enemyWithoutRockSprite);
            _enemyWithoutRockSprite.CanDraw = false;

            _rock = new Rock2D();
            _rock.Initialize();

            _wobbleSpeed = (float)(2.0f * Math.PI) / WobbleInterval;
            _rand = new Random();
            _appearDelay = _rand.Next(MinAppearDelay, MaxAppearDelay);
            _dropDelay = _rand.Next(MinDropDelay, MaxDropDelay);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            // In LoadContent because we have to know the width of the enemy sprite
            _enemySprite.Translate(-_enemySprite.Width, 20);
            _rock.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            _totalAppearTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

            if (_totalAppearTime >= _appearDelay)
            {
                _totalWobbleSpeed += _wobbleSpeed * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                _totalWobbleSpeed %= (float)Math.PI * 2.0f;

                var wobbleOffset = (float)Math.Sin(_totalWobbleSpeed);
                var zeppelinPos = _enemySprite.LocalPosition;
                zeppelinPos.X +=
                    (float)((Speed) * renderContext.GameTime.ElapsedGameTime.TotalSeconds) *
                    _direction;
                zeppelinPos.Y += wobbleOffset * 2.0f;

                _enemySprite.Translate(zeppelinPos);

                if ((_direction == 1 && zeppelinPos.X >= renderContext.GraphicsDevice.Viewport.Width) ||
                    (_direction == -1 && zeppelinPos.X <= -212))
                {
                    _direction *= -1;
                    _appearDelay = _rand.Next(MinAppearDelay, MaxAppearDelay);
                    _totalAppearTime = 0;
                    _enemySprite.Effect = (_enemySprite.Effect == SpriteEffects.None)
                                                 ? SpriteEffects.FlipHorizontally
                                                 : SpriteEffects.None;
                }

                _enemyWithoutRockSprite.Translate(_enemySprite.LocalPosition);
                _enemyWithoutRockSprite.Effect = _enemySprite.Effect;

                if (_rock.CanDrop)
                {
                    if (!_enemySprite.CanDraw)
                    {
                        _enemySprite.CanDraw = true;
                        _enemyWithoutRockSprite.CanDraw = false;
                    }
                    _totalDropTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;

                    if (_totalDropTime >= _dropDelay)
                    {
                        _rock.Drop(zeppelinPos + new Vector2(90, 90));
                        _totalDropTime = 0;
                        _dropDelay = _rand.Next(MinDropDelay, MaxDropDelay);

                        _enemyWithoutRockSprite.CanDraw = true;
                        _enemySprite.CanDraw = false;
                    }
                }
            }
            _rock.Update(renderContext);
        }

        public override void Draw(RenderContext renderContext)
        {
            _rock.Draw(renderContext);

            base.Draw(renderContext);
        }
    }
}
