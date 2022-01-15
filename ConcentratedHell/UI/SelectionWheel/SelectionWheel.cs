using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell.UI.SelectionWheel
{
    class SelectionWheel
    {
        #region Static
        public static SelectionWheel Instance;
        public static Texture2D sprite;
        public static Texture2D weaponStatSprite;
        public static Texture2D overlay;
        public static Vector2 origin;

        public static string weaponStats;

        public static void Initialize()
        {
            Instance = new SelectionWheel();
            sprite = Main.Instance.Content.Load<Texture2D>("UI/radialWheelSelection");
            weaponStatSprite = Main.Instance.Content.Load<Texture2D>("UI/weapon_stat_sprite");
            overlay = Main.Instance.Content.Load<Texture2D>("UI/overlay");
            origin = Main.screenSize.Center.ToVector2();
        }
        #endregion

        List<WheelSection> sections;
        public GameValue progress;
        public float cursorAngle = 0;
        public float fullCursorAngle;

        public SelectionWheel()
        {
            sections = new List<WheelSection>();
            progress = new GameValue(0, 10, 1, 0);

            Player.OnWeaponChange += UpdateWeaponStats;
        }

        public void Update()
        {
            progress.Regenerate(Main.keyboardState.IsKeyDown(Keys.Tab) ? 1 : -1);

            Universe.speedMultiplier = Math.Clamp(1f - (float)progress.Percent(), 0.2f, 1f);

            if (progress.Percent() > 0)
            {
                cursorAngle = MathF.Atan2(
                    Cursor.Instance.screenPosition.Y - origin.Y,
                    Cursor.Instance.screenPosition.X - origin.X
                    );

                fullCursorAngle = FullRadian(cursorAngle);

                foreach (WheelSection x in sections)
                {
                    x.Update();
                }
            }
        }

        static float FullRadian(float radian)
        {
            if (radian < 0)
            {
                return (float)((Math.PI - MathF.Abs(radian)) + Math.PI);
            }
            else
            {
                return radian;
            }
        }

        public void UpdateSections()
        {
            sections = new List<WheelSection>();
            float degreeIncrement = 360f / Player.Instance.arsenal.Count;
            float radianIncrement = MathHelper.ToRadians(degreeIncrement);
            foreach (var item in Player.Instance.arsenal.Keys.Select((value, index) => new { value, index }))
            {
                float radianRotation = MathHelper.ToRadians(item.index * degreeIncrement) + (radianIncrement / 2f);

                float min = radianRotation - (radianIncrement / 2f);
                float max = radianRotation + (radianIncrement / 2f);

                sections.Add(new WheelSection(item.value, radianRotation, min, max));
            }
        }

        public void Draw()
        {
            if (progress.Percent() <= 0)
            {
                return;
            }

            float _scale = (float)progress.Percent();
            Color color = new Color(_scale, _scale, _scale, _scale);
            Main.spriteBatch.Draw(sprite, Main.screenSize.Center.ToVector2(), null, color, 0f, sprite.Bounds.Size.ToVector2() / 2f, _scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(overlay, Main.screenSize, color);

            foreach(WheelSection x in sections)
            {
                x.Draw(250f * _scale, _scale, color);
            }

            DrawWeaponStats(color, _scale);
        }

        void DrawWeaponStats(Color color, float scale)
        {
            Color textColor = new Color(Color.Black, scale);

            Rectangle weaponStatRect = new Rectangle((int)(0.75f * Main.screenSize.Width), (Main.screenSize.Height / 2) - 500, 450, 1000);
            Main.spriteBatch.Draw(weaponStatSprite, weaponStatRect, color);
            Vector2 textRect = UI.font.MeasureString(Player.Instance.equippedWeapon.name);
            Main.spriteBatch.DrawString(UI.font, Player.Instance.equippedWeapon.name, new Vector2(weaponStatRect.Center.X, weaponStatRect.Y + 50) - (textRect / 2f), textColor);

            Main.spriteBatch.Draw(Player.Instance.equippedWeapon.sprite, new Vector2(weaponStatRect.Center.X, weaponStatRect.Center.Y - 250f), null, color, 0f, Player.Instance.equippedWeapon.sprite.Bounds.Center.ToVector2(), scale * 10f, SpriteEffects.None, 0f);

            Main.spriteBatch.DrawString(UI.font, weaponStats, weaponStatRect.Center.ToVector2() - new Vector2(190, 100), textColor);
        }

        void UpdateWeaponStats()
        {
            weaponStats = WrapText($"" +
                $"Ammo : {Player.Instance.equippedWeapon.ammoUsage} {Ammo.names[Player.Instance.equippedWeapon.ammoType]}\n" +
                $"Knockback : {Player.Instance.equippedWeapon.knockback}\n" +
                $"Fire rate : {Math.Round(1f/Player.Instance.equippedWeapon.cooldown.rate,3)}/s\n" +
                $"\n" +
                $"{Combat.Weapon.weaponDescriptions[Player.Instance.equippedWeapon.type]}");
        }

        string WrapText(string input)
        {
            string final = "";
            int amount = 0;
            foreach(char x in input)
            {
                final += x;
                if(x == '\n')
                {
                    amount = 0;
                }
                amount++;
                if ((amount >= 22f) && (x == ' '))
                {
                    final += "\n";
                    amount = 0;
                }
            }
            return final;
        }
    }
}
