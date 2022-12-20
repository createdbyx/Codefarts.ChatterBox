//#define DEBUGTEST   // comment this out to hide debugging graphics

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    public class DefaultBubbleRenderer : IChatBubbleRenderer
    {
        private ChatBubblesComponent chatBubbleComponent;

        private SpriteBatch spriteBatch;
        private Texture2D texture;
        DepthStencilState stencilAlways;
        DepthStencilState stencilKeepIfZero;
        AlphaTestEffect testFX;
        BlendState colorChannels;


        public DefaultBubbleRenderer(ChatBubblesComponent cbc)
        {
            if (cbc == null) throw new ArgumentNullException("cbc");
            this.chatBubbleComponent = cbc;
            this.spriteBatch = new SpriteBatch(cbc.GraphicsDevice);
            this.texture = new Texture2D(this.spriteBatch.GraphicsDevice, 128, 128, false, SurfaceFormat.Color);
            
            // build pixel data  here
            var index = 0;
            var data = new Color[texture.Width, texture.Height];

            for (int idx = 0; idx < texture.Width; idx++)
            {
                for (int idy = idx / 2; idy < texture.Height - (idx / 2); idy++)
                {
                    data[idx, idy] = Color.White;
                }
            }
            var stream = new Color[texture.Width * texture.Height];
            for (int idy = 0; idy < texture.Height; idy++)
            {
                for (int idx = 0; idx < texture.Width; idx++)
                {
                    stream[index++] = data[idx, idy];
                }
            }
            texture.SetData(stream);


            // set up stencil state to always replace stencil buffer with 1
            stencilAlways = new DepthStencilState();
            stencilAlways.StencilEnable = true;
            stencilAlways.StencilFunction = CompareFunction.Always;
            stencilAlways.StencilPass = StencilOperation.Replace;
            stencilAlways.ReferenceStencil = 1;

            this.colorChannels = new BlendState();
            this.colorChannels.ColorWriteChannels = ColorWriteChannels.None;

            testFX = new AlphaTestEffect(this.spriteBatch.GraphicsDevice);
            testFX.AlphaFunction = CompareFunction.Greater;
            testFX.ReferenceAlpha = 0;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, this.spriteBatch.GraphicsDevice.Viewport.Width, this.spriteBatch.GraphicsDevice.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            testFX.View = Matrix.Identity;
            testFX.World = Matrix.Identity;
            testFX.Projection = halfPixelOffset * projection;

            // set up stencil state to pass if the stencil value is 0
            stencilKeepIfZero = new DepthStencilState();
            stencilKeepIfZero.StencilEnable = true;
            stencilKeepIfZero.StencilFunction = CompareFunction.Equal;
            stencilKeepIfZero.StencilPass = StencilOperation.Keep;
            stencilKeepIfZero.ReferenceStencil = 1;
        }

        public void Draw(GameTime gameTime, List<ChatBubble> bubbles)
        {
            var enumerator = bubbles.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var chatBubble = enumerator.Current;

                var center = chatBubble.Position + (chatBubble.Size / 2);

                var angle = (float)Math.Atan2(center.Y - chatBubble.AnchorPosition.Y, center.X - chatBubble.AnchorPosition.X);

                var distance = 40f;
                if (chatBubble.Size.X < chatBubble.Size.Y) distance = distance > chatBubble.Size.X ? chatBubble.Size.X : distance;
                if (chatBubble.Size.X > chatBubble.Size.Y) distance = distance > chatBubble.Size.Y ? chatBubble.Size.Y : distance;

                distance = (center - chatBubble.AnchorPosition).Length();
                distance /= 2f;
                var stubA = new Vector2(center.X + ((float)Math.Cos(angle + -MathHelper.PiOver2) * distance),
                                        center.Y + ((float)Math.Sin(angle + -MathHelper.PiOver2) * distance));
                var stubB = new Vector2(center.X + ((float)Math.Cos(angle + MathHelper.PiOver2) * distance),
                                        center.Y + ((float)Math.Sin(angle + MathHelper.PiOver2) * distance));

                var rect = new Rectangle((int)chatBubble.Position.X, (int)chatBubble.Position.Y, (int)chatBubble.Size.X,
                                         (int)chatBubble.Size.Y);
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(this.texture, rect, new Rectangle(0, 0, 1, 1), Color.White);

                var dist = (stubB - stubA).Length();
                distance = distance > chatBubble.Size.Y ? chatBubble.Size.Y : distance;

                var scale = new Vector2(dist / this.texture.Width, distance / this.texture.Height);
                this.spriteBatch.Draw(this.texture, center, null, Color.White, angle + MathHelper.Pi,
                                      new Vector2(0, this.texture.Height / 2), scale, SpriteEffects.None, 0);

                var textPos = chatBubble.Position; // round the numbers to whole numbers
                textPos.X = (int)textPos.X;
                textPos.Y = (int)textPos.Y;
                var font = chatBubble.Font ?? this.chatBubbleComponent.DefaultFont;
                this.spriteBatch.DrawString(font, chatBubble.Text,
                                            chatBubble.Position + (Vector2.One * (this.chatBubbleComponent.BorderSize / 2f)),
                                            Color.Black);

#if DEBUGTEST
                this.DrawLine(this.spriteBatch, center, chatBubble.AnchorPosition, Color.Red);
                this.spriteBatch.DrawString(font, distance.ToString(), chatBubble.Position, Color.Green);
#endif
                this.spriteBatch.End();
            }
        }

#if DEBUGTEST
        //Calculates the distances and the angle and then draws a line
        private void DrawLine(SpriteBatch sprite, Vector2 start, Vector2 end, Color color)
        {
            var distance = Vector2.Distance(start, end);
            var rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            if (this.texture != null)
                sprite.Draw(this.texture, new Rectangle((int)start.X, (int)start.Y, (int)(distance < 1 ? 1 : distance), 1),
                     new Rectangle(0, 0, 1, 1), color, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }
#endif
    }
}