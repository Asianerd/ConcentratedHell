using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.UI
{
    class PickupText
    {
        #region Static
        public static List<PickupText> collection;

        public static void Initialize()
        {
            collection = new List<PickupText>();
        }

        public static void AppendItem(Ammo.Type type, int amount)
        {
            List<PickupText> target = collection.Where(n => n.type == type).ToList();

            if(target.Count >= 1)
            {
                target[0].amount += amount;
                target[0].age.AffectValue(0f);
            }
            else
            {
                target.Add(new PickupText(type, amount));
            }
        }

        public static void StaticUpdate()
        {
            foreach(PickupText x in collection)
            {
                x.Update();
            }
            collection.RemoveAll(n => !n.active);
        }

        public static void StaticDraw()
        {
            foreach(var item in collection.Select((value, index) => new { value, index }))
            {
                string title = $"x{item.value.amount}";
                Vector2 stringRect = UI.font.MeasureString(title);
                Vector2 imageRenderPosition = new Vector2(
                    Main.screenSize.Width - item.value.sprite.Width - 25f,
                    (int)(item.index * 50f) + (Main.screenSize.Height / 2f) + (stringRect.Y / 2f)
                    );
                Vector2 textRenderedPosition = new Vector2(
                    Main.screenSize.Width - stringRect.X - (item.value.sprite.Bounds.Width * 5f) - 30f,
                    (int)(item.index * 50f) + (Main.screenSize.Height / 2f)
                    );
                Color color = ((-4f * (float)item.value.age.Percent()) + 4f) * Color.White;
                Main.spriteBatch.DrawString(UI.font, title, textRenderedPosition, color);
                Main.spriteBatch.Draw(item.value.sprite, imageRenderPosition, null, color, 0f, item.value.sprite.Bounds.Center.ToVector2(), 5f, SpriteEffects.None, 0f);
            }
        }
        #endregion

        public Texture2D sprite;

        public Ammo.Type type;
        public int amount;
        public GameValue age;
        public bool active = true;
        /* 0 = Start
         * 1 = End
         */

        public PickupText(Ammo.Type _type, int _amount)
        {
            type = _type;
            age = new GameValue(0, 120, 1, 0);
            amount = _amount;

            sprite = Ammo.spriteTable[type];

            collection.Add(this);
        }

        public void Update()
        {
            age.Regenerate();
            active = (float)age.Percent() != 1f;
        }
    }
}
