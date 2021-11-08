using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Player
    {
        public static Player Instance = null;
        public static Texture2D sprite;
        
        public static void Initialize()
        {
            if(Instance == null)
            {
                Instance = new Player();
            }
        }

        public static void LoadContent(Texture2D _sprite)
        {
            sprite = _sprite;
        }

        public Vector2 position;

        public void Update()
        {

        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(sprite, position, null, Color.White, 0f, sprite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);
        }
    }
}
