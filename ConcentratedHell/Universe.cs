using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    public static class Universe
    {
        public static Random RANDOM = new Random();
        public static int SCALE = 1;
        /* Scale
         * 1 = 1 scale : 1 pixel
         * 2 = 2 scale : 2 pixel
         * 3 = 3 scale : 3 pixel
         */

        public static float ANGLETO(Vector2 from, Vector2 to, bool inDegrees = true)
        {
            if (inDegrees)
            {
                return MathHelper.ToDegrees((float)Math.Atan2(to.Y - from.Y, to.X - from.X));
            }
            else
            {
                return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
            }
        }
    }
}
