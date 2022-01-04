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
        public static List<Texture2D> playerEyeSprites;
        
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

            string _playerEyeAssetsPath = "Player/Blink";
            playerEyeSprites = new List<Texture2D>();
            for (int i = 1; i <= 5; i++)
            {
                playerEyeSprites.Add(Main.Instance.Content.Load<Texture2D>($"{_playerEyeAssetsPath}/blink{i}"));
            }
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

        public GameValue eyeBlink;

        Player()
        {
            rect = new Rectangle(-96, 288, 64, 64);
            size = new Vector2(64, 64);

            ammoInventory = new Dictionary<Ammo.Type, int>()
            {
                { Ammo.Type.Small, 50 },
                { Ammo.Type.Medium, 40 },
                { Ammo.Type.Large, 14 },
                { Ammo.Type.Shell, 30 },
                { Ammo.Type.Rocket, 6 },
                { Ammo.Type.Plasma, 80 },
            };
            /*foreach (Ammo.Type x in Enum.GetValues(typeof(Ammo.Type)).Cast<Ammo.Type>())
            {
                ammoInventory.Add(x, 50);
            }*/

            arsenal = new Dictionary<Weapon.Type, Weapon>();

            eyeBlink = new GameValue(0, 120, 1);
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

            /*if (MouseInput.RMouse.active)*/
            /*if (MouseInput.RMouse.active)
            {
                var x = new Cyborg(new Rectangle(Cursor.Instance.worldPosition.ToPoint(), rect.Size));
            }*/

            if (Main.keyboardState.IsKeyDown(Keys.T))
            {
                var x = new Cyborg(new Rectangle(Cursor.Instance.worldPosition.ToPoint(), rect.Size));
            }

            Move();
            if (Input.inputs[Keys.LeftShift].active)
            {
                Skill.ExecuteSkill(Skill.Type.Dash);
            }

            eyeBlink.Regenerate(0.2f);
            if (eyeBlink.Percent() >= 1f)
            {
                eyeBlink.AffectValue(0f);
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

        public void Knockback(float _direction, float _distance)
        {
            Vector2 targetVelocity = new Vector2(
                MathF.Cos(_direction) * _distance,
                MathF.Sin(_direction) * _distance
                );

            Rectangle targetRectangle = new Rectangle((rect.Location.ToVector2() + (targetVelocity * speed)).ToPoint(), rect.Size);

            if (Map.IsValidPosition(targetRectangle))
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

        public void Draw()
        {
            Main.spriteBatch.Draw(sprite, rect, Color.White);
            Vector2 eyePosition = new Vector2(
                (MathF.Cos(Cursor.Instance.playerToCursor) * 5f) + rect.Center.X,
                (MathF.Sin(Cursor.Instance.playerToCursor) * 5f) + rect.Center.Y - 5f
                );
            // sprites = 1, 2, 3, 4, 5(o)
            // blink = 1, 2, 3, 4, 5, 6, 7, 8, 9..120
            Texture2D _eyeSprite = playerEyeSprites[(eyeBlink.I >= playerEyeSprites.Count ? (playerEyeSprites.Count - 1) : (int)eyeBlink.I)];
            Main.spriteBatch.Draw(
                _eyeSprite,
                eyePosition,
                null,
                Color.White,
                0f,
                _eyeSprite.Bounds.Center.ToVector2(),
                5f,
                SpriteEffects.None,
                0f
                );
            if(equippedWeapon != null)
            {
                float distance = 45f + (5f * (float)equippedWeapon.cooldown.Percent());
                Vector2 renderedPosition = new Vector2(
                    (MathF.Cos(Cursor.Instance.playerToCursor) * distance) + rect.Center.X,
                    (MathF.Sin(Cursor.Instance.playerToCursor) * distance) + rect.Center.Y
                    );
                Main.spriteBatch.Draw(
                    equippedWeapon.sprite,
                    renderedPosition,
                    null,
                    Color.White,
                    Cursor.Instance.playerToCursor,
                    equippedWeapon.sprite.Bounds.Center.ToVector2(),
                    5f,
                    Math.Abs(Cursor.Instance.playerToCursor) >= (Math.PI / 2f) ? SpriteEffects.FlipVertically : SpriteEffects.None,
                    0f);
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

        #region Ammo
        public void AffectAmmo(Ammo.Type type, int amount)
        {
            ammoInventory[type] += amount;
            UI.PickupText.AppendItem(type, amount);
        }
        #endregion
    }
}
