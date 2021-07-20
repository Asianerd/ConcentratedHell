using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class PlasmaPrism : Gun
    {
        public PlasmaPrism()
        {
            Type = GunType.PlasmaPrism;
            AmmoType = Projectile.ProjectileType.LightShard;
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 100, 5, 0);
            AmmoUsage = 0;

            FiringEvent += Fire;
        }

        void Fire()
        {
            //var x = new LightShard(MathHelper.ToDegrees(Player.Instance.RadiansToMouse), Position, 10f);
            foreach(Enemy x in Enemy.Enemies)
            {
                var y = new LightShard(Universe.ANGLETO(Position, x.Position), Position, 10f);
            }
            if(Enemy.Enemies.Count != 0)
            {
                Rendering.ShakeScreen();
            }
        }
    }
}