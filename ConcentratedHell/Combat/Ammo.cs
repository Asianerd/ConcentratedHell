using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcentratedHell
{
    class Ammo
    {
        public static Dictionary<Type, Texture2D> spriteTable;
        public static Dictionary<Type, string> names;

        public static void LoadContent()
        {
            string ammoAssetPath = "Pickups/Ammo";
            spriteTable = new Dictionary<Type, Texture2D>();
            foreach(Type x in Enum.GetValues(typeof(Type)).Cast<Type>())
            {
                spriteTable.Add(x, Main.Instance.Content.Load<Texture2D>($"{ammoAssetPath}/{x.ToString().ToLower()}"));
            }

            names = new Dictionary<Type, string>()
            {
                { Type.Small, "Small" },
                { Type.Medium, "Medium" },
                { Type.Large, "Large" },
                { Type.Shell, "Shell" },
                { Type.Rocket, "Rocket" },
                { Type.Plasma, "Plasma" },
            };
        }

        public enum Type
        {
            Small,
            Medium,
            Large,
            Shell,
            Rocket,
            Plasma
        }
    }
}
