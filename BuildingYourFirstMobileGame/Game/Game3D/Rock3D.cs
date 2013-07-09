using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using BuildingYourFirstMobileGame.Engine.Objects;

namespace BuildingYourFirstMobileGame.Game.Game3D
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

        public override void Initialize()
        {
            _rockModel = new GameModel("Game3D/Rock");
            AddChild(_rockModel);

            _explosionSprite = new GameAnimatedSprite("Game2D/Explosion_Spritesheet", 16, 50, new Point(FrameSize, FrameSize), 4);
            _explosionSprite.CanDraw = false;
            _explosionSprite.Scale(new Vector2(ScaleFactor, ScaleFactor));
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
            _explosionSprite.Update(renderContext);
            base.Update(renderContext);

            if (CanDrop) return;

            if (IsFalling)
            {
                var deltaTime = (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                _currentSpeed += Gravity * deltaTime;

                var rockPos = LocalPosition;
                rockPos.Y -= _currentSpeed * deltaTime;
                Translate(rockPos);

                if (rockPos.Y <= -140)
                {
                    IsFalling = false;

                    _explosionSprite.CanDraw = true;
                    _rockModel.CanDraw = false;

                    var projVec = renderContext.GraphicsDevice.Viewport.Project(rockPos, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

                    _explosionSprite.Translate(new Vector2(projVec.X - FrameSize, projVec.Y + -FrameSize));
                    _explosionSprite.PlayAnimation();
                }
            }
            else
            {
                if (!_explosionSprite.IsPlaying)
                {
                    _rockModel.CanDraw = true;
                    _explosionSprite.CanDraw = false;
                    CanDrop = true;
                }
            }
        }

        public override void Draw(RenderContext renderContext)
        {
            if (!(CanDrop || IsFalling))
            {
                renderContext.SpriteBatch.Begin();
                _explosionSprite.Draw(renderContext);
                renderContext.SpriteBatch.End();

                //Reset Renderstate
                renderContext.GraphicsDevice.BlendState = BlendState.Opaque;
                renderContext.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                renderContext.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }

            base.Draw(renderContext);
        }
    }
}
