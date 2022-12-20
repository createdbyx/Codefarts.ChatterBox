using Microsoft.Xna.Framework;

namespace Codefarts.ChatterBox
{
    public static class DefaultLayout
    {
        public static void DefaultChatBubbleLayout(object sender, ChatBubbleEventsArgs e)
        {
                     // restrict anchor
            RestrictChatBubbleAnchorLength(e.BubbleA, e.GameTime);

            // restrict box overlapping
            if (e.BubbleA != e.BubbleB) KeepChatBubbleBoxesSeparated(e.BubbleA, e.BubbleB, e.GameTime);
   }

        internal static void RestrictChatBubbleAnchorLength(ChatBubble bubble, GameTime gameTime)
        {
            var halfSize = (bubble.Size / 2);
            var centerA = bubble.Position + halfSize;
            var centerToAnchorLength = (centerA - bubble.AnchorPosition).Length();
               var centerToPositionLength = (centerA - bubble.Position).Length();

               if (centerToAnchorLength < centerToPositionLength + 5)
               {
                   var vector = Vector2.Lerp(bubble.AnchorPosition, centerA, 1.005f);
                   bubble.Position += (vector - centerA) * gameTime.ElapsedGameTime.Milliseconds;
               }
                 return;
               if (centerToAnchorLength > centerToPositionLength + 25)
               {
                   var vector = centerA - bubble.AnchorPosition;
                   var length = vector.Length();
                   vector.Normalize();
                   vector *= length + -(0.2f * gameTime.ElapsedGameTime.Milliseconds);
                   bubble.Position = bubble.AnchorPosition + vector + (bubble.Position - centerA);
               }
        }

        internal static void KeepChatBubbleBoxesSeparated(ChatBubble bubbleA, ChatBubble bubbleB, GameTime gameTime)
        {
            // push chat windows away from each other if they intersect
            var rectA = bubbleA.Rectangle;
            var rectB = bubbleB.Rectangle;
            rectA.Inflate(10, 10);
            rectB.Inflate(10, 10);
            if (Rectangle.Intersect(rectA, rectB) != Rectangle.Empty)
            {
                var vector = bubbleB.Position - bubbleA.Position;
                vector.Normalize();
                vector *= 0.25f * gameTime.ElapsedGameTime.Milliseconds;

                bubbleA.Position += -vector;
                bubbleB.Position += vector;
            }
        }

        public static void DefaultChatBoxLayout(object sender, ChatBoxEventsArgs e)
        {
            if (e.BoxA != e.BoxB)
            {
                if (Rectangle.Intersect(e.BoxA.Rectangle, e.BoxB.Rectangle) == Rectangle.Empty) return;
                Vector2 dist = Vector2.UnitY * 0.5f * e.GameTime.ElapsedGameTime.Milliseconds;
                e.BoxB.Position += dist / 2f;
                e.BoxA.Position += -(dist / 2f);
            }
        }
    }
}
