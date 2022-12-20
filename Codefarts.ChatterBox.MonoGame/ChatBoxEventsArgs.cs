using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public class ChatBoxEventsArgs  : System.EventArgs
    {
        public GameTime GameTime;
        public ChatBox BoxA;
        public ChatBox BoxB;
    }
}