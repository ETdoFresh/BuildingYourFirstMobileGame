using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;

namespace Source.Game.Objects
{
    class ButtonsController:GameObject2D
    {
        private GameButton[] _buttons = new GameButton[3];
        private static bool[] _buttonsPressed = new bool[3];
        private static bool[] _forcedRelease = new bool[3];

        private static List<GameButton> _tempButtonList = new List<GameButton>();
        private static Random _random = new Random();

        public static bool JumpPressed { get { return _buttonsPressed[0]; } }
        public static bool SlidePressed { get { return _buttonsPressed[1]; } }
        public static bool ShieldPressed { get { return _buttonsPressed[2]; } }

        public override void Initialize()
        {
            _buttons[0] = new GameButton("Sprites/JumpButton", true);
            AddChild(_buttons[0]);

            _buttons[1] = new GameButton("Sprites/SlideButton", true);
            AddChild(_buttons[1]);

            _buttons[2] = new GameButton("Sprites/ShieldButton", true);
            AddChild(_buttons[2]);

            RandomizeButtons();

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            for (var i = 0; i < _buttons.Length; ++i)
            {
                if (_buttons[i].IsPressed && (!JumpPressed || !SlidePressed || !ShieldPressed))
                {
                    if (!_forcedRelease[i]) _buttonsPressed[i] = true;
                }
                else if (!_buttons[i].IsPressed && (_buttonsPressed[i] || _forcedRelease[i]))
                {
                    _buttonsPressed[i] = false;
                    _forcedRelease[i] = false;
                    RandomizeButtons();
                }
                else _buttonsPressed[i] = false;
            }

            base.Update(renderContext);
        }

        public static void ForceButtonRelease(int buttonIndex)
        {
            if (_buttonsPressed[buttonIndex])
            {
                _buttonsPressed[buttonIndex] = false;
                _forcedRelease[buttonIndex] = true;
            }
        }

        private void RandomizeButtons()
        {
            _tempButtonList.Clear();
            _tempButtonList.AddRange(_buttons);
            var iterations = 0;

            while (_tempButtonList.Count > 0)
            {
                var index = _random.Next(0, _tempButtonList.Count);

                _tempButtonList[index].Translate(0, 160 * iterations);
                _tempButtonList.RemoveAt(index);
                ++iterations;
            }
        }
    }
}
