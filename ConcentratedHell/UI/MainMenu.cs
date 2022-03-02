using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell.UI
{
    class MainMenu
    {
        public static List<Button> buttons;

        public static void Initialize()
        {
            buttons = new List<Button>()
            {
                new Button(new Rectangle(960, 540, 200, 100), "Play", () => {
                    Universe.state = Universe.GameState.Playing;
                    Main.StaticInitialize();
                }),
                new Button(new Rectangle(960, 690, 200, 100), "Exit", () => {
                    Main.Instance.Exit();
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
            foreach(Button x in buttons)
            {
                x.Draw();
            }
        }
    }
}
