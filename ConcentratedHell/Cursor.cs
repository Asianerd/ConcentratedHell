using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Cursor
    {
        public static Cursor Instance;

        Vector2 Position;
        Vector2 Size = Vector2.One * 64;
        static Texture2D CursorSprite;

        public static void Initialize(Texture2D _cursorSprite)
        {
            if(Instance == null)
            {
                Instance = new Cursor();
                CursorSprite = _cursorSprite;
                Instance.__privateInit__();
            }
        }

        void __privateInit__()
        {
            Main.PlayerUpdateEvent += Move;
            Rendering.DrawCursor += Draw;
        }

        void Move()
        {
            Position = Mouse.GetState().Position.ToVector2();
        }

        void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                CursorSprite,
                Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
                );
        }
    }
}
