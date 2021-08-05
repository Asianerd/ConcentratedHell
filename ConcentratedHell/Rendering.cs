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

        public static SpriteBatch MainSpriteBatch;

        public static int SizingScale = 64;
        /* So that sprites are rendered to the proper size */
        public static int FontWidth = 12;
        /* Font width in pixels */
        public static Vector3 ScreenOffset = Vector3.Zero;
        public static GameValue ScreenShakeValue;
        public static int ScreenShakeIntensity;

        #region Attributes
        public static Texture2D Background;
        public static Vector2 BackgroundOrigin;
        public static Texture2D BackgroundWalls;
        public static Vector2 BackgroundWallsOrigin;
        public static Vector3 CameraPosition;
        #endregion

        #region Rendering
        public static void Initialize(SpriteBatch _mainSpriteBatch, Texture2D _background, Texture2D _backgroundWalls)
        {
            Main.PlayerUpdateEvent += Update;

            MainSpriteBatch = _mainSpriteBatch;
            Background = _background;
            BackgroundWalls = _backgroundWalls;

            BackgroundOrigin = new Vector2(960, 540);
            BackgroundWallsOrigin = new Vector2(1048, 627);

            ScreenShakeValue = new GameValue("Screenshake", 0, 25, 1);
        }

        public static void RenderObjects()
        {
            /* Drawing order:
             * 
             * Background
             * Entities
             * Items
             * Player
             * UI
             * Cursor
             */
            CameraPosition = new Vector3((Main.screenSize / 2) - Player.Instance.Position, 0f) + ScreenOffset;
            //_cameraPosition = new Vector3(Math.Clamp(_cameraPosition.X, -1920, 1920f), Math.Clamp(_cameraPosition.Y, -1080, 1080), 0f);
            Matrix _renderedOffset = Matrix.CreateTranslation(CameraPosition);
            MainSpriteBatch.Begin(transformMatrix:_renderedOffset); // Drawn in real world
            MainSpriteBatch.Draw(Background, Vector2.Zero, null, Color.White, 0f, BackgroundOrigin, 2f, SpriteEffects.None, 0f);
            MainSpriteBatch.Draw(BackgroundWalls, Vector2.Zero, null, Color.White, 0f, BackgroundWallsOrigin, 2f, SpriteEffects.None, 0f);
            if (DrawEntities != null)
            {
                DrawEntities(MainSpriteBatch);
            }
            if (DrawPlayer != null)
            {
                DrawPlayer(MainSpriteBatch);
            }
            if (DrawItems != null)
            {
                DrawItems(MainSpriteBatch);
            }
            MainSpriteBatch.End();
            MainSpriteBatch.Begin(); // Drawn in camera world
            UI.Instance.Draw(MainSpriteBatch);
            RenderCursor();
            MainSpriteBatch.End();
        }

        public static void RenderCursor()
        {
            if (DrawCursor != null)
            {
                DrawCursor(MainSpriteBatch);
            }
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
