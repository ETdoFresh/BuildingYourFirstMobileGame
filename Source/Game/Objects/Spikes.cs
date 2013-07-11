using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Source.Engine.Objects;
using Source.Engine.SceneGraph;

namespace Source.Game.Objects
{
    class Spikes : GameObject3D
    {
        private GameModel _model;

        public override void Initialize()
        {
            _model = new GameModel("Models/Spikes");
            _model.CreateBoundingBox(30, 20, 30, new Vector3(0, 10, 0));
            _model.Translate(0, Path.GROUND_POS, -100);
            _model.Scale(new Vector3(1.5f));
            //_model.DrawBoundingBox = true;
            AddChild(_model);

            base.Initialize();
        }
    }
}
