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
/*        public Type type;
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
            type = _type;
            collection.Add(this);

            healthBarSize = new Vector2(entity.rect.Width, 8);

            entity.speed = _speed;
            entity.direction = (Main.random.Next(0, 100) / 100f) * MathF.PI * 2f;

            detectDistance = _detectDistance;

            sprite = enemyAssets[type][0];
            eyeSprite = enemyAssets[type][angered ? 2 : 1];
        }

        public virtual void Update()
        {
            if (angered)
            {
                PathFind(Player.Instance.entity.rect.Location);
            }
            else
            {
                if (Vector2.Distance(Player.Instance.entity.rect.Center.ToVector2(), entity.rect.Center.ToVector2()) <= detectDistance)
                {
                    Anger();
                }
            }

            alive = entity.health.Percent() > 0f;
            if (!alive)
            {
                OnDeath(_direction: lastHitDirection, power: lastHitPower);
            }
        }

        public virtual void PathFind(Point target)
        {
            //float distance = MathF.Sqrt(MathF.Pow(MathF.Abs(target.X - rect.X), 2) + MathF.Pow(MathF.Abs(target.Y - rect.Y), 2));
            float distance = Vector2.Distance(target.ToVector2(), entity.rect.Location.ToVector2());
            // (x + y)^0.5

            float processedSpeed = (distance < entity.speed ? distance : entity.speed) * Universe.speedMultiplier;

            entity.direction = MathF.Atan2(
                target.Y - entity.rect.Y,
                target.X - entity.rect.X
                );
            Vector2 increment = new Vector2(
                MathF.Cos(entity.direction) * processedSpeed,
                MathF.Sin(entity.direction) * processedSpeed
                );

            Rectangle candidate = new Rectangle(
                entity.rect.Location + increment.ToPoint(),
                entity.rect.Size
                );

            if (increment != Vector2.Zero)
            {
                increment.Normalize();
            }

            if (Map.IsValidPosition(candidate))
            {
                entity.rect.Location = candidate.Location;
                return;
            }

            Point xVel = new Point((int)Math.Ceiling(increment.X), 0);
            Rectangle xRect = new Rectangle(entity.rect.Location + xVel, entity.rect.Size);
            if (Map.IsValidPosition(xRect))
            {
                entity.rect.Location = xRect.Location;
            }

            Point yVel = new Point(0, (int)Math.Ceiling(increment.Y));
            Rectangle yRect = new Rectangle(entity.rect.Location + yVel, entity.rect.Size);
            if (Map.IsValidPosition(yRect))
            {
                entity.rect.Location = yRect.Location;
            }
        }

        public void Knockback(float _direction, float rawDistance)
        {
            float _distance = rawDistance * knockbackResist;

            Vector2 targetVelocity = new Vector2(
                MathF.Cos(_direction) * _distance,
                MathF.Sin(_direction) * _distance
                );

            Rectangle targetRectangle = new Rectangle((entity.rect.Location.ToVector2() + (targetVelocity * entity.speed)).ToPoint(), entity.rect.Size);

            if (Map.IsValidPosition(targetRectangle))
            {
                entity.rect.Location = targetRectangle.Location;
                return;
            }

            Vector2 xVel = new Vector2(targetVelocity.X, 0);
            Rectangle xRect = new Rectangle((entity.rect.Location.ToVector2() + (xVel * entity.speed)).ToPoint(), entity.rect.Size);
            if (Map.IsValidPosition(xRect))
            {
                entity.rect.Location = xRect.Location;
            }

            Vector2 yVel = new Vector2(0, targetVelocity.Y);
            Rectangle yRect = new Rectangle((entity.rect.Location.ToVector2() + (yVel * entity.speed)).ToPoint(), entity.rect.Size);
            if (Map.IsValidPosition(yRect))
            {
                entity.rect.Location = yRect.Location;
            }
        }

        public virtual void AffectHealth(double damage, float direction, float speed)
        {
            entity.health.AffectValue(-damage);
            Anger();

            lastHitDirection = direction;
            lastHitPower = speed / 2f;
        }

        public virtual void OnDeath(float _direction = -10f, float power = 3f, float spread = 0.1f)
        {
            var x = new Pickups.AmmoPickup(
                Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>().ToArray()[Main.random.Next(0, Enum.GetValues(typeof(Ammo.Type)).Length)],
                Main.random.Next(1, 50), entity.rect.Center.ToVector2());

            Vector2 pos = entity.rect.Center.ToVector2();
            for (int i = 0; i <= 10; i++)
            {
                float direction = (float)(_direction == -10f ? ((Main.random.Next(0, 100) / 100f) * Math.PI * 2f) : (_direction + ((Main.random.Next(-100, 100) / 100f) * spread * Math.PI * 2f)));
                var p = new Particles.GoreParticle(pos, direction, power * (Main.random.Next(97, 103) / 100f));
                //var p = new Particles.SmokeParticle(pos, direction, power * (Main.random.Next(97, 103) / 100f));
            }
        }

        public virtual void Anger()
        {
            if (angered)
            {
                return;
            }

            angered = true;
            eyeSprite = enemyAssets[type][2];
            foreach (Enemy x in collection.Where(n => Vector2.Distance(n.entity.rect.Center.ToVector2(), entity.rect.Center.ToVector2()) <= 200f))
            {
                if (Main.random.Next(0, 10) == 1)
                {
                    x.Anger();
                }
            }
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(sprite, entity.rect, Color.White);
            Vector2 renderPosition = new Vector2(
                (MathF.Cos(entity.direction) * 5f) + entity.rect.Center.X,
                (MathF.Sin(entity.direction) * 5f) + entity.rect.Center.Y - 5f
                );
            Main.spriteBatch.Draw(eyeSprite, renderPosition, null, Color.White, 0f, eyeSprite.Bounds.Center.ToVector2(), 4.5f, SpriteEffects.None, 0f);
        }

        public virtual void DrawHealthBar()
        {
            if (entity.health.Percent() != 1)
            {
                Main.spriteBatch.Draw(healthBar, new Rectangle(entity.rect.Location.X, entity.rect.Bottom + 10, (int)(healthBarSize.X * entity.health.Percent()), (int)healthBarSize.Y), Color.White);
            }
        }*/

        #region Statics
        public static Texture2D healthBar;
        public static Dictionary<Entity.Type, List<Texture2D>> enemyAssets;
        /* 0 Body
         * 1 Neutral Eyes
         * 2 Angered Eyes
         */

        public static void LoadContent(Texture2D _healthBar)
        {
            enemyAssets = new Dictionary<Entity.Type, List<Texture2D>>();
            foreach (Entity.Type x in Enum.GetValues(typeof(Entity.Type)).Cast<Entity.Type>())
            {
                if (x == Entity.Type.Player)
                {
                    continue;
                }

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

        public enum Type
        {
            Cyborg
        }
        #endregion
    }
}
