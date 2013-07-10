using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.Game3D;

namespace Source
{
    class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
        public Camera Camera { get; set; }
    }
}
