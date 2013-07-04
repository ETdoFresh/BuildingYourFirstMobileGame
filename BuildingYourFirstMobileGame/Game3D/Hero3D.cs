using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Game3D
{
    class Hero3D : GameObject3D
    {
        private GameModel _heroModel;
        private int _direction = 1; //1 = Right / -1 = Left
        private const int Speed = 75;

        public override void Initialize()
        {
            _heroModel = new GameModel("Game3D/Vampire");
            _heroModel.Position = new Vector3(0, -147, -100);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _heroModel.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            var heroPos = _heroModel.Position;
            var projVec = renderContext.GraphicsDevice.Viewport.Project(heroPos, renderContext.Camera.Projection, renderContext.Camera.View, Matrix.Identity);

            if (_direction == 1 && projVec.X >= renderContext.GraphicsDevice.Viewport.Width)
            {
                _direction = -1;
            }
            else if (_direction == -1 &&
                projVec.X <= 0)
            {
                _direction = 1;
            }

            heroPos += Vector3.Right * (float)(Speed * renderContext.GameTime.ElapsedGameTime.TotalSeconds * _direction);
            _heroModel.Position = heroPos;
            _heroModel.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90 * _direction), 0, 0);

            _heroModel.Update(renderContext);
        }

        public override void Draw(RenderContext renderContext)
        {
            _heroModel.Draw(renderContext);
        }

    }
}
