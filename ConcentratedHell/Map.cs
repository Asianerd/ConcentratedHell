using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Map
    {
        public static List<Tile> map;
        public static Texture2D placeholderSprite;

        public static void Initialize(List<Tile> _map)
        {
            map = _map;

            Main.DrawEvent += Draw;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile x in map)
            {
                x.Draw(spriteBatch);
            }
        }

        public static bool IsValidPosition(Point position)
        {
            foreach(Tile x in map)
            {
                if(x.rect.Contains(position))
                {
                    return false;
                }
            }
            return true;
        }
    }

    class Tile
    {
        public Rectangle rect;
        public Texture2D sprite;
        public Vector2 spriteOrigin;

        public Tile(Rectangle _rect, Texture2D _sprite=null)
        {
            rect = _rect;
            if (_sprite == null)
            {
                sprite = Map.placeholderSprite;
            }
            else
            {
                sprite = _sprite;
            }
            spriteOrigin = sprite.Bounds.Center.ToVector2();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(sprite, rect, Color.White);
            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //spriteBatch.Draw(sprite, rect, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
