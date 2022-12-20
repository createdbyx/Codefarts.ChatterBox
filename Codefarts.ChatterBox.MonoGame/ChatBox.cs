using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    [Flags]
    public enum Alignment
    {
        Top = 1,
        Left = 2,
        Center = 4,
        Right = 8,
        Bottom = 16,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }

    public sealed class ChatBox 
    {
        internal TimeSpan Duration { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public string Text { get; internal set; }
        internal SpriteFont Font { get; set; }
        internal TimeSpan RemovalTime { get; set; }
        internal string ID { get; set; }
        internal bool IsInitilized;
        public Alignment Alignment { get; internal set; }
        public Alignment ImageAlignment { get; internal set; }
        public Vector2 TextPosition { get; internal set; }
        public Vector2 TextSize { get; internal set; }
        public Vector2 ImagePosition { get; internal set; }
        public Vector2 ImageSize { get; internal set; }
        public object ImageData { get; internal set; }


        public Rectangle Rectangle
        {
            get { return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Size.X, (int)this.Size.Y); }
            set
            {
                this.Position = new Vector2(value.X, value.Y);
                this.Size = new Vector2(value.Width, value.Height);
            }
        }

        internal ChatBox()
        {

        }
    }
}