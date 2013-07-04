using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame
{
    class Hero2D : GameObject2D
    {
        private GameSprite _heroSprite;
        private int _direction = 1; // Right = 1 | Left = -1
        private const int Speed = 60; // px per sec

        public override void Initialize()
        {
            base.Initialize();
            _heroSprite = new GameSprite("Game2D/Hero");
            _heroSprite.Position = new Vector2(10, 348);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _heroSprite.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);
            var heroPos = _heroSprite.Position;

            if (_direction == 1 && heroPos.X >= renderContext.GraphicsDevice.Viewport.Width - (_heroSprite.Width * _heroSprite.Scale.X))
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
