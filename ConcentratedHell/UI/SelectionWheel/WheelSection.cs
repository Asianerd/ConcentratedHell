using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConcentratedHell.Combat;

namespace ConcentratedHell.UI.SelectionWheel
{
    class WheelSection
    {
        Weapon weapon;
        public bool isEquipped = false;
        public bool selected = false;
        public float rotation; // Radians
        public float min, max;

        public GameValue progress;

        public WheelSection(Weapon.Type type, float _rotation, float _min, float _max)
        {
            weapon = Player.Instance.arsenal[type];
            rotation = _rotation;

            min = _min;
            max = _max;

            progress = new GameValue(0, 10, 1, 0);
        }

        public void Update()
        {
            selected = (FullRadian(SelectionWheel.Instance.cursorAngle) > min) && (FullRadian(SelectionWheel.Instance.cursorAngle) < max);

            isEquipped = Player.Instance.equippedWeapon.type == weapon.type;

            if(selected && MouseInput.LMouse.active)
            {
                Player.Instance.EquipWeapon(weapon.type);
            }

            progress.Regenerate(isEquipped ? 1 : -1.5);
        }

        static float FullRadian(float radian)
        {
            if(radian < 0)
            {
                return (float)((Math.PI - MathF.Abs(radian)) + Math.PI);
            }
            else
            {
                return radian;
            }
        }

        public void Draw(float _distance, float scale)
        {
            float distance = _distance - ((float)progress.Percent() * _distance);

            Vector2 renderedPosition = new Vector2(
                MathF.Cos(rotation) * distance,
                MathF.Sin(rotation) * distance
                ) + SelectionWheel.origin;

            Main.spriteBatch.Draw(weapon.sprite, renderedPosition, null, Color.White, 0f, weapon.sprite.Bounds.Center.ToVector2(), scale * 6f, SpriteEffects.None, 0f);
        }
    }
}