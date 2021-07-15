using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Shotgun : Gun
    {
        public Shotgun()
        {
            Type = GunType.Shotgun;
            AmmoType = Projectile.ProjectileType.Pellet;
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 200, 5, 0);

            AmmoUsage = 3;
            FiringEvent += Fire;
        }

        void Fire()
        {
            for (int i = 0; i < 3; i++)
            {
                var x = new Pellet(MathHelper.ToDegrees(Player.Instance.RadiansToMouse) + ((i - 1) * 10), Position, 10f);
            }
        }
    }
}