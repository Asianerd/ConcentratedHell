using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Trapper : Gun
    {
        public Trapper()
        {
            AmmoUsage = 0;

            Type = GunType.Trapper;
            AmmoType = Projectile.ProjectileType.GravTrap;
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 100, 5, 0);

            FiringEvent += Fire;
        }

        void Fire()
        {
            var x = new GravTrap(MathHelper.ToDegrees(Player.Instance.RadiansToMouse), Position, 10f);
        }
    }
}
