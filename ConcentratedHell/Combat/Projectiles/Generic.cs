using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Combat.Projectiles
{
    class Generic:Projectile
    {
        public Generic(Vector2 _position, float _direction) : base(Type.Generic, 5f, 20f, _position, _direction)
        {

        }
    }
}
