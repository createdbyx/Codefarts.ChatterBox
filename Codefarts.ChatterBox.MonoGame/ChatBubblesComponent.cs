using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Codefarts.ChatterBox
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ChatBubblesComponent : DrawableGameComponent
    {

        private List<ChatBubble> chatBubbles;
        public Vector2 BorderSize { get; set; }
        public SpriteFont DefaultFont { get; set; }
        private Dictionary<string, Vector2> updatedPositions;
        private Dictionary<string, ChatBubble> uniqueChatBoxes;
        ChatBubbleEventsArgs cbArgs = new ChatBubbleEventsArgs();

        public EventHandler<ChatBubbleEventsArgs> LayoutChatBubble { get; set; }
        public IChatBubbleRenderer ChatBubbleRenderer { get; set; }


        public void Clear()
        {
            this.chatBubbles.Clear();
        }

        public void CreateChatBubble(string text, Vector2 position, TimeSpan duration)
        {
            this.CreateChatBubble(text, this.DefaultFont, position, duration, null);
        }

        public void CreateChatBubble(string text, Vector2 position)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBubble(text, this.DefaultFont, position, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), null);
        }

        public void CreateChatBubble(string text, Vector2 position, string id)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.CreateChatBubble(text, this.DefaultFont, position, TimeSpan.FromSeconds(text.Split(new[] { ' ' }).Length), id);
        }

        public void CreateChatBubble(string text, Vector2 position, TimeSpan duration, string id)
        {
            this.CreateChatBubble(text, this.DefaultFont, position, duration, id);
        }

        public void CreateChatBubble(string text, SpriteFont font, Vector2 position, TimeSpan duration, string id)
        {
            if (string.IsNullOrEmpty(text)) return;

            var item = new ChatBubble();
            item.Font = font;
            item.Text = text;
            item.AnchorPosition = position;
            item.Duration = duration;
            item.ID = id;
            item.RemovalTime = TimeSpan.MinValue;

            // check if item with that id is already present and if so remove any previous instances before adding
            if (!string.IsNullOrEmpty(id))
            {
                if (this.uniqueChatBoxes.ContainsKey(id))
                {
                    var uniqueBox = this.uniqueChatBoxes[id];
                    this.uniqueChatBoxes.Remove(id);
                    this.updatedPositions.Remove(id);
                    this.chatBubbles.Remove(uniqueBox);
                }
                this.uniqueChatBoxes.Add(id, item);
            }
            this.chatBubbles.Add(item);
        }

        public void SetPosition(string id, Vector2 position)
        {
            if (this.updatedPositions.ContainsKey(id))
            {
                this.updatedPositions[id] = position;
            }
            else
            {
                this.updatedPositions.Add(id, position);
            }
        }

        public int VisibleChatBubbleCount
        {
            get { return this.chatBubbles.Count; }
        }

        public ChatBubblesComponent(Game game)
            : base(game)
        {
            this.chatBubbles = new List<ChatBubble>();
            this.updatedPositions = new Dictionary<string, Vector2>();
            this.uniqueChatBoxes = new Dictionary<string, ChatBubble>();
            this.LayoutChatBubble = DefaultLayout.DefaultChatBubbleLayout;
            this.BorderSize = Vector2.One * 10;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (this.ChatBubbleRenderer == null) this.ChatBubbleRenderer = new DefaultBubbleRenderer(this);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // push boxes away from each other but try to keep in view
            int indexA = 0;
            while (indexA < this.chatBubbles.Count)
            {
                var chatBubbleA = this.chatBubbles[indexA++];


                if (!string.IsNullOrEmpty(chatBubbleA.ID) && this.updatedPositions.ContainsKey(chatBubbleA.ID))
                {
                    var value = this.updatedPositions[chatBubbleA.ID];
                    chatBubbleA.AnchorPosition = value;
                    this.updatedPositions.Remove(chatBubbleA.ID);
                }

                // check if size has been set
                if (!chatBubbleA.IsInitilized)
                {
                    // setup size
                    var font = chatBubbleA.Font;
                    font = font ?? this.DefaultFont;
                    if (font != null)
                    {
                        chatBubbleA.Size = font.MeasureString(chatBubbleA.Text);
                        chatBubbleA.Size += Vector2.One * this.BorderSize;
                        chatBubbleA.Position = chatBubbleA.AnchorPosition - (chatBubbleA.Size / 2) + (-Vector2.UnitY * 30);
                    }
                    chatBubbleA.RemovalTime = gameTime.TotalGameTime + chatBubbleA.Duration;
                    chatBubbleA.IsInitilized = true;
                }

                // remove the chat box if the display time is over
                if (gameTime.TotalGameTime > chatBubbleA.RemovalTime) this.chatBubbles.Remove(chatBubbleA);

                var indexB = 0;
                while (indexB < this.chatBubbles.Count)
                {
                    var chatBubbleB = this.chatBubbles[indexB++];

                    if (this.LayoutChatBubble != null)
                    {
                        this.cbArgs.BubbleA = chatBubbleA;
                        this.cbArgs.BubbleB = chatBubbleB;
                        this.cbArgs.GameTime = gameTime;

                        this.LayoutChatBubble(this, this.cbArgs);
                    }
                }
            }
        }


        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (this.ChatBubbleRenderer != null) this.ChatBubbleRenderer.Draw(gameTime, this.chatBubbles);
        }

    }
}