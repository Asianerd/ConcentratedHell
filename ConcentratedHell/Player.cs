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
        //public Vector2 position;
        public float speed = 5;

        Player()
        {
            rect = new Rectangle(0, 0, 64, 64);
        }

        public void Update()
        {
            speed = 5 * (Main.keyboardState.IsKeyDown(Keys.LeftShift) ? 2 : 1);

            ImprovedPlayerMovement();
        }

        public void PlayerMovement()
        {
            Vector2 target = Vector2.Zero;
            foreach (Keys key in Input.playerInputKeys)
            {
                if (Main.keyboardState.IsKeyDown(key))
                {
                    target += Input.directionalVectors[Input.keyDirections[key]];
                }
            }
            if (target != Vector2.Zero)
            {
                target.Normalize();
            }

            Point targetPosition = (rect.Location.ToVector2() + (target * speed)).ToPoint();
            
            if(Map.IsValidPosition(targetPosition))
            {
                rect.Location = targetPosition;
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


            Point targetPosition = (rect.Location.ToVector2() + (targetVelocity * speed)).ToPoint();

            if (Map.IsValidPosition(targetPosition))
            {
                rect.Location = targetPosition;
                return;
            }

            Vector2 xVel = new Vector2(targetVelocity.X, 0);
            Vector2 yVel = new Vector2(0, targetVelocity.Y);

            if (Map.IsValidPosition((rect.Location.ToVector2() + (xVel * speed)).ToPoint()))
            {
                rect.Location += (xVel * speed).ToPoint();
            }
            else if (Map.IsValidPosition((rect.Location.ToVector2() + (yVel * speed)).ToPoint()))
            {
                rect.Location += (yVel * speed).ToPoint();
            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(sprite, position, null, Color.White, 0f, sprite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);
            spritebatch.Draw(sprite, rect, null, Color.White, 0f, sprite.Bounds.Center.ToVector2(), SpriteEffects.None, 0f);
            //spritebatch.Draw(sprite, position, null, Color.Red, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
        }
    }
}
