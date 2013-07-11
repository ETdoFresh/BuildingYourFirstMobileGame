using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Source.Engine.Helper;

namespace Source.Engine
{
    class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
        public BaseCamera Camera { get; set; }
        public float GameSpeed { get; set; }
        public float InitialGameSpeed { get; set; }
    }
}
