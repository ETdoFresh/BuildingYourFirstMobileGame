using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.Helpers
{
    static class InputHelper
    {
        public enum State { Idle, Pressed, Hold, Released }
        public enum MouseButton { Left, Right, Middle }

        static private MouseState _prevMouseState;
        static private MouseState _currentMouseState;

        static private KeyboardState _prevKeyboardState;
        static private KeyboardState _currentKeyboardState;

        static public MouseState CurrentMouseState { get { return _currentMouseState; } }
        static public KeyboardState CurrentKeyboardState { get { return _currentKeyboardState; } }

        static public bool IsMousePressed(MouseButton mouseButton = MouseButton.Left)
        {
            if (GetMouseState(mouseButton) == State.Pressed)
                return true;

            return false;
        }

        static public bool isKeyPressed(Keys key)
        {
            if (GeyKeyboardState(key) == State.Pressed)
                return true;

            return false;
        }

        static public bool IsReleased()
        {
            if (_prevMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released)
                return true;

            return false;
        }

        static public State GetMouseState(MouseButton mouseButton = MouseButton.Left)
        {
            ButtonState prevState, currentState;

            switch (mouseButton)
            {
                case (MouseButton.Right):
                    prevState = _prevMouseState.RightButton;
                    currentState = _currentMouseState.RightButton;
                    break;
                case (MouseButton.Middle):
                    prevState = _prevMouseState.MiddleButton;
                    currentState = _currentMouseState.MiddleButton;
                    break;
                case (MouseButton.Left):
                default:
                    prevState = _prevMouseState.LeftButton;
                    currentState = _currentMouseState.LeftButton;
                    break;
            }

            if (prevState == ButtonState.Released && currentState == ButtonState.Pressed)
                return State.Pressed;
            else if (prevState == ButtonState.Pressed && currentState == ButtonState.Released)
                return State.Released;
            else if (prevState == ButtonState.Pressed && currentState == ButtonState.Pressed)
                return State.Hold;
            else
                return State.Idle;
        }

        static public State GeyKeyboardState(Keys key)
        {
            if (_prevKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key))
                return State.Pressed;
            else if (_prevKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key))
                return State.Released;
            else if (_prevKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyDown(key))
                return State.Hold;
            else
                return State.Idle;
        }

        static public void Update(RenderContext renderContext)
        {
            _prevMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _prevKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
    }
}
