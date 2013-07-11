using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using Source.Engine;
using Microsoft.Xna.Framework.Content;

namespace Source.Game.Objects
{
    class Enemy : GameObject3D
    {
        private GameAnimatedModel _enemyModel;

        private GameModel _rockModel;
        private bool _rockFalling;
        private Vector3 _rockDirection;
        public bool RockHit { get; set; }
        private float _rockSpeed;

        private GameAnimatedSprite _explosionSprite;
        private Hero _hero;

        public Enemy(Hero hero)
        {
            _hero = hero;
        }

        public override void Initialize()
        {
            _enemyModel = new GameAnimatedModel("Models/Enemy");
            _enemyModel.AnimationComplete += EnemyAnimationComplete;
            _enemyModel.Translate(0, 150, -100);
            AddChild(_enemyModel);

            _rockModel = new GameModel("Models/Rock");
            _rockModel.CreateBoundingBox(30, 30, 30);
            //_rockModel.DrawBoundingBox = true;
            AddChild(_rockModel);

            _explosionSprite = new GameAnimatedSprite("Sprites/Explosion_Spritesheet", 16, 50, new Point(64, 64), 4);
            _explosionSprite.Scale(new Vector2(2f));
            _explosionSprite.Initialize();

            base.Initialize();
        }

        private void EnemyAnimationComplete(string name)
        {
            if (name.Equals("Drop"))
                _enemyModel.PlayAnimation("Fly", true, 0.2f);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _explosionSprite.LoadContent(contentManager);
            _enemyModel.PlayAnimation("Fly");
        }

        public override void Update(RenderContext renderContext)
        {
            if (_rockFalling)
            {
                if (!RockHit)
                {
                    var rockPos = _rockModel.LocalPosition;
                    rockPos -= _rockDirection * _rockSpeed * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                    _rockModel.Translate(rockPos);

                    if (rockPos.Y <= Path.GROUND_POS)
                    {
                        RockHit = true;
                    }
                }
                else
                {
                    _rockModel.CanDraw = false;

                    var projVec = renderContext.GraphicsDevice.Viewport.Project(_rockModel.WorldPosition, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

                    _explosionSprite.Translate(new Vector2(projVec.X - 64, projVec.Y - 64));
                    _explosionSprite.PlayAnimation();
                    _explosionSprite.Update(renderContext);

                    if (!_explosionSprite.IsPlaying)
                    {
                        _rockModel.CanDraw = true;
                        _rockFalling = false;
                        RockHit = false;
                    }
                }
            }

            base.Update(renderContext);

            if (!_rockFalling)
            {
                var boneMat = _enemyModel.GetBoneTransform("Rock_Position");
                _rockModel.Translate(boneMat.Translation - LocalPosition);
                _rockModel.Update(renderContext);

                var heroDistance = WorldPosition.X - _hero.WorldPos.X;
                if (Math.Abs(heroDistance) <= 250)
                {
                    var futureHeroHitPos = _hero.WorldPos + new Vector3(renderContext.GameSpeed, 60, 0);
                    _rockDirection = _rockModel.WorldPosition - futureHeroHitPos;
                    _rockSpeed = _rockDirection.Length();
                    _rockDirection.Normalize();

                    _enemyModel.PlayAnimation("Drop", false, 0.5f);
                    _rockFalling = true;
                }
            }
        }

        public override void Draw(RenderContext renderContext)
        {
            if (RockHit)
            {
                renderContext.SpriteBatch.Begin();
                _explosionSprite.Draw(renderContext);
                renderContext.SpriteBatch.End();

                //renderContext.GraphicsDevice.BlendState = BlendState.Opaque;
                //renderContext.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                //renderContext.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                SceneManager.MainGame.ResetRenderState();
            }

            base.Draw(renderContext);
        }
    }
}
