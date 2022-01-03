using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.Pickups
{
    class AmmoPickup:Pickup
    {
        public Ammo.Type type;

        public AmmoPickup(Ammo.Type _type, int amount, Vector2 position) : base(PickupType.Ammo, amount, position, Ammo.spriteTable[_type])
        {
            type = _type;
            renderedScale = 7f;
            rotation = Main.random.Next(-300, 300) / 100f;
        }

        public override void Update()
        {
            base.Update();

            rotation += 0.01f;
        }

        public override void AddToPlayer()
        {
            base.AddToPlayer();

            Player.Instance.ammoInventory[type] += amount;
        }
    }
}
