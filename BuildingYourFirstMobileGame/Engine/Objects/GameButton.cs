using BuildingYourFirstMobileGame.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.Objects
{
    class GameButton : GameSprite
    {
        public event Action OnClick = delegate { };
        public event Action OnEnter = delegate { };
        public event Action OnLeave = delegate { };

        public bool IsPressed { get { return _isPressed; } }

        private bool _isSpriteSheet;
        private Rectangle? _normalRect, _pressedRect;
        private bool _isPressed;
        private int _touchId;

        public GameButton(string assetFile) :
            this(assetFile, false) { }


        public GameButton(string assetFile, bool isSpriteSheet) :
            base(assetFile)
        {
            _isSpriteSheet = isSpriteSheet;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            //Set Dimensions after the button texture is loaded, otherwise we can't extract the width and height
            if (_isSpriteSheet)
            {
                CreateBoundingRect((int)Width, (int)Height / 2);
                _normalRect = new Rectangle(0, 0, (int)Width, (int)(Height / 2f));
                _pressedRect = new Rectangle(0, (int)(Height / 2f), (int)Width, (int)(Height / 2f));
            }
            else CreateBoundingRect((int)Width, (int)Height);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var touchStates = renderContext.TouchPanelState;
            if (touchStates.IsConnected)
            {
                if (!_isPressed)
                {
                    DrawRect = _normalRect;

                    foreach (var touchLoc in touchStates)
                    {
                        if (touchLoc.State == TouchLocationState.Pressed && HitTest(touchLoc.Position, false))
                        {
                            _isPressed = true;
                            _touchId = touchLoc.Id;

                            //ENTERED
                            if (OnEnter != null) OnEnter();
                            DrawRect = _pressedRect;
                            break;
                        }
                    }
                    if (InputHelper.IsMousePressed() && HitTest(new Vector2(InputHelper.CurrentMouseState.X, InputHelper.CurrentMouseState.Y), false))
                    {
                        _isPressed = true;

                        OnEnter();
                        DrawRect = _pressedRect;
                    }
                }
                else
                {
                    var touchLoc = touchStates.FirstOrDefault(tLocation => tLocation.Id == _touchId);

                    if ((touchLoc == null || !HitTest(touchLoc.Position, false)) && !HitTest(new Vector2(InputHelper.CurrentMouseState.X, InputHelper.CurrentMouseState.Y), false))
                    {
                        _touchId = -1;
                        _isPressed = false;

                        //LEFT
                        if (OnLeave != null) OnLeave();
                    }
                    else
                    {
                        if (touchLoc.State == TouchLocationState.Released || InputHelper.IsReleased())
                        {
                            _touchId = -1;
                            _isPressed = false;

                            if (OnClick != null) OnClick();
                        }
                    }
                }
            }
        }
    }
}
