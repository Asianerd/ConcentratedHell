﻿using System;
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

        public Vector2 position;
        public Vector2 target;

        public Camera()
        {
            position = Vector2.Zero;
            target = Vector2.Zero;

            Main.UpdateEvent += Update;
        }

        public void Update()
        {
            target = Player.Instance.rect.Location.ToVector2();
            position = Vector2.Lerp(position, target, 0.2f);
            
        }
    }
}
