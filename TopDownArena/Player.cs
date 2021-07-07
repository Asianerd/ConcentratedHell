using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TopDownArena
{
    class Player
    {
        #region Initialization stuff
        public static Player Instance;

        public static void Initialize(Texture2D sprite)
        {
            Instance = new Player(Vector2.Zero);
            Sprite = sprite;
            Rendering.DrawPlayer += Instance.Draw;
        }
        #endregion


        #region Fields
        public Vector2 Position;
        public static Texture2D Sprite;
        #endregion

        Player(Vector2 position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
