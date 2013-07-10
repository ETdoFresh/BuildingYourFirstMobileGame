using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source
{
    class Hero2D : GameObject2D
    {
        private GameAnimatedSprite _heroSprite;
        private const int FrameWidth = 32;
        private int _direction = 1; // Right = 1 | Left = -1
        private const int Speed = 60; // px per sec

        public override void Initialize()
        {
            base.Initialize();
            _heroSprite = new GameAnimatedSprite("Game2D/Hero_Spritesheet", 8, 80, new Point(32, 39));
            _heroSprite.Position = new Vector2(10, 348);
            _heroSprite.PlayAnimation(true);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _heroSprite.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            _heroSprite.Update(renderContext);

            var heroPos = _heroSprite.Position;

            if (_direction == 1 && heroPos.X >= renderContext.GraphicsDevice.Viewport.Width - (FrameWidth * _heroSprite.Scale.X))
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
            _heroSprite.Position = heroPos;
        }

        public override void Draw(RenderContext renderContext)
        {
            _heroSprite.Draw(renderContext);
        }
    }
}