using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class Camera
    {
        public static Camera Instance = null;

        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new Camera();
            }
        }

        public static Vector2 ScreenToWorld(Vector2 position)
        {
            return position - Instance.offset;
        }

        public Vector2 position;
        public Vector2 offset;
        public Vector2 target;

        public Camera()
        {
            position = Player.Instance.rect.Location.ToVector2() + (Player.size / 2f);
            target = Vector2.Zero;

            Main.UpdateEvent += Update;
        }

        public void Update()
        {
            target = Player.Instance.rect.Location.ToVector2() + (Player.size / 2f);
            position = Vector2.Lerp(position, target, 0.2f);
            offset = (Main.screenSize.Size.ToVector2() / 2f) - position;
        }
    }
}
