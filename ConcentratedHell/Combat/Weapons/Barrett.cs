using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell.Combat.Weapons
{
    class Barrett:Weapon
    {
        public Barrett():base(Type.Barrett, Projectiles.Projectile.Type.Sniper_round, 110f, 15f, Ammo.Type.Large, new GameValue(0, 120, 1))
        {

        }

        public override void Update()
        {
            base.Update();

            if (MouseInput.RMouse.isPressed)
            {
                Camera.Instance.SetTarget(
                    Vector2.Lerp(Player.Instance.rect.Center.ToVector2(), Cursor.Instance.worldPosition, 0.5f)
                    );
            }
        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);

            var x = new Projectiles.Sniper_round(origin, Cursor.Instance.playerToCursor);

            SpawnEmptyCartridge();
        }
    }
}
