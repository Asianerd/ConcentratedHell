using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        /* So that sprites are rendered to the proper size */
        public static Vector3 ScreenOffset = Vector3.Zero;
        public static GameValue ScreenShakeValue;
        public static int ScreenShakeIntensity;

        #region Rendering
        public static void Initialize(SpriteBatch _playerSpriteBatch, SpriteBatch _entitySpriteBatch, SpriteBatch _cursorSpriteBatch, SpriteBatch _itemSpriteBatch)
        {
            Main.PlayerUpdateEvent += Update;

            PlayerSpriteBatch = _playerSpriteBatch;
            EntitySpriteBatch = _entitySpriteBatch;
            CursorSpriteBatch = _cursorSpriteBatch;
            ItemSpriteBatch = _itemSpriteBatch;

            ScreenShakeValue = new GameValue("Screenshake", 0, 25, 1);
        }

        public static void RenderObjects()
        {
            Matrix _renderedOffset = Matrix.CreateTranslation(ScreenOffset);
            PlayerSpriteBatch.Begin(transformMatrix: _renderedOffset);
            EntitySpriteBatch.Begin(transformMatrix: _renderedOffset);
            ItemSpriteBatch.Begin(transformMatrix: _renderedOffset);
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
        #endregion


        #region Screen shaking
        public static void ShakeScreen(float _percent = 0.5f, int _intensity = 10)
        {
            ScreenShakeValue.AffectValue(_percent);
            ScreenShakeIntensity = _intensity;
        }
        #endregion

        #region Regular behaviour
        static void Update()
        {
            ScreenShakeValue.Regenerate();
            if(ScreenShakeValue.Percent() != 1)
            {
                ScreenOffset = new Vector3(Universe.RANDOM.Next(-ScreenShakeIntensity, ScreenShakeIntensity), Universe.RANDOM.Next(-ScreenShakeIntensity, ScreenShakeIntensity), 0);
            }
            else
            {
                ScreenOffset = Vector3.Zero;
            }
        }
        #endregion
    }
}
