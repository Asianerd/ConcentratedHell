using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Pellet : Projectile, Projectile.IProjectiles
    {
        public Pellet(double _direction, Vector2 _position, float _speed) : base(_direction, _position)
        {
            Main.UpdateEvent += Update;
            Rendering.DrawEntities += Draw;
            OnCollide += Destroy;

            Type = ProjectileType.Pellet;
            Sprite = Sprites[Type];

            Speed = _speed;

            Projectiles.Add(this);
        }

        public void Destroy(ProjectileEventType _type)
        {
            Projectiles.Remove(this);
        }
    }
}
