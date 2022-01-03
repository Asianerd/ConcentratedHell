using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConcentratedHell.Combat.Projectiles;

namespace ConcentratedHell.Combat
{
    class Weapon
    {
        /* Handguns possibly?
         * 
         * Railgun
         * Burst rifle
         * Rocket launcher
         * Grenade launcher
         *      - Possible ice launcher?
         * Amogus shotgun
         * Dildo launcher
         * Sniper
         * Anti-material rifle (BFG-9000 tendrils? / Piercing? / )
         *      - High damage & knockback
         *      - High cooldown
         *      - Special 
         * Gatling gun
         *      - Low cooldown
         *      - Low damage & medium knockback
         * 
         * Kunai/ Shuriken
         *      - Inflict poison?
         * 
         * Grappling hook
         */

        #region Static
        public static Dictionary<Type, Texture2D> spriteTable;

        public static void Initialize()
        {
            string weaponSpritePath = "Weapons";
            spriteTable = new Dictionary<Type, Texture2D>();
            foreach(Type x in Enum.GetValues(typeof(Type)).Cast<Type>())
            {
                spriteTable.Add(x, Main.Instance.Content.Load<Texture2D>($"{weaponSpritePath}/{x.ToString().ToLower()}"));
            }
        }
        #endregion

        public Texture2D sprite;
        public Type type;
        public Projectile.Type projectileType;
        public Ammo.Type ammoType;
        public int ammoUsage;
        public GameValue cooldown;
        public float projectileSpawnDistance;
        public float knockback;

        public Weapon(Type _type, Projectile.Type _projectileType, float _projectileSpawnDistance, float _knockback, Ammo.Type _ammoType, GameValue _cooldown, int _ammoUsage = 1)
        {
            type = _type;
            projectileType = _projectileType;
            projectileSpawnDistance = _projectileSpawnDistance;
            knockback = _knockback;

            ammoType = _ammoType;
            ammoUsage = _ammoUsage;
            cooldown = _cooldown;

            sprite = spriteTable[type];
        }

        public virtual void Update()
        {
            cooldown.Regenerate();

            if (MouseInput.LMouse.isPressed && (cooldown.Percent() == 1f))
            {
                if (Player.Instance.ammoInventory[ammoType] >= ammoUsage)
                {
                    Player.Instance.ammoInventory[ammoType] -= ammoUsage;
                    Fire(new Vector2(
                        (MathF.Cos(Cursor.Instance.playerToCursor) * projectileSpawnDistance) + Player.Instance.rect.Center.X,
                        (MathF.Sin(Cursor.Instance.playerToCursor) * projectileSpawnDistance) + Player.Instance.rect.Center.Y
                        ));
                    cooldown.AffectValue(0f);
                }
            }
        }

        public virtual void Fire(Vector2 origin) // Firing origin; The user's position
        {
            Player.Instance.Knockback(
                MathF.Atan2(
                    Player.Instance.rect.Center.Y - origin.Y,
                    Player.Instance.rect.Center.X - origin.X
                    ),
                knockback * (Main.random.Next(97, 103) / 100f)
                );
        }

        public virtual void AltFire(Vector2 origin)
        {

        }

        public enum Type
        {
            Shotgun,
            Plasma_Rifle,
            Auto_Shotgun,
            Barrett,
            Hell
        }
    }
}
