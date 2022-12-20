using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public struct ChatBoxValues
    {
        private Vector2 position;
        private Vector2 size;
        internal bool PositionChanged;
        internal bool SizeChanged;


        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                this.PositionChanged = true;
            }
        }

        public Vector2 Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                this.SizeChanged = true;
            }
        }
    }
}