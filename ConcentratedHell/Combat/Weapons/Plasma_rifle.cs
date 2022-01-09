using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell.Combat.Weapons
{
    class Plasma_rifle:Weapon
    {
        int cartridgeCounter = 0;

        public Plasma_rifle() : base(Type.Plasma_Rifle, Projectiles.Projectile.Type.Plasma_orb, 110f, 0.8f, Ammo.Type.Plasma, new GameValue(0, 5, 1, 100))
        {

        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);
            var x = new Projectiles.Plasma_orb(origin, Cursor.Instance.playerToCursor);

            cartridgeCounter++;
            if(cartridgeCounter >= 20)
            {
                SpawnEmptyCartridge();
                cartridgeCounter = 0;
            }
        }
    }
}
