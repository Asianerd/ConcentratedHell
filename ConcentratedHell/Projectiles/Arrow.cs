using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Arrow : Projectile, Projectile.IProjectiles
    {
        bool Duplicated = false;

        public Arrow(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.Arrow;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);
        }

        public void Update()
        {
            Move();
            if (Vector2.Distance(Player.Instance.Position, Position) >= Projectile.DespawnDistance)
            {
                Dispose(ProjectileEventType.Despawn);
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
                    candidate.AffectHealth(-Damage);
                    candidate.ToggleAnger();
                    Dispose(ProjectileEventType.Hit);
                }
            }
        }

        public void Destroy(ProjectileEventType _type)
        {
            if (_type == ProjectileEventType.Hit && !Duplicated)
            {
                for (int i = 0; i < 5; i++)
                {
                    var x = new Arrow((360 / 5) * i, new Vector2(
                        (float)Math.Cos(i) * 50,
                        (float)Math.Sin(i) * 50
                        ) + Position, 5f);
                    x.Duplicated = true;
                }
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
