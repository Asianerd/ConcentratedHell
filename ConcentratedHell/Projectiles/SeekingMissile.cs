using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ConcentratedHell
{
    class SeekingMissile: Projectile, Projectile.IProjectiles
    {
        Enemy TrackedEnemy;

        public SeekingMissile(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.SeekingMissile;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);

            float _distance = 4000f;
            foreach (Enemy x in Enemy.Enemies)
            {
                float _iterationDistance = Vector2.Distance(Position, x.Position);
                if (_iterationDistance < _distance)
                {
                    _distance = _iterationDistance;
                    TrackedEnemy = x;
                }
            }
        }

        void Move()
        {
            if (TrackedEnemy != null)
            {
                if (TrackedEnemy.Alive)
                {
                    Direction = Universe.ANGLETO(Position, TrackedEnemy.Position, true);
                }
            }
            Position += new Vector2((float)Math.Cos(MathHelper.ToRadians((float)Direction)), (float)Math.Sin(MathHelper.ToRadians((float)Direction))) * Speed;
            CollisionCheck();
        }

        public void Update()
        {
            Move();
            if ((Position.X < 0) ||
                (Position.X > Main.screenSize.X) ||
                (Position.Y < 0) ||
                (Position.Y > Main.screenSize.Y))
            {

                Dispose(ProjectileEventType.Despawn);
            }
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
                    Dispose(ProjectileEventType.Hit);
                }
            }
        }

        public void Destroy(ProjectileEventType _type)
        {
            foreach (Enemy x in Enemy.Enemies.Where(n => Vector2.Distance(Position, n.Position) <= 500f))
            {
                x.Health.AffectValue(-(double)(30 * ((500 - Vector2.Distance(Position, x.Position)) / 500)));
                x.ToggleAnger();
            }

            if(_type == ProjectileEventType.Hit)
            {
                Rendering.ShakeScreen();
            }

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
