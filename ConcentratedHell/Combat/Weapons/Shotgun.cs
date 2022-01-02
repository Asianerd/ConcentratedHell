using System;
using System.Collections.Generic;
using System.Text;
using ConcentratedHell.Combat.Projectiles;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Combat.Weapons
{
    class Shotgun:Weapon
    {
        public Shotgun() : base(Type.Shotgun, Projectile.Type.Pellet)
        {

        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);

            for (int i = 0; i <= 20; i++)
            {
                var x = new Pellet(origin, Cursor.Instance.playerToCursor + (Main.random.Next(-300, 300) / 1000f), Main.random.Next(-30,30)/10f);
            }
        }
    }
}
