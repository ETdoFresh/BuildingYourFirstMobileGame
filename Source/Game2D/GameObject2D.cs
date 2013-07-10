using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Game2D
{
    class GameObject2D
    {

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public bool CanDraw { get; set; }

        public GameObject2D()
        {
            Scale = Vector2.One;
            CanDraw = true;
        }

        public virtual void Initialize() { }
        public virtual void LoadContent(ContentManager contentManager) { }
        public virtual void Draw(RenderContext renderContext) { }
        public virtual void Update(RenderContext renderContext) { }
    }
}
