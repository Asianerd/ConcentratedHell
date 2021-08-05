using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ConcentratedHell
{
    class Grenade : Projectile, Projectile.IProjectiles
    {
        GameValue Velocity;
        GameValue Countdown;

        public Grenade(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.Grenade;
            Sprite = Sprites[Type];

            Speed = _speed;
            Velocity = new GameValue("Velocity", 0, 100, 0);
            Countdown = new GameValue("Countdoww", 0, 300, 1, 0);

            Projectiles.Add(this);
        }

        public void Update()
        {
            Move();
            Direction += Velocity.Percent() * 20;
            if (Vector2.Distance(Player.Instance.Position, Position) >= DespawnDistance)
            {
                Dispose(ProjectileEventType.Despawn);
            }
        }

        public void Move()
        {
            Velocity.AffectValue(-1d);
            Position += IncrementedVector * (float)(Velocity.Percent()) * 5;

            Countdown.Regenerate();
            if (Countdown.Percent() == 1)
            {
                Dispose(ProjectileEventType.Hit);
            }
        }

        #region Normal Collision, Destroying and Disposing
/*        public void CollisionCheck()
        {
            float nearest = 2205f;
            Enemy candidate = null;
            foreach (Enemy x in Enemy.Enemies)
            {
                float current = Vector2.Distance(Position, x.Position);
                if (current < nearest)
                {
                    nearest = current;
                    candidate = x;
                }
            }

            if (candidate != null)
            {
                if (nearest <= CollideDistance)
                {
                    candidate.Health.AffectValue(-Damage);
                    candidate.ToggleAnger();
                    Dispose(ProjectileEventType.Hit);
                }
            }
        }*/


        public void Destroy(ProjectileEventType _type)
        {
            foreach (Enemy x in Enemy.Enemies.Where(n => Vector2.Distance(Position, n.Position) < 700))
            {
                x.AffectHealth(-(double)(200 * ((500 - Vector2.Distance(Position, x.Position)) / 500)));
                x.ToggleAnger();
            }

            Rendering.ShakeScreen();

            Projectiles.Remove(this);
        }

        public void Dispose(ProjectileEventType _type)
        {
            Destroy(_type);
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;
        }
        #endregion
    }
}
