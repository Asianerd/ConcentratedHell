using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ConcentratedHell.Combat;

namespace ConcentratedHell
{
    class Player
    {
        #region Static
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
        #endregion

        public Rectangle rect;
        public static Vector2 size;
        public float direction = 0f; // Radians
        public float speed = 5;
        public bool hasMovementInput = false;

        public Dictionary<Ammo.Type, int> ammoInventory;
        public Dictionary<Weapon.Type, Weapon> arsenal;
        public Weapon equippedWeapon = null;

        Player()
        {
            rect = new Rectangle(0, 0, 64, 64);
            size = new Vector2(64, 64);

            ammoInventory = new Dictionary<Ammo.Type, int>();
            foreach (Ammo.Type x in Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>())
            {
                ammoInventory.Add(x, 100);
            }

            arsenal = new Dictionary<Weapon.Type, Weapon>();
        }

        public void Update()
        {
            speed = 8f * (Main.keyboardState.IsKeyDown(Keys.LeftControl) ? 0.5f : 1f) * Universe.speedMultiplier;

            if (!UI.UI.selectionActive)
            {
                if (equippedWeapon != null)
                {
                    equippedWeapon.Update();
                }
            }

            if (MouseInput.RMouse.active)
            {
                var x = new Cyborg(new Rectangle(Cursor.Instance.worldPosition.ToPoint(), rect.Size));
            }

            Move();
            if (Input.inputs[Keys.LeftShift].active)
            {
                Skill.ExecuteSkill(Skill.Type.Dash);
            }
        }

        public void Move()
        {
            Vector2 targetVelocity = Vector2.Zero;
            foreach (Keys key in Input.playerInputKeys)
            {
                if (Main.keyboardState.IsKeyDown(key))
                {
                    targetVelocity += Input.directionalVectors[Input.keyDirections[key]];
                }
            }
            hasMovementInput = targetVelocity != Vector2.Zero;
            if (hasMovementInput)
            {
                CalculateDirection(Vector2.Zero, targetVelocity);
                targetVelocity.Normalize();
            }
            else
            {
                return;
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

        void CalculateDirection(Vector2 start, Vector2 end)
        {
            direction = MathF.Atan2(end.Y - start.Y, end.X - start.X);
        }

        public void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
            if(equippedWeapon != null)
            {
                Vector2 renderedPosition = new Vector2(

                    );
                Main.spriteBatch.Draw(equippedWeapon.sprite, rect.Location.ToVector2(), Color.White);
            }
        }

        #region Weapon-related
        public void AddWeapon(Weapon weapon)
        {
            if (!arsenal.Keys.Contains(weapon.type))
            {
                arsenal.Add(weapon.type, weapon);
                equippedWeapon = arsenal[weapon.type];

                UI.SelectionWheel.SelectionWheel.Instance.UpdateSections();
            }
        }

        public void EquipWeapon(Weapon.Type type)
        {
            equippedWeapon = arsenal[type];
        }
        #endregion
    }
}
