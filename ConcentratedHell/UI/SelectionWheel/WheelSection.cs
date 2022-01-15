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
        GameValue scaleProgress;

        public WheelSection(Weapon.Type type, float _rotation, float _min, float _max)
        {
            weapon = Player.Instance.arsenal[type];
            rotation = _rotation;

            min = _min;
            max = _max;

            progress = new GameValue(0, 10, 1, 0);
            scaleProgress = new GameValue(0, 5, 1, 0);
        }

        public void Update()
        {
            selected = (SelectionWheel.Instance.fullCursorAngle > min) && (SelectionWheel.Instance.fullCursorAngle < max);

            scaleProgress.Regenerate(selected ? 1 : -1);

            isEquipped = Player.Instance.equippedWeapon.type == weapon.type;

            if(isEquipped)
            {
                scaleProgress.AffectValue(1f);
            }

            if(selected && MouseInput.LMouse.active)
            {
                Player.Instance.EquipWeapon(weapon.type);
            }

            progress.Regenerate(isEquipped ? 1 : -1.5);
        }

        public void Draw(float _distance, float scale, Color color)
        {
            float distance = _distance - ((float)progress.Percent() * _distance);

            Vector2 renderedPosition = new Vector2(
                MathF.Cos(rotation) * distance,
                MathF.Sin(rotation) * distance
                ) + SelectionWheel.origin;

            Main.spriteBatch.Draw(weapon.sprite, renderedPosition, null, color, 0f, weapon.sprite.Bounds.Center.ToVector2(), scale * 6f + (float)scaleProgress.Percent(), SpriteEffects.None, 0f);

            int ammoAmount = Player.Instance.ammoInventory[weapon.ammoType];
            string ammoText = ammoAmount.ToString();
            Vector2 origin = UI.font.MeasureString(ammoText) / 2f;
            renderedPosition += new Vector2(0, 40);
            Main.spriteBatch.DrawString(UI.font, ammoText, renderedPosition, ammoAmount > 0 ? color : UI.errorColor, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}