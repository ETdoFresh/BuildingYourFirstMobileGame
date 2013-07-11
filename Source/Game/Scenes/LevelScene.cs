using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Source.Engine;
using Source.Engine.Helper;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using Source.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Scenes
{
    class LevelScene : GameScene
    {
        private Background _background;
        private Path _path;
        private ButtonsController _buttonsController;
        private Hero _hero;

        private GameSprite _gameOverSprite;
        private List<GameObject3D> _obstacles = new List<GameObject3D>();
        private const float SET_OBSTACLE_THRESHOLD = 700;
        private float _moveAmount;
        private int _speedUpCount;

        public LevelScene() : base("Level") { }

        public override void Initialize()
        {
            _background = new Background();
            _background.DrawInFrontOf3D = false;
            AddSceneObject(_background);

            _path = new Path();
            AddSceneObject(_path);

            _buttonsController = new ButtonsController();
            AddSceneObject(_buttonsController);

            _hero = new Hero();
            _hero.Translate(-100, 0, 0);
            AddSceneObject(_hero);

            _gameOverSprite = new GameSprite("Sprites/GameOver");
            _gameOverSprite.Translate(140, 180);
            AddSceneObject(_gameOverSprite);

            AddObstacle(new SwingBall());
            AddObstacle(new SwingBall());
            AddObstacle(new Spikes());
            AddObstacle(new Spikes());
            AddObstacle(new Enemy(_hero));
            AddObstacle(new Enemy(_hero));

            var cam = new PerspectiveCamera();
            cam.Rotate(-5, 0, 0);
            cam.Translate(0, 50, 350);
            SceneManager.RenderContext.Camera = cam;
            AddSceneObject(SceneManager.RenderContext.Camera);

            base.Initialize();

            ResetLevel();
        }

        private void AddObstacle(GameObject3D obstacle)
        {
            AddSceneObject(obstacle);
            _obstacles.Add(obstacle);
        }

        private void ResetLevel()
        {
            foreach(var obstacle in _obstacles) obstacle.Translate(-1000, 0, 0);
            _hero.SetHeroAction(Hero.HeroAction.None);

            SceneManager.RenderContext.GameSpeed = SceneManager.RenderContext.InitialGameSpeed = 100;

            _gameOverSprite.CanDraw = false;
            _buttonsController.CanDraw = true;

            _moveAmount = 0;
            _speedUpCount = 0;
        }

        public override void Deactivated()
        {
            ResetLevel();
            base.Deactivated();
        }

        private void SetObstacle(Vector3 camPos)
        {
            var currentIteration = 0;
            while (true)
            {
                if (currentIteration >= 10) break;

                var randomIndex = new Random().Next(0, _obstacles.Count);

                if (_obstacles[randomIndex].WorldPosition.X < (camPos.X - SET_OBSTACLE_THRESHOLD))
                {
                    _obstacles[randomIndex].Translate(new Vector3(camPos.X + SET_OBSTACLE_THRESHOLD, 0, 0));
                    break;
                }

                ++currentIteration;
            }
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext); 
            
            if (_hero.CurrentAction == Hero.HeroAction.Die)
            {
                //GAME OVER!
                renderContext.GameSpeed = 0;

                _buttonsController.CanDraw = false;
                _gameOverSprite.CanDraw = true;

                if (InputHelper.CurrentTouchStates.Count > 0 && InputHelper.CurrentTouchStates[0].State == TouchLocationState.Released || InputHelper.IsMousePressed(InputHelper.MouseButton.Left))
                {
                    SceneManager.SetActiveScene("Menu");
                }
            }
            else
            {
                //OBSTACLE COLLISION
                foreach (var obstacle in _obstacles)
                {
                    if (_hero.HitTest(obstacle))
                    {
                        if (obstacle is Enemy)
                        {
                            (obstacle as Enemy).RockHit = true;

                            if (_hero.CurrentAction != Hero.HeroAction.Shield)
                                _hero.SetHeroAction(Hero.HeroAction.Die);
                        }
                        else
                        {
                            _hero.SetHeroAction(Hero.HeroAction.Die);
                        }
                    }
                }

                //CAMERA MOVEMENT
                var camPos = renderContext.Camera.LocalPosition;
                camPos += new Vector3(renderContext.GameSpeed * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds, 0, 0);
                renderContext.Camera.Translate(camPos);

                //SET NEW OBSTACLE
                _moveAmount += renderContext.GameSpeed * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                if (_moveAmount >= SET_OBSTACLE_THRESHOLD)
                {
                    _moveAmount = 0;
                    SetObstacle(camPos);

                    ++_speedUpCount;
                    if (_speedUpCount >= 5)
                    {
                        _speedUpCount = 0;
                        renderContext.GameSpeed += 30;
                        if (renderContext.GameSpeed > 300) renderContext.GameSpeed = 300;
                    }
                }
            }
        }
    }
}
