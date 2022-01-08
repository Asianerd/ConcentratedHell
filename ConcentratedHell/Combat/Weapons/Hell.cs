using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell.Combat.Weapons
{
    class Hell:Weapon
    {
        public Hell():base(Type.Hell, Projectiles.Projectile.Type.Sniper_round, 0f, 0f, Ammo.Type.Large, new GameValue(0, 50, 1), 0)
        {

        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);

            foreach(Entity.Entity x in Entity.Entity.collection)
            {
                float direction = MathF.Atan2(
                    x.rect.Center.Y - Player.Instance.rect.Center.Y,
                    x.rect.Center.X - Player.Instance.rect.Center.X
                    );

                var i = new Projectiles.Sniper_round(origin, direction);
            }
        }
    }
}
