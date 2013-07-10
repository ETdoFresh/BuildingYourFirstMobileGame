using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Source
{
    class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
    }
}
