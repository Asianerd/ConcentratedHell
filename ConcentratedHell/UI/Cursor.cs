using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Cursor
    {
        public static Cursor Instance;
        public static Texture2D sprite;
        public static Dictionary<Type, Texture2D> cursorSprites;
        public static Dictionary<Ammo.Type, Texture2D> cursorSights;

        public static void Initialize()
        {
            Instance = new Cursor();
        }

        public static void LoadContent()
        {
            cursorSprites = new Dictionary<Type, Texture2D>();
            foreach (Type x in Enum.GetValues(typeof(Type)).Cast<Type>())
            {
                cursorSprites.Add(x, Main.Instance.Content.Load<Texture2D>($"UI/CursorSprites/{x.ToString().ToLower()}"));
            }

            cursorSights = new Dictionary<Ammo.Type, Texture2D>();
            foreach(Ammo.Type x in Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>())
            {
                cursorSights.Add(x, Main.Instance.Content.Load<Texture2D>($"UI/CursorSights/{x.ToString().ToLower()}"));
            }
        }

        public Vector2 screenPosition;
        public Vector2 worldPosition;
        public float playerToCursor;
        public bool systemCursor;

        public void Update()
        {
            screenPosition = Main.mouseState.Position.ToVector2();
            worldPosition = Camera.ScreenToWorld(screenPosition);
            playerToCursor = MathF.Atan2(
                worldPosition.Y - Player.Instance.rect.Center.Y,
                worldPosition.X - Player.Instance.rect.Center.X
                );

            if (Universe.state == Universe.GameState.Playing)
            {
                if (Universe.paused)
                {
                    sprite = cursorSprites[Type.Default];
                    systemCursor = true;
                }
                else
                {
                    if (Player.Instance.equippedWeapon != null)
                    {
                        sprite = cursorSights[Player.Instance.equippedWeapon.ammoType];
                        systemCursor = false;
                    }
                    else
                    {
                        sprite = cursorSprites[Type.Default];
                        systemCursor = true;
                    }
                }
            }
            else
            {
                sprite = cursorSprites[Type.Default];
                systemCursor = true;
            }
        }

        public void Draw()
        {
            Main.spriteBatch.Draw(sprite, screenPosition, null, Color.White, 0f, systemCursor ? Vector2.Zero : sprite.Bounds.Center.ToVector2(), 5f, SpriteEffects.None, 0f);
        }

        public enum Type
        {
            Default
        }
    }
}
