using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Bullet : Projectile, Projectile.IProjectiles
    {
        public Bullet(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.Bullet;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);
        }

        public void Update()
        {
            Move();
            if ((Position.X < 0) ||
                (Position.X > Main.screenSize.X) ||
                (Position.Y < 0) ||
                (Position.Y > Main.screenSize.Y))
            {

                Dispose();
            }
        }

        public void Move()
        {
            Position += IncrementedVector * Speed;
            CollisionCheck();
        }

        #region Normal Collision, Destroying and Disposing
        public void CollisionCheck()
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
                    Dispose();
                }
            }
        }

        public void Destroy()
        {
            Projectiles.Remove(this);
        }

        public void Dispose()
        {
            Destroy();
            Main.UpdateEvent -= Update;
            Rendering.DrawEntities -= Draw;
        }
        #endregion
    }
}
