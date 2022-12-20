using System;
using Codefarts.ChatterBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameExample.Components;

public class ChatBoxExampleComponent : DrawableGameComponent
{
    SpriteBatch spriteBatch;
    private ChatBoxComponent chat;
    private string[] chatMessages = new[] { "This is a chat box window.\r\n\r\nPress spacebar to continue ...",
        "This window has a image aligned to the top left\r\n\r\nPress spacebar to continue ...",
        "This window has a image aligned to the top right\r\n\r\nPress spacebar to continue ...",
        "This window has a image aligned to the bottom left\r\n\r\nPress spacebar to continue ...",
        "This window has a image aligned to the bottom right\r\n\r\nPress spacebar to continue ...",
        "This chatbox has no id\r\n\r\nPress spacebar to continue ...",
    };
    private Alignment[] txtAligns = new[] { Alignment.TopLeft, Alignment.TopLeft, Alignment.TopLeft, Alignment.TopLeft, Alignment.TopLeft, Alignment.TopLeft };
    private Alignment[] imgAligns = new[] { Alignment.TopLeft, Alignment.TopLeft, Alignment.TopRight, Alignment.BottomLeft, Alignment.BottomRight, Alignment.TopLeft };
    private ImageData[] imgDatas = new ImageData[6];
    private string[] boxIDs = new[] { "chatbox", "chatbox", "chatbox", "chatbox", "chatbox", null };
    private int messageIndex = 0;

    private KeyboardState prevKBState;
    //  private IUserInputService input;
    
    public ChatBoxExampleComponent(Game game, GraphicsDeviceManager graphics) : base(game)
    {
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        graphics.ApplyChanges();

        this.Game.IsMouseVisible = true;

        this.chat = new ChatBoxComponent(this.Game);
        this.Game.Components.Add(this.chat);

       // this.input = new UserInputService(this);
      //  this.Game.Components.Add(this.input as GameComponent);
    }

    public override void Initialize()
    {
        base.Initialize();
        // setup image data
        Texture2D tex = this.Game.Content.Load<Texture2D>("lompster");
        Rectangle srcRect = new Rectangle(0, 0, tex.Width, tex.Height);

        this.imgDatas[0] = new ImageData();
        this.imgDatas[1] = new ImageData(tex, srcRect);
        this.imgDatas[5] = this.imgDatas[4] = this.imgDatas[3] = this.imgDatas[2] = this.imgDatas[1];

        this.DoShowNextMessage();  
    }
    
      private void DoShowNextMessage()
        {
            var vp = this.GraphicsDevice.Viewport;

            this.chat.CreateChatBox(this.chatMessages[this.messageIndex],
                                    new Vector2(vp.X + 300, (vp.Y + vp.Height) - 200),
                                    new Vector2(vp.Width - 600, 150),
                                    this.txtAligns[this.messageIndex],
                                    this.imgAligns[this.messageIndex],
                                    TimeSpan.FromDays(1000),
                                    this.boxIDs[this.messageIndex],
                                    this.imgDatas[this.messageIndex]);
            this.messageIndex++;
            if (this.messageIndex > this.chatMessages.Length - 1) this.messageIndex = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.chat.DefaultFont = this.Game.Content.Load<SpriteFont>("CourierNew");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var mouseState = Mouse.GetState();
            var kbState = Keyboard.GetState();
            
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                this.Game.Exit();
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                this.chat.SetPosition("chatbox", mouseState.Position.ToVector2());
            }
            
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                this.chat.SetSize("chatbox", mouseState.Position.ToVector2() - this.chat.GetChatBoxData(0).Position);
            }

            if (kbState[Keys.Space] == KeyState.Up && this.prevKBState[Keys.Space] == KeyState.Down)
            {
                this.DoShowNextMessage();
            }
            
            this.prevKBState = kbState;
            // if (this.input.IsLeftMouseButtonDown)
            // {
            //     this.chat.SetPosition("chatbox", this.input.MousePosition);
            // }
            // if (this.input.IsRightMouseButtonDown)
            // {
            //     this.chat.SetSize("chatbox", this.input.MousePosition - this.chat.GetChatBoxData(0).Position);
            // }
            //
            // if (this.input.KeyClick(Keys.Space))
            // {
            //     this.DoShowNextMessage();
            // }
            //
            base.Update(gameTime);
        }

        
}