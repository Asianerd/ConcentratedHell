using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConcentratedHell.Map_related.Levels;

namespace ConcentratedHell
{
    class Map
    {
        public static Map currentMap;
        public static Dictionary<Level, Map> maps;
        public static Texture2D placeholderSprite;
        public static Level level;
        public static List<Texture2D> slices;

        public static void Initialize()
        {
            maps = new Dictionary<Level, Map>()
            {
                { Level.Default, new Default() },
            };

            currentMap = new Default();

            slices = new List<Texture2D>();
            for (int i = 1; i <= 9; i++)
            {
                slices.Add(Main.Instance.Content.Load<Texture2D>($"Map/9slice/{i}"));
            }

            Main.UpdateEvent += StaticUpdate;
            Main.MidgroundDrawEvent += StaticDraw;
        }

        public static void StaticUpdate()
        {
            currentMap.Update();
        }

        public static void StaticDraw()
        {
            currentMap.Draw();
        }

        public static void AdvanceLevel(Level? _type)
        {
            if (_type != null)
            {
                level = (Level)Math.Clamp((int)_type + 1, 0, Enum.GetValues(typeof(Level)).Cast<Level>().Count() - 1);
                currentMap = maps[level];
            }
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

        #region Object-related
        public List<Tile> map;
        public Vector2 playerSpawn;
        public List<Vector2> enemySpawns;
        public int wave = 0;
        public GameValue waveCooldown;

        public GameValue spawnCooldown;
        public GameValue spawnProgress;

        public Map(List<Tile> tiles, Vector2 spawnPosition, List<Vector2> _enemySpawns)
        {
            map = tiles;
            playerSpawn = spawnPosition;
            enemySpawns = _enemySpawns;

            spawnProgress = new GameValue(0, 10, -1);
            spawnCooldown = new GameValue(0, 30, 1);

            waveCooldown = new GameValue(0, 300, 1, 0);
        }

        public virtual void Update()
        {
            foreach(Tile x in map)
            {
                x.Update();
            }

            if(Entity.Entity.enemies.Count <= 0f)
            {
                waveCooldown.Regenerate(Universe.speedMultiplier);
            }

            if((spawnProgress.Percent() <= 0f) && (waveCooldown.Percent() >= 1f))
            {
                wave++;
                waveCooldown.AffectValue(0f);

                spawnProgress.AffectMax(5d);
                spawnProgress.AffectValue(1f);
            }

            spawnCooldown.Regenerate(Universe.speedMultiplier);
            if((spawnCooldown.Percent() >= 1f) && (spawnProgress.Percent() > 0f))
            {
                var x = new Entity.Cyborg(new Rectangle(enemySpawns[Main.random.Next(0, enemySpawns.Count)].ToPoint(), new Point(64, 64)));
                spawnCooldown.AffectValue(0f);
                spawnProgress.Regenerate();
            }
        }

        public virtual void Draw()
        {
            foreach(Tile x in map)
            {
                x.Draw();
            }

            string levelText = $"" +
                $"Wave : {wave}\n" +
                $"Enemies : {Entity.Entity.enemies.Count}/{spawnProgress.Max}\n";
            Vector2 textOrigin = UI.UI.font.MeasureString(levelText) / 2f;
            Main.spriteBatch.DrawString(UI.UI.font, levelText, Vector2.Zero, Color.White, 0f, textOrigin, 2f, SpriteEffects.None, 0f);
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
