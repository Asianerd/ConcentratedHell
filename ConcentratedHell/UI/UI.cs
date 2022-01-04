using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell.UI
{
    class UI
    {
        public static UI Instance;
        public static SpriteFont font;
        public static bool selectionActive;
        public static Color errorColor;

        public static void Initialize()
        {
            Instance = new UI();
            Cursor.Initialize();
            SelectionWheel.SelectionWheel.Initialize();

            errorColor = new Color(184, 30, 17);

            PickupText.Initialize();

            Main.UpdateEvent += Update;
        }

        public static void LoadContent(SpriteFont _font)
        {
            font = _font;
        }

        public static void Update()
        {
            PickupText.StaticUpdate();
            Cursor.Instance.Update();
            SelectionWheel.SelectionWheel.Instance.Update();
            selectionActive = SelectionWheel.SelectionWheel.Instance.progress.Percent() > 0;
        }

        public static void Draw()
        {
            if (Player.Instance.equippedWeapon != null)
            {
                int ammoAmount = Player.Instance.ammoInventory[Player.Instance.equippedWeapon.ammoType];
                string ammoText = ammoAmount > 0 ? ammoAmount.ToString() : "Out of ammo!";
                Vector2 origin = font.MeasureString(ammoText) / 2f;
                Vector2 renderedPosition = Cursor.Instance.screenPosition + new Vector2(2, 50);
                Main.spriteBatch.DrawString(font, ammoText, renderedPosition, ammoAmount >= Player.Instance.equippedWeapon.ammoUsage ? Color.White : errorColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            }

            SelectionWheel.SelectionWheel.Instance.Draw();

            Cursor.Instance.Draw();
            PickupText.StaticDraw();
        }
    }
}
