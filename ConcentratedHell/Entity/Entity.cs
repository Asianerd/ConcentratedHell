using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Entity
    {
        #region Static
        public static List<Entity> collection;

        public static void Initialize()
        {
            collection = new List<Entity>();

            var x = new Player();

            Main.UpdateEvent += StaticUpdate;
            Main.DrawEvent += StaticDraw;
        }

        public static void StaticUpdate()
        {
            foreach(Entity x in collection)
            {
                x.Update();
            }
            collection.RemoveAll(n => (n.type != Type.Player) && (!n.alive));
        }

        public static void StaticDraw()
        {
            foreach(Entity x in collection)
            {
                x.Draw();
            }
            foreach(Entity x in collection)
            {
                x.DrawHealthBar();
            }
        }
        #endregion

        public Type type;
        public Rectangle rect;
        public Texture2D sprite;
        public Texture2D eyeSprite;

        public GameValue health;
        public bool alive = true;

        public Vector2 healthBarSize;

        #region Movement
        public float speed;
        public float direction;
        #endregion

        #region Combat
        public bool angered = false;

        public float detectDistance = 50f;
        public float knockbackResist = 1f;

        public float lastHitDirection;
        public float lastHitPower;
        #endregion

        public Entity(Rectangle _rect, Type _type, GameValue _health, float _speed, float _detectionDistance = 300f)
        {
            type = _type;

            rect = _rect;
            healthBarSize = new Vector2(rect.Width, 8f);
            health = _health;

            speed = _speed;
            direction = (Main.random.Next(0, 100) / 100f) * MathF.PI * 2f;

            detectDistance = _detectionDistance;

            collection.Add(this);

            if (type != Type.Player)
            {
                sprite = Enemy.enemyAssets[type][0];
                eyeSprite = Enemy.enemyAssets[type][1];
            }
            else
            {
            }
        }

        public virtual void Update()
        {
            if (angered)
            {
                PathFind(Player.Instance.rect.Location);
            }
            else
            {
                if (Vector2.Distance(Player.Instance.rect.Center.ToVector2(), rect.Center.ToVector2()) <= detectDistance)
                {
                    Anger();
                }
            }

            alive = health.Percent() > 0f;
            if (!alive)
            {
                OnDeath(_direction: lastHitDirection, power: lastHitPower);
            }
        }

        #region Movement
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

            if (increment != Vector2.Zero)
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
            if (Map.IsValidPosition(xRect))
            {
                rect.Location = xRect.Location;
            }

            Point yVel = new Point(0, (int)Math.Ceiling(increment.Y));
            Rectangle yRect = new Rectangle(rect.Location + yVel, rect.Size);
            if (Map.IsValidPosition(yRect))
            {
                rect.Location = yRect.Location;
            }
        }

        public virtual void Knockback(float _direction, float rawDistance)
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
        #endregion

        #region Stats
        public virtual void AffectHealth(double damage, float _direction, float _speed)
        {
            health.AffectValue(-damage);
            Anger();

            lastHitDirection = _direction;
            lastHitPower = _speed / 2f;
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
            }
        }

        public virtual void Anger()
        {
            if(angered)
            {
                return;
            }

            angered = true;
            eyeSprite = Enemy.enemyAssets[type][2];
            foreach(Entity x in collection.Where(n => Vector2.Distance(n.rect.Center.ToVector2(), rect.Center.ToVector2()) <= 200f))
            {
                if(Main.random.Next(0, 10) == 1)
                {
                    x.Anger();
                }
            }
        }
        #endregion

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
                Main.spriteBatch.Draw(Enemy.healthBar, new Rectangle(rect.Location.X, rect.Bottom + 10, (int)(healthBarSize.X * health.Percent()), (int)healthBarSize.Y), Color.White);
            }
        }

        public enum Type
        {
            Player,
            Cyborg
        }
    }
}
