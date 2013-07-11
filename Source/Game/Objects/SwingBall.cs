using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using Source.Engine;
using Microsoft.Xna.Framework.Content;

namespace Source.Game.Objects
{
    class SwingBall : GameObject3D
    {
        private GameAnimatedModel _model;
        private EmptyObject3D _ballHitregion;

        public override void Initialize()
        {
            _model = new GameAnimatedModel("Models/SwingBall");
            _model.Translate(0, 242, -100);
            _model.Scale(new Vector3(0.76f));
            AddChild(_model);

            _ballHitregion = new EmptyObject3D();
            _ballHitregion.CreateBoundingBox(90, 90, 100);
            //_ballHitregion.DrawBoundingBox = true;
            AddChild(_ballHitregion);

            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _model.PlayAnimation("Swing");
            _model.SetAnimationSpeed(0.8f);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var boneMat = _model.GetBoneTransform("BallBone");
            _ballHitregion.Translate(boneMat.Translation - LocalPosition);
            _ballHitregion.Update(renderContext);
        }
    }
}
