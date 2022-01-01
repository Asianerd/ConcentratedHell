using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell
{
    class Weapon
    {
        public static Dictionary<Type, Weapon> weaponTable; // Must remain constant

        public virtual void Fire()
        {

        }

        public virtual void Reload()
        {

        }

        public enum Type
        {
            M16
        }
    }
}
