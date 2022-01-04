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
        public Texture2D eyeSprite;
        public Vector2 healthBarSize;
        public bool alive = true;

        public float knockbackResist = 1f;

        public bool angered = false;
        public float detectDistance;

        public float lastHitDirection;
        public float lastHitPower;

        public Enemy(Rectangle _rect, Type _type, GameValue _health, float _speed, float _detectDistance)
        {
            rect = _rect;
            type = _type;
            collection.Add(this);

            health = _health;
            healthBarSize = new Vector2(rect.Width, 8);

            speed = _speed;
            direction = (Main.random.Next(0, 100) / 100f) * MathF.PI * 2f;

            detectDistance = _detectDistance;

            sprite = enemyAssets[type][0];
            eyeSprite = enemyAssets[type][angered ? 2 : 1];
        }

        public virtual void Update()
        {
            if (angered)
            {
                PathFind(Player.Instance.rect.Location);
            }
            else
            {
                if(Vector2.Distance(Player.Instance.rect.Center.ToVector2(), rect.Center.ToVector2()) <= detectDistance)
                {
                    Anger();
                }
            }

            alive = health.Percent() > 0f;
            if (!alive)
            {
                OnDeath(_direction:lastHitDirection, power:lastHitPower);
            }
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

        public void Knockback(float _direction, float rawDistance)
        {
            float _distance = rawDistance * knockbackResist;

            Vector2 targetVelocity = new Vector2(
                MathF.Cos(_direction) * _distance,
                MathF.Sin(_direction) * _distance
                );

            Rectangle targetRectangle = new Rectangle((rect.Location.ToVector2() + (targetVelocity * speed)).ToPoint(), rect.Size);

            if (Map.IsValidPosition(targetRectangle))
            {
                rect.Location = targetRectangle.Location;
                return;
            }

            Vector2 xVel = new Vector2(targetVelocity.X, 0);
            Rectangle xRect = new Rectangle((rect.Location.ToVector2() + (xVel * speed)).ToPoint(), rect.Size);
            if (Map.IsValidPosition(xRect))
            {
                rect.Location = xRect.Location;
            }

            Vector2 yVel = new Vector2(0, targetVelocity.Y);
            Rectangle yRect = new Rectangle((rect.Location.ToVector2() + (yVel * speed)).ToPoint(), rect.Size);
            if (Map.IsValidPosition(yRect))
            {
                rect.Location = yRect.Location;
            }
        }

        public virtual void AffectHealth(double damage, float direction, float speed)
        {
            health.AffectValue(-damage);
            Anger();

            lastHitDirection = direction;
            lastHitPower = speed/2f;
        }

        public virtual void OnDeath(float _direction = -10f, float power = 3f, float spread = 0.1f)
        {
            var x = new Pickups.AmmoPickup(
                Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>().ToArray()[Main.random.Next(0, Enum.GetValues(typeof(Ammo.Type)).Length)],
                Main.random.Next(1, 50), rect.Center.ToVector2());

            Vector2 pos = rect.Center.ToVector2();
            for (int i = 0; i <= 10; i++)
            {
                float direction = (float)(_direction == -10f ? ((Main.random.Next(0, 100) / 100f) * Math.PI * 2f) : (_direction + ((Main.random.Next(-100, 100) / 100f) * spread * Math.PI * 2f)));
                var p = new Particles.GoreParticle(pos, direction, power * (Main.random.Next(97, 103) / 100f));
                //var p = new Particles.SmokeParticle(pos, direction, power * (Main.random.Next(97, 103) / 100f));
            }
        }

        public virtual void Anger()
        {
            if(angered)
            {
                return;
            }

            angered = true;
            eyeSprite = enemyAssets[type][2];
            foreach(Enemy x in collection.Where(n => Vector2.Distance(n.rect.Center.ToVector2(), rect.Center.ToVector2()) <= 200f))
            {
                if(Main.random.Next(0, 10) == 1)
                {
                    x.Anger();
                }
            }
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
            Vector2 renderPosition = new Vector2(
                (MathF.Cos(direction) * 5f) + rect.Center.X,
                (MathF.Sin(direction) * 5f) + rect.Center.Y - 5f
                );
            Main.spriteBatch.Draw(eyeSprite, renderPosition, null, Color.White, 0f, eyeSprite.Bounds.Center.ToVector2(), 4.5f, SpriteEffects.None, 0f);
        }

        public virtual void DrawHealthBar()
        {
            if (health.Percent() != 1)
            {
                Main.spriteBatch.Draw(healthBar, new Rectangle(rect.Location.X, rect.Bottom + 10, (int)(healthBarSize.X * health.Percent()), (int)healthBarSize.Y), Color.White);
            }
        }

        #region Statics
        //public static Dictionary<Type, Texture2D> spriteTable;
        public static List<Enemy> collection;
        public static Texture2D healthBar;
        public static Dictionary<Type, List<Texture2D>> enemyAssets;
        /* 0 Body
         * 1 Neutral Eyes
         * 2 Angered Eyes
         */

        public static void Initialize()
        {
            collection = new List<Enemy>();
            Main.UpdateEvent += StaticUpdate;
            Main.DrawEvent += StaticDraw;
        }

        public static void LoadContent(Texture2D _healthBar)
        {
            enemyAssets = new Dictionary<Type, List<Texture2D>>();
            foreach (Type x in Enum.GetValues(typeof(Type)).Cast<Type>())
            {
                string _path = x.ToString().ToLower();
                enemyAssets.Add(x, new List<Texture2D>()
                {
                    Main.Instance.Content.Load<Texture2D>($"Enemy/{_path}/body"),
                    Main.Instance.Content.Load<Texture2D>($"Enemy/{_path}/Eyes/neutral"),
                    Main.Instance.Content.Load<Texture2D>($"Enemy/{_path}/Eyes/angered")
                });
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
