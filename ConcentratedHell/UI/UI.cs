using System;
using System.Collections.Generic;
using System.Text;
using ConcentratedHell.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcentratedHell.UI
{
    class UI
    {
        public static UI Instance;
        public static Rectangle viewport;
        public static SpriteFont font;
        public static bool selectionActive;
        public static Color errorColor;

        public static Texture2D healthbarSprite;
        public static Texture2D healthbarCase;
        public static Rectangle healthbarRect;

        public static bool showDebug = false;
        public static float fps;

        public static void Initialize()
        {
            Instance = new UI();
            Cursor.Initialize();
            SelectionWheel.SelectionWheel.Initialize();

            errorColor = new Color(184, 30, 17);

            healthbarSprite = Main.Instance.Content.Load<Texture2D>("UI/Healthbar/healthbar");
            healthbarCase = Main.Instance.Content.Load<Texture2D>("UI/Healthbar/healthbar_case");
            healthbarRect = new Rectangle(0, 0, 780, 50);

            PickupText.Initialize();

            Main.UpdateEvent += Update;
        }

        public static void LoadContent(SpriteFont _font)
        {
            font = _font;
        }

        public static void Update()
        {
            PickupText.StaticUpdate();
            Cursor.Instance.Update();
            SelectionWheel.SelectionWheel.Instance.Update();
            selectionActive = SelectionWheel.SelectionWheel.Instance.progress.Percent() > 0;

            viewport = new Rectangle(Camera.Instance.position.ToPoint() - (Main.screenSize.Size.ToVector2()/2f).ToPoint(), Main.screenSize.Size);
        }

        public static void Draw()
        {
            if (Player.Instance.equippedWeapon != null)
            {
                int ammoAmount = Player.Instance.ammoInventory[Player.Instance.equippedWeapon.ammoType];
                string ammoText = ammoAmount > 0 ? ammoAmount.ToString() : "Out of ammo!";
                Vector2 origin = font.MeasureString(ammoText) / 2f;
                Vector2 renderedPosition = Cursor.Instance.screenPosition + new Vector2(2, 50);
                Main.spriteBatch.DrawString(font, ammoText, renderedPosition, ammoAmount >= Player.Instance.equippedWeapon.ammoUsage ? Color.White : errorColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(healthbarSprite, new Rectangle(
                20,
                20,
                (int)(healthbarRect.Width * Player.Instance.health.Percent()),
                healthbarRect.Height
                ), Color.White);
            Main.spriteBatch.Draw(healthbarCase, new Vector2(20, 20), null, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            SelectionWheel.SelectionWheel.Instance.Draw();

            Cursor.Instance.Draw();
            PickupText.StaticDraw();

            string debugText = $"{fps}\n\nEntities : {Entity.Entity.collection.Count}\nProjectiles : {Combat.Projectiles.Projectile.collection.Count}\nParticles : {Particles.Particle.particles.Count}";
            if (showDebug)
            {
                Main.spriteBatch.DrawString(UI.font, debugText, Vector2.Zero, Color.White);
            }
        }
    }
}
