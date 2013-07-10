using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Game2D
{
    class Rock2D : GameObject2D
    {
        private GameSprite _rockSprite;
        private GameAnimatedSprite _explosionSprite;
        private float _currentSpeed = InitialDropSpeed;
        private bool _isFalling;

        private const float Gravity = 50.0f;
        private const float InitialDropSpeed = 60.0f;
        private const int FrameSize = 64;
        private const int ScaleFactor = 2;

        public bool CanDrop { get; private set; }

        public override void Initialize()
        {
            _rockSprite = new GameSprite("Game2D/Rock");
            _rockSprite.PivotPoint = new Vector2(10, 25);
            _rockSprite.CanDraw = false;
            AddChild(_rockSprite);

            _explosionSprite = new GameAnimatedSprite("Game2D/Explosion_Spritesheet", 16, 50, new Point(FrameSize, FrameSize), 4);
            _explosionSprite.PivotPoint = new Vector2(24, 40);
            _explosionSprite.CanDraw = false;
            _explosionSprite.Scale(new Vector2(ScaleFactor, ScaleFactor));
            CanDrop = true;
            AddChild(_explosionSprite);

            base.Initialize();
        }

        public void Drop(Vector2 pos)
        {
            if (CanDrop)
            {
                CanDrop = false;
                _rockSprite.Translate(pos);
                _rockSprite.CanDraw = true;
                _currentSpeed = InitialDropSpeed;
                _isFalling = true;
            }
        }

        public override void Update(RenderContext renderContext)
        {
            if (CanDrop) return;

            if (_isFalling)
            {
                var deltaTime = (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                _currentSpeed += Gravity * deltaTime;

                var rockPos = _rockSprite.LocalPosition;
                rockPos.Y += _currentSpeed * deltaTime;
                _rockSprite.Translate(rockPos);

                if (rockPos.Y >= 390)
                {
                    _isFalling = false;
                    _rockSprite.CanDraw = false;

                    _explosionSprite.CanDraw = true;
                    _explosionSprite.Translate(rockPos);
                    _explosionSprite.PlayAnimation();
                }
            }
            else
            {
                if (!_explosionSprite.IsPlaying)
                {
                    _explosionSprite.CanDraw = false;
                    CanDrop = true;
                }
            }

            base.Update(renderContext);
        }
    }
}
