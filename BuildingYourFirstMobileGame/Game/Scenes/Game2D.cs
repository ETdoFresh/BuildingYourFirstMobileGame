using BuildingYourFirstMobileGame.Engine.Objects;
using BuildingYourFirstMobileGame.Engine.SceneGraph;
using BuildingYourFirstMobileGame.Game.Game2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Game.Scenes
{
    class Game2D : GameScene
    {
        private GameSprite _background;
        private Hero2D _hero;
        private Enemy2D _enemy;

        public Game2D()
            : base("Game2D")
        {
            _background = new GameSprite("Game2D/BackGround");
            AddSceneObject(_background);

            _hero = new Hero2D();
            AddSceneObject(_hero);

            _enemy = new Enemy2D();
            AddSceneObject(_enemy);
        }
    }
}
