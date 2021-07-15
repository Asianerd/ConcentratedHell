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
        public static event Render DrawCursor;
        public static event Render DrawItems;

        public static SpriteBatch PlayerSpriteBatch;
        public static SpriteBatch EntitySpriteBatch;
        public static SpriteBatch CursorSpriteBatch;
        public static SpriteBatch ItemSpriteBatch;

        public static int SizingScale = 64;
        /* So that sprites are rendered to the proper size
         */

        public static void Initialize(SpriteBatch _playerSpriteBatch, SpriteBatch _entitySpriteBatch, SpriteBatch _cursorSpriteBatch, SpriteBatch _itemSpriteBatch)
        {
            PlayerSpriteBatch = _playerSpriteBatch;
            EntitySpriteBatch = _entitySpriteBatch;
            CursorSpriteBatch = _cursorSpriteBatch;
            ItemSpriteBatch = _itemSpriteBatch;
        }

        public static void RenderObjects()
        {
            PlayerSpriteBatch.Begin();
            EntitySpriteBatch.Begin();
            ItemSpriteBatch.Begin();
            if (DrawEntities != null)
            {
                DrawEntities(PlayerSpriteBatch);
            }
            if (DrawItems != null)
            {
                DrawItems(ItemSpriteBatch);
            }
            if (DrawPlayer != null)
            {
                DrawPlayer(EntitySpriteBatch);
            }
            ItemSpriteBatch.End();
            PlayerSpriteBatch.End();
            EntitySpriteBatch.End();
        }

        public static void RenderCursor()
        {
            CursorSpriteBatch.Begin();
            if (CursorSpriteBatch != null)
            {
                DrawCursor(CursorSpriteBatch);
            }
            CursorSpriteBatch.End();
        }
    }
}
