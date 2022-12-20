using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public partial class ChatBoxComponent : DrawableGameComponent
    {

        public void CreateChatBox(string text, Vector2 position, Vector2 size, TimeSpan duration)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, duration, null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, string id)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, TimeSpan duration, string id)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, duration, id, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, TimeSpan duration)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, duration, null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, string id)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, TimeSpan duration, string id)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, duration, id, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment,Alignment imageAlignment, TimeSpan duration)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, duration, null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment,Alignment imageAlignment)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, string id)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, TimeSpan duration, string id)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, duration, id,null);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, TimeSpan duration, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, duration, null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, string id, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, TimeSpan duration, string id, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, Alignment.TopLeft, Alignment.TopLeft, duration, id, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, TimeSpan duration, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, duration, null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, string id, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, TimeSpan duration, string id, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, Alignment.TopLeft, duration, id, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, TimeSpan duration, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, duration, null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, string id, object imagedata)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id, imagedata);
        }

        public void CreateChatBox(string text, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, TimeSpan duration, string id, object imagedata)
        {
            this.CreateChatBox(text, this.DefaultFont, position, size, textAlignment, imageAlignment, duration, id, imagedata);
        }

        public void CreateChatBox(string text, SpriteFont font, Vector2 position, Vector2 size, Alignment textAlignment, Alignment imageAlignment, TimeSpan duration, string id, object imageData)
        {
            if (string.IsNullOrEmpty(text)) return;
                       
            var item = new ChatBox();
            item.Font = font;
            item.Alignment = textAlignment;
            item.ImageAlignment = imageAlignment;
            item.Position = position;
            item.Size = size;
            item.Text = text;
            item.Duration = duration;
            item.ID = id;
            item.RemovalTime = TimeSpan.MinValue;
            item.ImageData = imageData;

            // check if item with that id is already present and if so remove any prev instances before adding
            if (!string.IsNullOrEmpty(id))
            {
                if (this.uniqueChatBoxes.ContainsKey(id))
                {
                    var uniqueBox = this.uniqueChatBoxes[id];
                    this.uniqueChatBoxes.Remove(id);
                    this.updatedValues.Remove(id);
                    this.chatBoxes.Remove(uniqueBox);
                }
                this.uniqueChatBoxes.Add(id, item);
            }
            this.chatBoxes.Add(item);
        }

    }
}