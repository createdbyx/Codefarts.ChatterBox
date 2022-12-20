using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public interface IChatBubbleRenderer
    {
        void Draw(GameTime gameTime, List<ChatBubble> bubbles);
    }
}