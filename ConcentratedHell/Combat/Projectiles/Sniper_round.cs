using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Combat.Projectiles
{
    class Sniper_round:Projectile
    {
        int pierced = 4;

        public Sniper_round(Vector2 position, float direction):base(Type.Sniper_round, 50f, 500f, position, direction, _weight:100f)
        {

        }

        public override void Update()
        {
            base.Update();

            if (alive)
            {
                for (int i = 0; i < 3; i++)
                {
                    var x = new Particles.SmokeParticle(Vector2.Lerp(position, position + increment, i / 3f), direction, 5f);
                }
            }
        }

        public override void Destroy(bool mapCollide = false)
        {
            if(mapCollide)
            {
                base.Destroy();
                return;
            }

            pierced -= 1;
            if (pierced <= 0)
            {
                base.Destroy();
            }
        }
    }
}
