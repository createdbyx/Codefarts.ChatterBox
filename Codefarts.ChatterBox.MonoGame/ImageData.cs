using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    public struct ImageData
    {
        public ImageData(Texture2D texture, Rectangle sourceRectangle)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
        }

        public Texture2D Texture;
        public Rectangle SourceRectangle;
    }
}