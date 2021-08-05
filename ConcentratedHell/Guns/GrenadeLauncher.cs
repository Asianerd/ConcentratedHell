using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class GrenadeLauncher : Gun
    {
        public GrenadeLauncher()
        {
            Type = GunType.GrenadeLauncher;
            AmmoType = Projectile.ProjectileType.Grenade;
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 5, 5, 0);

            FiringEvent += Fire;
        }

        void Fire()
        {
            var x = new Grenade(MathHelper.ToDegrees(Player.Instance.RadiansToMouse), Position, 10f);
        }
    }
}
