using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell
{
    class Player
    {
        public static Player Instance = null;
        public static Texture2D sprite;
        
        public static void Initialize()
        {
            if(Instance == null)
            {
                Instance = new Player();

                Main.UpdateEvent += Instance.Update;
                Main.DrawEvent += Instance.Draw;
            }
        }

        public static void LoadContent(Texture2D _sprite)
        {
            sprite = _sprite;
        }

        public Rectangle rect;
        public static Vector2 size;
        //public Vector2 position;
        public float speed = 5;

        Player()
        {
            rect = new Rectangle(0, 0, 64, 64);
            size = new Vector2(64, 64);
        }

        public void Update()
        {
            speed = 8f * (Main.keyboardState.IsKeyDown(Keys.LeftControl) ? 0.5f : 1f);

            ImprovedPlayerMovement();
            if(Input.inputs[Keys.LeftShift].active)
            {
                Debug.WriteLine("Shift is being pressed");
                Skill.ExecuteSkill(Skill.Type.Dash);
            }
        }

        public void ImprovedPlayerMovement()
        {
            Vector2 targetVelocity = Vector2.Zero;
            foreach (Keys key in Input.playerInputKeys)
            {
                if (Main.keyboardState.IsKeyDown(key))
                {
                    targetVelocity += Input.directionalVectors[Input.keyDirections[key]];
                }
            }
            if (targetVelocity != Vector2.Zero)
            {
                targetVelocity.Normalize();
            }


            Rectangle targetRectangle = new Rectangle((rect.Location.ToVector2() + (targetVelocity * speed)).ToPoint(), rect.Size);

            if(Map.IsValidPosition(targetRectangle))
            {
                rect.Location = targetRectangle.Location;
                return;
            }

            Vector2 xVel = new Vector2(targetVelocity.X, 0);
            Rectangle xRect = new Rectangle((rect.Location.ToVector2() + (xVel * speed)).ToPoint(), rect.Size);
            if (Map.IsValidPosition(xRect))
            {
                rect.Location = xRect.Location;
            }

            Vector2 yVel = new Vector2(0, targetVelocity.Y);
            Rectangle yRect = new Rectangle((rect.Location.ToVector2() + (yVel * speed)).ToPoint(), rect.Size);
            if (Map.IsValidPosition(yRect))
            {
                rect.Location = yRect.Location;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(sprite, rect, Color.White);
        }
    }
}
