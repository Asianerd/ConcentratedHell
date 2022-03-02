using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.UI
{
    class PauseMenu
    {
        static Texture2D darkOverlay;
        static List<Button> buttons;

        public static void Initialize()
        {
            darkOverlay = Main.Instance.Content.Load<Texture2D>("UI/PauseMenu/overlay");

            buttons = new List<Button>()
            {
                new Button(new Rectangle(100, 900, 500, 100), "Return to main menu", () => {
                    Universe.state = Universe.GameState.Main_menu;
                    Universe.paused = false;
                }),
            };
        }

        public static void Update()
        {
            foreach(Button x in buttons)
            {
                x.Update();
            }
        }

        public static void Draw()
        {
            Main.spriteBatch.Draw(darkOverlay, Main.screenSize, Color.White);
            Main.spriteBatch.DrawString(UI.font, "Paused", Vector2.One * 100, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            Main.spriteBatch.DrawString(UI.font, "F4 to close the game", new Vector2(130, 230), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            foreach(Button x in buttons)
            {
                x.Draw();
            }
        }
    }
}
