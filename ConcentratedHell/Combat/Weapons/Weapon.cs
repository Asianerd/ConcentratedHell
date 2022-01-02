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

        public Weapon(Type _type, Projectile.Type _projectileType)
        {
            type = _type;
            projectileType = _projectileType;

            sprite = spriteTable[type];
        }

        public virtual void Update()
        {
            if (MouseInput.LMouse.active)
            {
                Fire(Player.Instance.rect.Center.ToVector2());
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
