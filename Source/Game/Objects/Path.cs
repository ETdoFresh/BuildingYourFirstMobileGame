using Microsoft.Xna.Framework;
using Source.Engine;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game.Objects
{
    class Path : GameObject3D
    {
        private GameModel[] _pathParts = new GameModel[5];
        private int _tailPartIndex;
        private float _prevSwitchPosition;
        public const int GROUND_POS = -147;

        public override void Initialize()
        {
            for (var i = 0; i < _pathParts.Length; ++i)
            {
                _pathParts[i] = new GameModel("Models/Path");
                _pathParts[i].Translate(-1600 + (800 * i), GROUND_POS - 93, -100);
                AddChild(_pathParts[i]);
            }

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (renderContext.Camera.LocalPosition.X - _prevSwitchPosition >= 800)
            {
                _prevSwitchPosition += 800;

                var tailPos = _pathParts[_tailPartIndex].LocalPosition;
                tailPos = new Vector3(_prevSwitchPosition + 1600, tailPos.Y, tailPos.Z);
                _pathParts[_tailPartIndex].Translate(tailPos);

                _tailPartIndex = (_tailPartIndex + 1) % _pathParts.Length;
            }

            base.Update(renderContext);
        }
    }
}