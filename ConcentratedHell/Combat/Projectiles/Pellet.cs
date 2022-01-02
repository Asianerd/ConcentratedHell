using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell.Combat.Projectiles
{
    class Pellet:Projectile
    {
        public Pellet(Vector2 _position, float _direction, float _fluctuation):base(Type.Pellet, 30f + _fluctuation, 5f, _position, _direction)
        {
            renderedScale = 2f;
        }
    }
}
