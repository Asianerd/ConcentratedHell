using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Map
    {
        public static Map currentMap;
        public static Dictionary<Level, List<Tile>> tileSets;
        public static Dictionary<Level, Texture2D> mapSprites;
        public static Texture2D placeholderSprite;
        public static Level level;
        public static List<Texture2D> slices;

        public List<Tile> map;
        public Texture2D sprite;

        public static void Initialize()
        {
            InitializeTilesets();

            currentMap = new Map();
            currentMap.map = tileSets[Level.Default];
            mapSprites = new Dictionary<Level, Texture2D>();
            foreach (Level x in Enum.GetValues(typeof(Level)).Cast<Level>())
            {
                mapSprites[x] = Main.Instance.Content.Load<Texture2D>($"Map/sprites/{x.ToString().ToLower()}");
            }
            currentMap.sprite = mapSprites[level];

            slices = new List<Texture2D>();
            for (int i = 1; i <= 9; i++)
            {
                slices.Add(Main.Instance.Content.Load<Texture2D>($"Map/9slice/{i}"));
            }

            Main.UpdateEvent += Update;
            Main.MidgroundDrawEvent += Draw;
        }

        public static void InitializeTilesets()
        {
            tileSets = new Dictionary<Level, List<Tile>>()
            {
                { Level.Default, new List<Tile>()
                    {                    
                        //new Tile(new Rectangle(-1024, -512, 2048, 32)), // Up
                        /*new Tile(new Rectangle(-1024, -512, 32, 1024)), // Left
                        new Tile(new Rectangle(3024, -512, 2032, 1024)), // Right
                        new Tile(new Rectangle(-1024, 512, 2080, 32)), // Down*/

                        new Tile(new Rectangle(-3024, -2512, 6048, 2032)),  // Up
                        new Tile(new Rectangle(-3024, 512, 6048, 2032)),    // Down
                        new Tile(new Rectangle(1024, -480, 2032, 992)),     // Right
                        new Tile(new Rectangle(-3024, -480, 2032, 992)),    // Left

                        new Tile(new Rectangle(-760, -120, 240, 240)),
                        new Tile(new Rectangle(520, -120, 240, 240)),
                    }
                }
            };
        }

        public static void Update()
        {
            foreach(Tile x in currentMap.map)
            {
                x.Update();
            }
        }

        public static void AdvanceLevel(Level ?_type)
        {
            if (_type != null)
            {
                level = (Level)Math.Clamp((int)_type + 1, 0, Enum.GetValues(typeof(Level)).Cast<Level>().Count() - 1);
                currentMap.map = tileSets[level];
                currentMap.sprite = mapSprites[level];
            }
        }

        public static void Draw()
        {
            foreach (Tile x in currentMap.map)
            {
                x.Draw();
            }
            //Main.spriteBatch.Draw(currentMap.sprite, Vector2.Zero, null, Color.White, 0f, currentMap.sprite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);
        }

        #region Position validation
        // Split into many overloads to increase performance

        public static bool IsValidPosition(Point position)
        {
            foreach (Tile x in currentMap.map)
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
            foreach (Tile x in currentMap.map)
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
            foreach (Tile x in currentMap.map)
            {
                if (x.rect.Intersects(rectangle))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        public enum Level
        {
            Default
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

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            // 9-Slice drawing

            int pixel = 8;

            Main.spriteBatch.Draw(Map.slices[0], new Rectangle(rect.X, rect.Y, pixel, pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[1], new Rectangle(rect.X + pixel, rect.Y, rect.Width - pixel, pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[2], new Rectangle(rect.Right - pixel, rect.Y, pixel, pixel), Color.White);

            Main.spriteBatch.Draw(Map.slices[3], new Rectangle(rect.X, rect.Y + pixel, pixel, rect.Height - pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[4], new Rectangle(rect.X + pixel, rect.Y + pixel, rect.Width - pixel, rect.Height - pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[5], new Rectangle(rect.Right - pixel, rect.Y + pixel, pixel, rect.Height - pixel), Color.White);

            Main.spriteBatch.Draw(Map.slices[6], new Rectangle(rect.X, rect.Bottom - pixel, pixel, pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[7], new Rectangle(rect.X + pixel, rect.Bottom - pixel, rect.Width - pixel, pixel), Color.White);
            Main.spriteBatch.Draw(Map.slices[8], new Rectangle(rect.Right - pixel, rect.Bottom - pixel, pixel, pixel), Color.White);
        }
    }

    class Door:Tile
    {
        bool locked = false;
        bool activated = false;
        GameValue progress;
        Vector2 start;
        Vector2 destination;

        static float activationDistance = 300f;

        public Door(Rectangle _rect, Vector2 _start, Vector2 _destination, GameValue _progress = null, Texture2D _sprite = null, bool _locked = false) : base(_rect, _sprite)
        {
            locked = _locked;
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
            if (locked)
            {
                progress.AffectValue(0f);
            }
        }

        public override void Update()
        {
            float distance = Vector2.Distance(start, Player.Instance.rect.Center.ToVector2()); // bruh why cant point class have these things

            if (!locked)
            {
                activated = distance <= activationDistance;

                if (activated)
                {
                    progress.Regenerate(Universe.speedMultiplier);
                }
                else
                {
                    progress.Regenerate(-1 * Universe.speedMultiplier);
                }
            }

            rect.Location = Vector2.Lerp(start, destination, (float)progress.Percent()).ToPoint();
        }
    }
}
