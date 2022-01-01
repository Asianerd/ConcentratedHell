using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class UI
    {
        public static UI Instance;
        public static SpriteFont font;

        public static void Initialize()
        {
            Instance = new UI();
            Cursor.Initialize();

            Main.UpdateEvent += Update;
        }

        public static void LoadContent(SpriteFont _font)
        {
            font = _font;
        }

        public static void Update()
        {
            Cursor.Instance.Update();
        }

        public static void Draw()
        {
            Cursor.Instance.Draw();
        }
    }
}
