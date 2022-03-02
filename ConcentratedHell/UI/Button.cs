using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.UI
{
    class Button
    {
        public static List<Texture2D> slices = new List<Texture2D>();

        public static void Initialize()
        {
            slices = new List<Texture2D>();
            for(int i = 1; i <= 9; i++)
            {
                slices.Add(Main.Instance.Content.Load<Texture2D>($"UI/Button/9slice/{i}"));
            }
        }

        string text;
        Rectangle rect;
        Action action;

        public bool active;
        public bool isPressed;
        bool wasPressed;

        public Button(Rectangle _rect, string _text, Action _action)
        {
            rect = _rect;
            text = _text;
            action = _action;
        }

        public void Update()
        {
            wasPressed = isPressed;
            isPressed = rect.Contains(Cursor.Instance.screenPosition) && MouseInput.LMouse.isPressed;

            active = isPressed && !wasPressed;

            if(active)
            {
                action.Invoke();
            }
        }

        public void Draw()
        {
            Color color = isPressed ? Color.Gray : Color.White;
            int pixel = 8;
            int width = rect.Width - (pixel * 2);
            int height = rect.Height - (pixel * 2);
            Main.spriteBatch.Draw(slices[0], new Rectangle(new Point(rect.X, rect.Y), new Point(pixel, pixel)), color);
            Main.spriteBatch.Draw(slices[1], new Rectangle(new Point(rect.X + pixel, rect.Y), new Point(width, pixel)), color);
            Main.spriteBatch.Draw(slices[2], new Rectangle(new Point(rect.Right - pixel, rect.Y), new Point(pixel, pixel)), color);
            Main.spriteBatch.Draw(slices[3], new Rectangle(new Point(rect.X, rect.Y + pixel), new Point(pixel, height)), color);
            Main.spriteBatch.Draw(slices[4], new Rectangle(new Point(rect.X + pixel, rect.Y + pixel), new Point(width, height)), color);
            Main.spriteBatch.Draw(slices[5], new Rectangle(new Point(rect.Right - pixel, rect.Y + pixel), new Point(pixel, height)), color);
            Main.spriteBatch.Draw(slices[6], new Rectangle(new Point(rect.X, rect.Bottom - pixel), new Point(pixel, pixel)), color);
            Main.spriteBatch.Draw(slices[7], new Rectangle(new Point(rect.X + pixel, rect.Bottom - pixel), new Point(width, pixel)), color);
            Main.spriteBatch.Draw(slices[8], new Rectangle(new Point(rect.Right - pixel, rect.Bottom - pixel), new Point(pixel, pixel)), color);

            //Main.spriteBatch.Draw(sprite, rect, isPressed ? Color.Gray : Color.White);

            Main.spriteBatch.DrawString(UI.font, text, rect.Center.ToVector2(), isPressed?Color.LightGray:Color.White, 0f, UI.font.MeasureString(text)/2f, 1f, SpriteEffects.None, 0f);
        }
    }
}
