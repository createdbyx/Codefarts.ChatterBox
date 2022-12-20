using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codefarts.ChatterBox
{
    public class DefaultBoxRenderer : IChatBoxRenderer, IDisposable
    {
        private SpriteBatch spriteBatch;

        private Texture2D texture;

        //  private StencilHelper stencil;
        public Matrix WorldTransform { get; set; }
        DepthStencilState stencilAlways;
        DepthStencilState stencilKeepIfZero;
        AlphaTestEffect testFX;
        BlendState colorChannels;


        public DefaultBoxRenderer(ChatBoxComponent cbc)
        {
            if (cbc == null) throw new ArgumentNullException("cbc");
            this.spriteBatch = new SpriteBatch(cbc.GraphicsDevice);
            // this.stencil = new StencilHelper(cbc.GraphicsDevice);
            this.texture = new Texture2D(cbc.GraphicsDevice, 2, 2);
            this.texture.SetData(new[] { Color.White, Color.White, Color.White, Color.White });
            this.WorldTransform = Matrix.Identity;

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
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, this.spriteBatch.GraphicsDevice.Viewport.Width,
                                                                   this.spriteBatch.GraphicsDevice.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            testFX.View = Matrix.Identity;
            testFX.World = this.WorldTransform;
            testFX.Projection = halfPixelOffset * projection;

            // set up stencil state to pass if the stencil value is 0
            stencilKeepIfZero = new DepthStencilState();
            stencilKeepIfZero.StencilEnable = true;
            stencilKeepIfZero.StencilFunction = CompareFunction.Equal;
            stencilKeepIfZero.StencilPass = StencilOperation.Keep;
            stencilKeepIfZero.ReferenceStencil = 1;
        }

        public void Render(ChatBox chatBox, GameTime gameTime)
        {
            if (chatBox.Size.X <= 0 || chatBox.Size.Y <= 0) return;

            this.spriteBatch.Begin();

            // draw background
            Rectangle chatBoxRectangle = chatBox.Rectangle;
            var srcRect = new Rectangle(0, 0, 2, 2);
            this.spriteBatch.Draw(this.texture, chatBoxRectangle, srcRect, Color.Black);
            chatBoxRectangle.Inflate(-4, -4);
            this.spriteBatch.Draw(this.texture, chatBoxRectangle, srcRect, Color.White);

            // calculate destination rectangles for a image and some text
            Rectangle imgRect = Rectangle.Empty;
            Rectangle txtRect = Rectangle.Empty;

            // flush existing sprite data
            this.spriteBatch.End();

            // update textFX
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, this.spriteBatch.GraphicsDevice.Viewport.Width,
                                                                   this.spriteBatch.GraphicsDevice.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            testFX.World = this.WorldTransform;
            testFX.Projection = halfPixelOffset * projection;

            // render stencil area
            this.spriteBatch.Begin(SpriteSortMode.Immediate, this.colorChannels, null, this.stencilAlways, null, this.testFX);
            this.spriteBatch.Draw(this.texture, chatBoxRectangle, new Rectangle(0, 0, 2, 2), Color.White);
            this.spriteBatch.End();

            // begin rendering again
            this.spriteBatch.Begin();

            // check if we are using
            ImageData data = chatBox.ImageData is ImageData ? (ImageData)chatBox.ImageData : new ImageData();
            if (data.Texture != null && data.SourceRectangle != Rectangle.Empty)
            {
                imgRect = data.SourceRectangle;
                imgRect = this.CalculateImageAlignmentRectangle(chatBox, imgRect);
                // imgRect.X += chatBoxRectangle.X;
                // imgRect.Y += chatBoxRectangle.Y;

                this.spriteBatch.Draw(data.Texture, imgRect, data.SourceRectangle, Color.White);
            }

            // draw Text
            if (chatBox.Font != null)
            {
                var txtSize = chatBox.Font.MeasureString(chatBox.Text);
                txtRect.Location = new Point((int)chatBox.Position.X, (int)chatBox.Position.Y);
                txtRect.Width = (int)txtSize.X;
                txtRect.Height = (int)txtSize.Y;

                if (imgRect != Rectangle.Empty)
                {
                    if ((chatBox.ImageAlignment & Alignment.Left) == Alignment.Left) txtRect.X = imgRect.Right;
                    if ((chatBox.ImageAlignment & Alignment.Right) == Alignment.Right) txtRect.X = (int)chatBox.Position.X;
                    txtRect.Y = chatBoxRectangle.Y;
                }

                txtRect = Rectangle.Intersect(chatBoxRectangle, txtRect);

                // the commented line of code below was for debugging purposes
                //this.spriteBatch.Draw(this.texture, txtRect, new Rectangle(0, 0, 2, 2), Color.Green);

                // draw the text (it will be cropped by the stencil)
                //  this.spriteBatch.DrawString(chatBox.Font, chatBox.Text, new Vector2(txtRect.Location.X, txtRect.Location.Y), Color.Black);

                this.DrawTextWithWrapping(chatBox.Text, chatBox.Font, txtRect);
            }

            // end again to flush out the text
            this.spriteBatch.End();
        }

        /// <remarks>ChatGPT3.5 generated method with minor tweaks.</remarks>
        void DrawTextWithWrapping(string text, SpriteFont font, Rectangle rect)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            // Split the text into lines that fit within the rectangle
            List<string> lines = new List<string>();
            int lineStartIndex = 0;
            while (lineStartIndex < text.Length)
            {
                // Find the index of the last character that fits within the rectangle
                int lineEndIndex = lineStartIndex;
                Vector2 size = Vector2.Zero;
                while (lineEndIndex < text.Length)
                {
                    size = font.MeasureString(text.Substring(lineStartIndex, lineEndIndex - lineStartIndex + 1));
                    if (size.X > rect.Width)
                    {
                        break;
                    }

                    lineEndIndex++;
                }

                // If no characters fit, draw the first character and move on to the next line
                if (lineStartIndex == lineEndIndex)
                {
                    lines.Add(text[lineStartIndex].ToString());
                    lineStartIndex++;
                }
                else
                {
                    // If the last character that fits is not the last character in the line, find the last space within the rectangle
                    int lastSpaceIndex = text.LastIndexOf(' ', lineStartIndex, lineEndIndex - lineStartIndex);
                    if (lastSpaceIndex > lineStartIndex)
                    {
                        // If a space was found, add the line up to the space and move the start index to after the space
                        lines.Add(text.Substring(lineStartIndex, lastSpaceIndex - lineStartIndex));
                        lineStartIndex = lastSpaceIndex + 1;
                    }
                    else
                    {
                        // If no space was found, add the line up to the last character that fits and move the start index to after that character
                        lines.Add(text.Substring(lineStartIndex, lineEndIndex - lineStartIndex));
                        lineStartIndex = lineEndIndex;
                    }
                }
            }

// Begin the SpriteBatch
            //  spriteBatch.Begin();

// Calculate the Y position for the first line of text
            float y = rect.Y;

// Draw each line of text
            foreach (string line in lines)
            {
                // Calculate the X position for the line of text
                float x = rect.X;
                // if (align == TextAlignment.Center)
                // {
                //     x += (rect.Width - font.MeasureString(line).X) / 2;
                // }
                // else if (align == TextAlignment.Right)
                // {
                //     x += rect.Width - font.MeasureString(line).X;
                // }

                // Draw the line of text
                spriteBatch.DrawString(font, line, new Vector2(x, y), Color.Black);

                // Increment the Y position for the next line of text
                y += font.LineSpacing;
            }
        }

        internal Rectangle CalculateImageAlignmentRectangle(ChatBox chatBox, Rectangle imgRect)
        {
            switch (chatBox.ImageAlignment)
            {
                case Alignment.Top:
                    imgRect.X = (int)((chatBox.Size.X / 2f) - (imgRect.Width / 2f));
                    imgRect.Y = (int)(chatBox.Position.Y + 4f);
                    break;
                case Alignment.Left:
                    imgRect.X = (int)(chatBox.Position.X + 4f);
                    imgRect.Y = (int)((chatBox.Size.Y / 2f) - (imgRect.Height / 2f));
                    break;
                case Alignment.Center:
                    imgRect.X = (int)((chatBox.Size.X / 2f) - (imgRect.Width / 2f));
                    imgRect.Y = (int)((chatBox.Size.Y / 2f) - (imgRect.Height / 2f));
                    break;
                case Alignment.Right:
                    imgRect.X = (int)(chatBox.Position.X + chatBox.Size.X - (imgRect.Width + 2));
                    imgRect.Y = (int)((chatBox.Size.Y / 2f) - (imgRect.Height / 2f));
                    break;
                case Alignment.Bottom:
                    imgRect.X = (int)((chatBox.Size.X / 2f) - (imgRect.Width / 2f));
                    imgRect.Y = (int)(chatBox.Position.Y + chatBox.Size.Y - (imgRect.Height + 2));
                    break;
                case Alignment.TopLeft:
                    imgRect.Location = new Point((int)(chatBox.Position.X + 2), (int)(chatBox.Position.Y + 2));
                    break;
                case Alignment.TopRight:
                    imgRect.X = (int)(chatBox.Position.X + chatBox.Size.X - (imgRect.Width + 2));
                    imgRect.Y = (int)(chatBox.Position.Y + 2);
                    break;
                case Alignment.BottomLeft:
                    imgRect.X = (int)(chatBox.Position.X + 2);
                    imgRect.Y = (int)(chatBox.Position.Y + chatBox.Size.Y - (imgRect.Height + 2));
                    break;
                case Alignment.BottomRight:
                    imgRect.X = (int)(chatBox.Position.X + chatBox.Size.X - (imgRect.Width + 2));
                    imgRect.Y = (int)(chatBox.Position.Y + chatBox.Size.Y - (imgRect.Height + 2));
                    break;
            }

            return imgRect;
        }

        public void End(GameTime gameTime)
        {
            // this.spriteBatch.End();
        }

        public void Begin(GameTime gameTime)
        {
            //   this.spriteBatch.Begin(SpriteSortMode.Deferred, SpriteBlendMode.AlphaBlend, SaveStateMode.SaveState, this.WorldTransform);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.texture.Dispose();
            this.texture = null;
            this.spriteBatch.Dispose();
            this.spriteBatch = null;
        }
    }
}