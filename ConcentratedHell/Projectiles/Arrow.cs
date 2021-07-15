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
            OnCollide += Destroy;

            Type = ProjectileType.Arrow;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);
        }

        public void Destroy(ProjectileEventType _type)
        {
            if (_type == ProjectileEventType.Hit && !Duplicated)
            {
                for (int i = 0; i < 5; i++)
                {
                    var x = new Arrow((360 / 5) * i, new Vector2(
                        (float)Math.Cos(i)*50,
                        (float)Math.Sin(i)*50
                        )+Position, 5f);
                    x.Duplicated = true;
                }
            }
            Projectiles.Remove(this);
        }
    }
}
