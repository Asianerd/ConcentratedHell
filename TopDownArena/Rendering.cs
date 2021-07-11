using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Rendering
    {
        public delegate void Render(SpriteBatch spriteBatch);
        public static event Render DrawPlayer;
        public static event Render DrawEntities;

        public static SpriteBatch PlayerSpriteBatch;
        public static SpriteBatch EntitySpriteBatch;

        public static int SizingScale = 64;
        /* So that sprites are rendered to the proper size
         */

        public static void Initialize(SpriteBatch _playerSpriteBatch, SpriteBatch _entitySpriteBatch)
        {
            PlayerSpriteBatch = _playerSpriteBatch;
            EntitySpriteBatch = _entitySpriteBatch;
        }

        public static void RenderObjects()
        {
            PlayerSpriteBatch.Begin();
            EntitySpriteBatch.Begin();
            if (DrawEntities != null)
            {
                DrawEntities(PlayerSpriteBatch);
            }
            if (DrawPlayer != null)
            {
                DrawPlayer(EntitySpriteBatch);
            }
            PlayerSpriteBatch.End();
            EntitySpriteBatch.End();
        }
    }
}
