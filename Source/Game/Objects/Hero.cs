using Microsoft.Xna.Framework;
using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;

namespace Source.Game.Objects
{
    class Hero : GameObject3D
    {
        public enum HeroAction { Run, Shield, Jump, Slide, Die, None }

        private float _jumpTimer;
        private const float JUMP_TIMEOUT = 2.0f;
        private const float JUMP_SPEED = 150.0f;

        private GameAnimatedModel _model;
        private Vector2 _velocity;
        public HeroAction CurrentAction { get; private set; }
        public Vector3 WorldPos { get { return _model.WorldPosition; } }

        public override void Initialize()
        {
            _model = new GameAnimatedModel("Models/Hero");
            _model.Translate(0, Path.GROUND_POS, -100);
            _model.Scale(new Vector3(1.5f));
            AddChild(_model);

            //DrawBoundingBox = true;
            CurrentAction = HeroAction.None;

            base.Initialize();
        }

        public void SetHeroAction(HeroAction action)
        {
            if (CurrentAction != action)
            {
                _model.Rotate(0, 90, 0);

                switch (action)
                {
                    case HeroAction.Run:
                        _model.PlayAnimation("Run", true, 0.2f);
                        _model.CreateBoundingBox(100, 55, 25, new Vector3(0, 27.5f, 5));
                        break;
                    case HeroAction.Shield:
                        _model.PlayAnimation("Shield", true, 0.2f);
                        _model.CreateBoundingBox(100, 48, 25, new Vector3(0, 24.0f, 10));
                        break;
                    case HeroAction.Slide:
                        _model.PlayAnimation("Slide", true, 0.2f);
                        _model.CreateBoundingBox(100, 40, 45, new Vector3(0, 20.0f, -5));
                        break;
                    case HeroAction.Jump:
                        _model.PlayAnimation("Jump", true, 0.2f);
                        break;
                    case HeroAction.Die:
                        _velocity = Vector2.Zero;

                        _model.Translate(_model.LocalPosition.X, Path.GROUND_POS, _model.LocalPosition.Z);
                        _model.SetAnimationSpeed(0.5f);
                        _model.Rotate(0, 0, 0);
                        _model.PlayAnimation("Die", false);
                        break;
                }

                CurrentAction = action;
            }
        }

        public override void Update(RenderContext renderContext)
        {
            if (CurrentAction == HeroAction.Die)
            {
                base.Update(renderContext);
                return;
            }

            //STATE MACHINE
            var animSpeed = 0.5f * (renderContext.GameSpeed / renderContext.InitialGameSpeed);
            _model.SetAnimationSpeed(float.IsNaN(animSpeed) ? 0 : animSpeed);

            switch (CurrentAction)
            {
                case HeroAction.Run:

                    if (ButtonsController.SlidePressed)
                    {
                        SetHeroAction(HeroAction.Slide);
                    }
                    else if (ButtonsController.ShieldPressed)
                    {
                        SetHeroAction(HeroAction.Shield);
                    }
                    else if (ButtonsController.JumpPressed)
                    {
                        _jumpTimer = 0;
                        SetHeroAction(HeroAction.Jump);
                        _velocity.Y = JUMP_SPEED;
                    }
                    break;

                case HeroAction.Shield:
                    if (!ButtonsController.ShieldPressed)
                        SetHeroAction(HeroAction.Run);
                    break;
                case HeroAction.Slide:
                    if (!ButtonsController.SlidePressed)
                        SetHeroAction(HeroAction.Run);
                    break;
                case HeroAction.Jump:
                    _jumpTimer += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
                    _velocity.Y -= (JUMP_SPEED * 2f) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

                    if ((ButtonsController.JumpPressed && _velocity.Y < 0) && _jumpTimer < JUMP_TIMEOUT)
                        _velocity.Y = 0;
                    else if (_model.LocalPosition.Y <= Path.GROUND_POS)
                    {
                        ButtonsController.ForceButtonRelease(0);
                        _velocity.Y = 0;
                        _model.LocalPosition = new Vector3(_model.LocalPosition.X, Path.GROUND_POS, _model.LocalPosition.Z);
                        SetHeroAction(HeroAction.Run);
                    }
                    break;
                case HeroAction.None:
                    SetHeroAction(HeroAction.Run);
                    break;
            }

            //POSITION
            _velocity.X = renderContext.GameSpeed;
            var newPos = _model.LocalPosition + (new Vector3(_velocity, 0) * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds);
            _model.Translate(newPos);

            base.Update(renderContext);
        }
    }
}
