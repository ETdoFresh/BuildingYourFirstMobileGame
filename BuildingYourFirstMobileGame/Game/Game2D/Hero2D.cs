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
    class Hero2D : GameObject2D
    {
        private GameAnimatedSprite _heroSprite;
        private const int FrameWidth = 18;
        private int _direction = 1; // Right = 1 | Left = -1
        private const int Speed = 60; // px per sec

        public override void Initialize()
        {
            base.Initialize();
            _heroSprite = new GameAnimatedSprite("Game2D/Hero_Spritesheet", 8, 80, new Point(32, 39));
            _heroSprite.Translate(18, 388);
            _heroSprite.PivotPoint = new Vector2(16, 39);
            AddChild(_heroSprite);

            base.Initialize();

            _heroSprite.PlayAnimation(true);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var heroPos = _heroSprite.LocalPosition;

            if (_direction == 1 && heroPos.X >= renderContext.GraphicsDevice.Viewport.Width - (FrameWidth * _heroSprite.LocalScale.X))
            {
                _direction = -1;
                _heroSprite.Effect = SpriteEffects.FlipHorizontally;
            }
            else if (_direction == -1 && heroPos.X < 0)
            {
                _direction = 1;
                _heroSprite.Effect = SpriteEffects.None;
            }

            heroPos.X += (float)(Speed * renderContext.GameTime.ElapsedGameTime.TotalSeconds * _direction);
            _heroSprite.Translate(heroPos);
        }
    }
}
