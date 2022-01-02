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
        public static Texture2D overlay;
        public static Vector2 origin;

        public static void Initialize()
        {
            Instance = new SelectionWheel();
            sprite = Main.Instance.Content.Load<Texture2D>("UI/radialWheelSelection");
            overlay = Main.Instance.Content.Load<Texture2D>("UI/overlay");
            origin = Main.screenSize.Center.ToVector2();
        }
        #endregion

        List<WheelSection> sections;
        public GameValue progress;
        public float cursorAngle = 0;

        public SelectionWheel()
        {
            sections = new List<WheelSection>();
            progress = new GameValue(0, 10, 1, 0);
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

                foreach (WheelSection x in sections)
                {
                    x.Update();
                }
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

            float _c = (float)progress.Percent();
            Color color = new Color(_c, _c, _c, _c);
            Main.spriteBatch.Draw(sprite, Main.screenSize.Center.ToVector2(), null, color, 0f, sprite.Bounds.Size.ToVector2() / 2f, (float)progress.Percent(), SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(overlay, Main.screenSize, color);

            foreach(WheelSection x in sections)
            {
                x.Draw(250f * (float)progress.Percent(), (float)progress.Percent());
            }
        }
    }
}
