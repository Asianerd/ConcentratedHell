using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Bow : Gun
    {
        public Bow()
        {
            Type = GunType.Bow;
            AmmoType = Projectile.ProjectileType.Arrow;
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 100, 5, 0);

            FiringEvent += Fire;
        }

        void Fire()
        {
            var x = new Arrow(MathHelper.ToDegrees(Player.Instance.RadiansToMouse), Position, 10f);
        }
    }
}