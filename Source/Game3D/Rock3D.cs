using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Source.Game2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game3D
{
    class Rock3D : GameObject3D
    {
        private GameModel _rockModel;
        private GameAnimatedSprite _explosionSprite;
        private float _currentSpeed = InitialDropSpeed;

        public bool IsFalling { get; private set; }

        private const float Gravity = 50.0f;
        private const float InitialDropSpeed = 60.0f;
        private const int FrameSize = 64;
        private const int ScaleFactor = 2;

        public bool CanDrop { get; private set; }
        public override Vector3 Position
        {
            get { return _rockModel.Position; }
            set { _rockModel.Position = value; }
        }

        public override void Initialize()
        {
            _rockModel = new GameModel("Game3D/Rock");
            _rockModel.Initialize();

            _explosionSprite = new GameAnimatedSprite("Game2D/Explosion_Spritesheet", 16, 50, new Point(FrameSize, FrameSize), 4);
            _explosionSprite.CanDraw = false;
            _explosionSprite.Scale = new Vector2(ScaleFactor, ScaleFactor);
            CanDrop = true;
            _explosionSprite.Initialize();

            base.Initialize();
        }

        public void SetTransform(Matrix transform)
        {
            WorldMatrix = transform;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _explosionSprite.LoadContent(contentManager);
            _rockModel.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public void Drop()
        {
            if (CanDrop)
            {
                CanDrop = false;
                _currentSpeed = InitialDropSpeed;
                IsFalling = true;
            }
        }

        public override void Update(RenderContext renderContext)
        {
            _rockModel.Update(renderContext);
            _explosionSprite.Update(renderContext);

            if (CanDrop) return;

            if (IsFalling)
            {
                var deltaTime = (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                _currentSpeed += Gravity * deltaTime;

                var rockPos = _rockModel.Position;
                rockPos.Y -= _currentSpeed * deltaTime;
                _rockModel.Position = rockPos;

                if (rockPos.Y <= -140)
                {
                    IsFalling = false;

                    _explosionSprite.CanDraw = true;

                    var projVec = renderContext.GraphicsDevice.Viewport.Project(rockPos, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

                    _explosionSprite.Position = new Vector2(projVec.X - FrameSize, projVec.Y + -FrameSize);
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
            if (CanDrop || IsFalling)
                _rockModel.Draw(renderContext);
            else
            {
                renderContext.SpriteBatch.Begin();
                _explosionSprite.Draw(renderContext);
                renderContext.SpriteBatch.End();
            }

            base.Draw(renderContext);
        }
    }
}