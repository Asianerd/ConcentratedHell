using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell.Combat.Weapons
{
    class AutoShotgun:Weapon
    {
        public AutoShotgun() : base(Type.Auto_Shotgun, Projectiles.Projectile.Type.Pellet, 65f, 2f, Ammo.Type.Shell, new GameValue(0, 15, 1))
        {

        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);

            for(int i = 0; i <= 30; i++)
            {
                var x = new Projectiles.Pellet(origin, Cursor.Instance.playerToCursor + Main.random.Next(-150, 150) / 1000f, Main.random.Next(-300, 300) / 100f);
            }
        }
    }
}
