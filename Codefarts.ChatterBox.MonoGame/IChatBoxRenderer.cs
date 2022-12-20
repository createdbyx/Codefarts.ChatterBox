using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public interface IChatBoxRenderer
    {
        void Render(ChatBox chatBox, GameTime gameTime);
        void End(GameTime gameTime);
        void Begin(GameTime gameTime);
    }
}