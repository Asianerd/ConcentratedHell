using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Combat.Projectiles
{
    class Small_round:Projectile
    {
        public Small_round(Vector2 position, float direction) : base(Type.Small_round, 20f, 5f, position, direction)
        {

        }
    }
}
