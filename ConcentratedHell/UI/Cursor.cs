using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Cursor
    {
        public static Cursor Instance;
        public static Texture2D sprite;

        public static void Initialize()
        {
            Instance = new Cursor();
        }

        public static void LoadContent(Texture2D _sprite)
        {
            sprite = _sprite;
        }

        public Vector2 screenPosition;
        public Vector2 worldPosition;
        public float playerToCursor;

        public void Update()
        {
            screenPosition = Main.mouseState.Position.ToVector2();
            worldPosition = Camera.ScreenToWorld(screenPosition);
            playerToCursor = MathF.Atan2(
                worldPosition.Y - Player.Instance.rect.Center.Y,
                worldPosition.X - Player.Instance.rect.Center.X
                );
        }

        public void Draw()
        {
            Main.spriteBatch.Draw(sprite, screenPosition, Color.White);
        }
    }
}
