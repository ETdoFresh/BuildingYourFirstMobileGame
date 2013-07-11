using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Scenes
{
    class MenuScene : GameScene
    {
        GameSprite _background;
        GameButton _startButton;
        GameButton _exitButton;
        Song _backgroundMusic;

        public MenuScene() : base("Menu") { }

        public override void Initialize()
        {
            _background = new GameSprite("Sprites/MenuBackground");
            AddSceneObject(_background);

            _startButton = new GameButton("Sprites/StartButton", true);
            _startButton.PivotPoint = new Vector2(162, 0);
            _startButton.Translate(400, 150);
            _startButton.OnClick += () => SceneManager.SetActiveScene("Level");
            AddSceneObject(_startButton);

            _exitButton = new GameButton("Sprites/ExitButton", true);
            _exitButton.PivotPoint = new Vector2(162, 0);
            _exitButton.Translate(400, 250);
            _exitButton.OnClick += () => SceneManager.MainGame.Exit();
            AddSceneObject(_exitButton);

            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            //_backgroundMusic = contentManager.Load<Song>("BackgroundMusic");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(_backgroundMusic);

            base.LoadContent(contentManager);
        }
    }
}
