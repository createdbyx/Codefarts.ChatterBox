using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public class ChatBubbleEventsArgs  : System.EventArgs
    {
        public GameTime GameTime;
        public ChatBubble BubbleA;
        public ChatBubble BubbleB;
    }
}
