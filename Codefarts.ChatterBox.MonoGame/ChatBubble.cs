using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    public  sealed class ChatBubble
    {
        internal TimeSpan Duration { get; set; }
        public Vector2 Position { get; internal set; }
        public Vector2 AnchorPosition { get; internal set; }
        public Vector2 Size { get; internal set; }
        public string Text { get; internal set; }
        internal SpriteFont Font { get; set; }
        internal TimeSpan RemovalTime { get; set; }
        internal string ID { get; set; }
        internal bool IsInitilized;
        
        
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Size.X, (int)this.Size.Y); }
        }

        internal ChatBubble()
        {
            
        }
    }
}
