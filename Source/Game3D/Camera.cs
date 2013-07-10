using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game3D
{
    class Camera : GameObject3D
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        public Camera()
        {
            Projection = Matrix.CreateOrthographic(800, 480, 0.1f, 300);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var lookAt = Vector3.Transform(Vector3.Forward, Rotation);
            lookAt.Normalize();

            View = Matrix.CreateLookAt(Position, (Position + lookAt), Vector3.Up);
        }
    }
}