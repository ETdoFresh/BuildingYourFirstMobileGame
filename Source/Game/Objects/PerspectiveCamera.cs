using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Source.Engine.Helper;

namespace Source.Game.Objects
{
    class PerspectiveCamera: BaseCamera
    {
        public PerspectiveCamera()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 3.0f, 800f / 480f, 0.1f, 700);
        }
    }
}
