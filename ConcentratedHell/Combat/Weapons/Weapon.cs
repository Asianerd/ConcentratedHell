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
        public GameValue cooldown;

        public Weapon(Type _type, Projectile.Type _projectileType, Ammo.Type _ammoType, GameValue _cooldown)
        {
            type = _type;
            projectileType = _projectileType;
            ammoType = _ammoType;
            cooldown = _cooldown;

            sprite = spriteTable[type];
        }

        public virtual void Update()
        {
            cooldown.Regenerate();

            if (MouseInput.LMouse.isPressed && (cooldown.Percent() == 1f))
            {
                Fire(Player.Instance.rect.Center.ToVector2());
                cooldown.AffectValue(0f);
            }
        }

        public virtual void Fire(Vector2 origin) // Firing origin; The user's position
        {

        }

        public virtual void Reload()
        {

        }

        public enum Type
        {
            Shotgun,
            Plasma_Rifle
        }
    }
}
