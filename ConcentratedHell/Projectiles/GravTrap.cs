using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ConcentratedHell
{
    class GravTrap : Projectile, Projectile.IProjectiles
    {
        GameValue Velocity;
        GameValue Age;

        public GravTrap(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;

            Type = ProjectileType.GravTrap;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);

            Velocity = new GameValue("Velocity", 0, 100, 0);
            Age = new GameValue("Age", 0, 500, 1, 0);
        }

        public void Update()
        {
            Move();
            Age.Regenerate();
            Direction += (1 - Age.Percent())*30;
            if (Vector2.Distance(Player.Instance.Position, Position) >= DespawnDistance)
            {
                Dispose(ProjectileEventType.Despawn);
            }

            if(Age.Percent() == 1)
            {
                Dispose(ProjectileEventType.Hit);
            }

            foreach (Enemy x in Enemy.Enemies.Where(n => Vector2.Distance(Position, n.Position) < 400f))
            {
                if (Vector2.Distance(Position, x.Position) > 200)
                {
                    x.Position = Position;
                }
            }
        }

        public void Move()
        {
            Velocity.AffectValue(-1d);
            Position += IncrementedVector * (float)Velocity.Percent() * Speed;
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
