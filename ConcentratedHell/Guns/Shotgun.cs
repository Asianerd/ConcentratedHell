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
            Sprite = GunSprites[Type];

            Main.PlayerUpdateEvent += Update;
            Rendering.DrawPlayer += Draw;
            DestroyEvent += Destroy;

            Cooldown = new GameValue("Cooldown", 0, 100, 5);

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