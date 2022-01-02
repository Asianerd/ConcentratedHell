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

            Main.UpdateEvent += Update;
            Main.DrawEvent += Draw;
        }

        public static void Update()
        {
            foreach(Tile x in map)
            {
                x.Update();
            }
        }

        public static void Draw()
        {
            foreach (Tile x in map)
            {
                x.Draw();
            }
        }

        #region Position validation
        // Split into many overloads to increase performance

        public static bool IsValidPosition(Point position)
        {
            foreach (Tile x in map)
            {
                if (x.rect.Contains(position))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidPosition(Vector2 position)
        {
            foreach (Tile x in map)
            {
                if (x.rect.Contains(position))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidPosition(Rectangle rectangle)
        {
            foreach (Tile x in map)
            {
                if (x.rect.Intersects(rectangle))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
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

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
        }
    }

    class Door:Tile
    {
        bool activated = false;
        GameValue progress;
        Vector2 start;
        Vector2 destination;

        static float activationDistance = 300f;

        public Door(Rectangle _rect, Vector2 _start, Vector2 _destination, GameValue _progress = null, Texture2D _sprite = null) : base(_rect, _sprite)
        {
            destination = _destination;
            start = _start;
            if (progress == null)
            {
                progress = new GameValue(0, 15, 1);
            }
            else
            {
                progress = _progress;
            }
            // Progress will regenerate when its open
            // Open = 100%
            // Closed = 0%
        }

        public override void Update()
        {
            float distance = Vector2.Distance(start, Player.Instance.rect.Center.ToVector2()); // bruh why cant point class have these things

            activated = distance <= activationDistance;

            if(activated)
            {
                progress.Regenerate(Universe.speedMultiplier);
            }
            else
            {
                progress.Regenerate(-1 * Universe.speedMultiplier);
            }

            /*Vector2 candidate = Vector2.Lerp(start, destination, (float)progress.Percent());
            rect.Location = candidate.ToPoint();*/
            rect.Location = Vector2.Lerp(start, destination, (float)progress.Percent()).ToPoint();
        }
    }
}
