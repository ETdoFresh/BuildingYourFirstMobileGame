using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game3D
{
    abstract class GameObject3D
    {
        public virtual Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        protected Matrix WorldMatrix;

        protected GameObject3D()
        {
            Scale = Vector3.One;
        }

        public virtual void Initialize() { }
        public virtual void LoadContent(ContentManager contentManager) { }

        public virtual void Update(RenderContext renderContext)
        {
            WorldMatrix = Matrix.CreateFromQuaternion(Rotation) *
                            Matrix.CreateScale(Scale) *
                            Matrix.CreateTranslation(Position);
        }

        public virtual void Draw(RenderContext renderContext) { }
    }
}