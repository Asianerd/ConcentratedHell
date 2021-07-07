using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownArena
{
    class Rendering
    {
        public delegate void Render(SpriteBatch spriteBatch);
        public static event Render DrawPlayer;
        public static event Render DrawEntities;

        public static SpriteBatch PlayerSpriteBatch;
        public static SpriteBatch EntitySpriteBatch;

        public static void RenderObjects()
        {
            if (DrawEntities != null)
            {
                DrawEntities(PlayerSpriteBatch);
            }
            if (DrawPlayer != null)
            {
                DrawPlayer(EntitySpriteBatch);
            }
        }
    }
}
