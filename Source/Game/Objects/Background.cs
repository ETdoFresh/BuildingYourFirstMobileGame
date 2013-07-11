using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Objects
{
    class Background:GameObject2D
    {
        private GameSprite _air;

        private const float MOUNTAINS_SPEED = 0.4f;
        private GameSprite _mountains;

        private const float CLOUDS_FRONT_SPEED = 0.7f;
        private GameSprite _cloudsFront;

        private const float CLOUDS_MIDDLE_SPEED = 0.5f;
        private GameSprite _cloudsMiddle;

        private const float CLOUDS_BACK_SPEED = 0.3f;
        private GameSprite _cloudsBack;

        public override void Initialize()
        {
            _air = new GameSprite("Sprites/Background_Air");
            AddChild(_air);

            _cloudsBack = new GameSprite("Sprites/Background_CloudsBack");
            AddChild(_cloudsBack);

            _mountains = new GameSprite("Sprites/Background_Mountains");
            AddChild(_mountains);

            _cloudsMiddle = new GameSprite("Sprites/Background_CloudsMiddle");
            AddChild(_cloudsMiddle);

            _cloudsFront = new GameSprite("Sprites/Background_CloudsFront");
            AddChild(_cloudsFront);

            base.Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            //Mountains Position
            var objectSpeed = renderContext.GameSpeed * MOUNTAINS_SPEED;
            objectSpeed *= (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            var objectPosX = _mountains.LocalPosition.X - objectSpeed;

            if (objectPosX < -800)
                objectPosX += 800;

            _mountains.Translate(objectPosX, 225);

            //Front Clouds Position
            objectSpeed = renderContext.GameSpeed * CLOUDS_FRONT_SPEED;
            objectSpeed *= (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            objectPosX = _cloudsFront.LocalPosition.X - objectSpeed;

            if (objectPosX < -800)
                objectPosX += 800;

            _cloudsFront.Translate(objectPosX, 25);

            //Middle Clouds Position
            objectSpeed = renderContext.GameSpeed * CLOUDS_MIDDLE_SPEED;
            objectSpeed *= (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            objectPosX = _cloudsMiddle.LocalPosition.X - objectSpeed;

            if (objectPosX < -800)
                objectPosX += 800;

            _cloudsMiddle.Translate(objectPosX, 130);

            //Back Clouds Position
            objectSpeed = renderContext.GameSpeed * CLOUDS_BACK_SPEED;
            objectSpeed *= (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            objectPosX = _cloudsBack.LocalPosition.X - objectSpeed;

            if (objectPosX < -800)
                objectPosX += 800;

            _cloudsBack.Translate(objectPosX, 200);

            base.Update(renderContext);
        }
    }
}
