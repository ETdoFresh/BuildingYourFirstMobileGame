using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame
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
            _rockSprite.CanDraw = false;
            _rockSprite.Initialize();

            _explosionSprite = new GameAnimatedSprite("Game2D/Explosion_Spritesheet", 16, 50, new Point(FrameSize, FrameSize), 4);
            _explosionSprite.CanDraw = false;
            _explosionSprite.Scale = new Vector2(ScaleFactor, ScaleFactor);
            CanDrop = true;
            _explosionSprite.Initialize();

            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _explosionSprite.LoadContent(contentManager);
            _rockSprite.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public void Drop(Vector2 pos)
        {
            if (CanDrop)
            {
                CanDrop = false;
                _rockSprite.Position = pos;
                _rockSprite.CanDraw = true;
                _currentSpeed = InitialDropSpeed;
                _isFalling = true;
            }
        }

        public override void Update(RenderContext renderContext)
        {
            _rockSprite.Update(renderContext);
            _explosionSprite.Update(renderContext);

            if (CanDrop) return;

            if (_isFalling)
            {
                var deltaTime = (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                _currentSpeed += Gravity * deltaTime;

                var rockPos = _rockSprite.Position;
                rockPos.Y += _currentSpeed * deltaTime;
                _rockSprite.Position = rockPos;

                if (rockPos.Y >= 350)
                {
                    _isFalling = false;
                    _rockSprite.CanDraw = false;

                    _explosionSprite.CanDraw = true;
                    // Update explosion position taking center of rock position and center of explosion into account
                    // Necessary because of scale!
                    _explosionSprite.Position = new Vector2(rockPos.X + (_rockSprite.Width / ScaleFactor) - FrameSize, rockPos.Y + (_rockSprite.Height / ScaleFactor) - FrameSize);
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

        public override void Draw(RenderContext renderContext)
        {
            _rockSprite.Draw(renderContext);
            _explosionSprite.Draw(renderContext);

            base.Draw(renderContext);
        }
    }
}
