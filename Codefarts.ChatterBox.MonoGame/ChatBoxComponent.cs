using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public partial class ChatBoxComponent : DrawableGameComponent
    {

        private List<ChatBox> chatBoxes;
        public SpriteFont DefaultFont { get; set; }
        private Dictionary<string, ChatBoxValues> updatedValues;
        private Dictionary<string, ChatBox> uniqueChatBoxes;
        ChatBoxEventsArgs cbArgs = new ChatBoxEventsArgs();

        public EventHandler<ChatBoxEventsArgs> LayoutChatBox { get; set; }
        public IChatBoxRenderer ChatBoxRenderer { get; set; }


        public void Clear()
        {
            this.chatBoxes.Clear();
        }

        public ChatBox GetChatBoxData(int index)
        {
            return this.chatBoxes[index];
        }

        public void SetPosition(string id, Vector2 position)
        {
            if (this.updatedValues.ContainsKey(id))
            {
                var value = this.updatedValues[id];
                value.Position = position;
                this.updatedValues[id] = value;
            }
            else
            {
                this.updatedValues.Add(id, new ChatBoxValues() { Position = position });
            }
        }

        public void SetSize(string id, Vector2 size)
        {
            if (this.updatedValues.ContainsKey(id))
            {
                var value = this.updatedValues[id];
                value.Size = size;
                this.updatedValues[id] = value;
            }
            else
            {
                this.updatedValues.Add(id, new ChatBoxValues() { Size = size });
            }
        }

        public int VisibleChatBoxCount
        {
            get { return this.chatBoxes.Count; }
        }

        public ChatBoxComponent(Game game)
            : base(game)
        {
            this.chatBoxes = new List<ChatBox>();
            this.updatedValues = new Dictionary<string, ChatBoxValues>();
            this.uniqueChatBoxes = new Dictionary<string, ChatBox>();
            this.LayoutChatBox = DefaultLayout.DefaultChatBoxLayout;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (this.ChatBoxRenderer == null) this.ChatBoxRenderer = new DefaultBoxRenderer(this);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // push boxes away from each other but try to keep in view
            int index = 0;
            while (index < this.chatBoxes.Count)
            {
                var item = this.chatBoxes[index++];

                if (!string.IsNullOrEmpty(item.ID) && this.updatedValues.ContainsKey(item.ID))
                {
                    var value = this.updatedValues[item.ID];
                    if (value.PositionChanged) item.Position = value.Position;
                    if (value.SizeChanged) item.Size = value.Size;
                    this.updatedValues.Remove(item.ID);
                }

                // layout chat boxes
                if (this.LayoutChatBox != null)
                {
                    int indexB = 0;
                    while (indexB < this.chatBoxes.Count)
                    {
                        var itemB = this.chatBoxes[indexB++];
                        this.cbArgs.BoxA = item;
                        this.cbArgs.BoxB = itemB;
                        this.cbArgs.GameTime = gameTime;
                        this.LayoutChatBox(this, this.cbArgs);
                    }
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (this.ChatBoxRenderer == null) return;

            this.ChatBoxRenderer.Begin(gameTime);
            foreach (var box in this.chatBoxes)
            {
                this.ChatBoxRenderer.Render(box, gameTime);
            }
            this.ChatBoxRenderer.End(gameTime);

            base.Draw(gameTime);
        }
    }
}