using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class LightShard : Projectile, Projectile.IProjectiles
    {
        public LightShard(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.LightShard;
            Sprite = Sprites[Type];

            Speed = _speed;
            Damage = 500;

            Projectiles.Add(this);
        }

        public void Update()
        {
            Move();
            if (Vector2.Distance(Player.Instance.Position, Position) >= DespawnDistance)
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
