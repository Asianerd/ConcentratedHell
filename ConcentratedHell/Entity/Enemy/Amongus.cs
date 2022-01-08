using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Entity
{
    class Amongus:Entity
    {
        public Amongus(Rectangle rect):base(rect, Type.Amongus, new GameValue(0, 5000, 100), 5f)
        {

        }

        public override void Update()
        {
            base.Update();

            if (Vector2.Distance(Player.Instance.rect.Location.ToVector2(), rect.Location.ToVector2()) >= 1000f)
            {
                rect.Location = Player.Instance.rect.Location;
            }
        }

        public override void OnDeath(float _direction = -10, float power = 3, float spread = 0.1f)
        {
            base.OnDeath(_direction, power, spread);

            foreach (Ammo.Type x in Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>())
            {
                var i = new Pickups.AmmoPickup(x, 10000, rect.Location.ToVector2());
            }
        }

        public override void DrawEyes()
        {

        }
    }
}
