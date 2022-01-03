using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Particles
{
    class SmokeGroup:ParticleGroup
    {
        public SmokeGroup(Vector2 origin) : base(Type.Smoke, origin)
        {

        }
    }
}
