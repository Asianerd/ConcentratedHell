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

        public static void Initialize()
        {
            Instance = new UI();
            Cursor.Initialize();
            SelectionWheel.SelectionWheel.Initialize();

            Main.UpdateEvent += Update;
        }

        public static void LoadContent(SpriteFont _font)
        {
            font = _font;
        }

        public static void Update()
        {
            Cursor.Instance.Update();
            SelectionWheel.SelectionWheel.Instance.Update();
            selectionActive = SelectionWheel.SelectionWheel.Instance.progress.Percent() > 0;
        }

        public static void Draw()
        {
            SelectionWheel.SelectionWheel.Instance.Draw();

            Cursor.Instance.Draw();
        }
    }
}
