using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Enemy
    {
        public Rectangle rect;
        public GameValue health;
        public float direction;
        public float speed;
        public Type type;
        public Texture2D sprite;
        public Vector2 healthBarSize;
        public bool alive = true;

        public Enemy(Rectangle _rect, Type _type, GameValue _health, float _speed)
        {
            rect = _rect;
            type = _type;
            sprite = spriteTable[type];
            collection.Add(this);

            health = _health;
            healthBarSize = new Vector2(rect.Width, 8);

            speed = _speed;
        }

        public virtual void Update()
        {
            PathFind(Player.Instance.rect.Location);
            alive = health.Percent() > 0f;
        }

        public virtual void PathFind(Point target)
        {
            //float distance = MathF.Sqrt(MathF.Pow(MathF.Abs(target.X - rect.X), 2) + MathF.Pow(MathF.Abs(target.Y - rect.Y), 2));
            float distance = Vector2.Distance(target.ToVector2(), rect.Location.ToVector2());
            // (x + y)^0.5

            float processedSpeed = (distance < speed ? distance : speed) * Universe.speedMultiplier;

            direction = MathF.Atan2(
                target.Y - rect.Y,
                target.X - rect.X
                );
            Vector2 increment = new Vector2(
                MathF.Cos(direction) * processedSpeed,
                MathF.Sin(direction) * processedSpeed
                );

            Rectangle candidate = new Rectangle(
                rect.Location + increment.ToPoint(),
                rect.Size
                );

            if(increment != Vector2.Zero)
            {
                increment.Normalize();
            }

            if (Map.IsValidPosition(candidate))
            {
                rect.Location = candidate.Location;
                return;
            }

            Point xVel = new Point((int)Math.Ceiling(increment.X), 0);
            Rectangle xRect = new Rectangle(rect.Location + xVel, rect.Size);
            if(Map.IsValidPosition(xRect))
            {
                rect.Location = xRect.Location;
            }

            Point yVel = new Point(0, (int)Math.Ceiling(increment.Y));
            Rectangle yRect = new Rectangle(rect.Location + yVel, rect.Size);
            if(Map.IsValidPosition(yRect))
            {
                rect.Location = yRect.Location;
            }
        }

        public virtual void AffectHealth(double damage)
        {
            health.AffectValue(-damage);
        }

        public virtual void Die()
        {
            collection.Remove(this);
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
        }

        public virtual void DrawHealthBar()
        {
            if (health.Percent() != 1)
            {
                Main.spriteBatch.Draw(healthBar, new Rectangle(rect.Location.X, rect.Bottom + 10, (int)(healthBarSize.X * health.Percent()), (int)healthBarSize.Y), Color.White);
            }
        }

        #region Statics
        public static Dictionary<Type, Texture2D> spriteTable;
        public static List<Enemy> collection;
        public static Texture2D healthBar;

        public static void Initialize()
        {
            collection = new List<Enemy>();
            Main.UpdateEvent += StaticUpdate;
            Main.DrawEvent += StaticDraw;
        }

        public static void LoadContent(Texture2D _healthBar)
        {
            string enemyPath = "Enemy";
            spriteTable = new Dictionary<Type, Texture2D>();
            foreach(Type x in Enum.GetValues(typeof(Type)).Cast<Type>())
            {
                spriteTable.Add(x, Main.Instance.Content.Load<Texture2D>($"{enemyPath}/{x.ToString().ToLower()}"));
            }

            healthBar = _healthBar;
        }

        public static void StaticUpdate()
        {
            foreach(Enemy x in collection)
            {
                x.Update();
            }
            collection.RemoveAll(n => !n.alive);
        }

        public static void StaticDraw()
        {
            foreach(Enemy x in collection)
            {
                x.Draw();
            }
            foreach(Enemy x in collection)
            {
                x.DrawHealthBar();
            }
        }

        public enum Type
        {
            Cyborg
        }
        #endregion
    }
}
