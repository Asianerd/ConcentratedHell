using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell
{
    class Skill
    {
        public GameValue cooldown;

        public static Dictionary<Type, Skill> skills;

        public static void Initialize()
        {
            skills = new Dictionary<Type, Skill>()
            {
                { Type.Dash, new Dash() }
            };

            Main.UpdateEvent += UpdateSkills;
            Main.MidgroundDrawEvent += DrawSkills;
        }

        public static void ExecuteSkill(Type type)
        {
            skills[type].Execute();
        }

        public static void UpdateSkills()
        {
            foreach(Skill x in skills.Values)
            {
                x.Update();
            }
        }

        public static void DrawSkills()
        {
            foreach(Skill x in skills.Values)
            {
                x.Draw();
            }
        }

        #region Enheritance
        public Skill()
        {
            cooldown = new GameValue(0, 120, 1, 100);
        }

        public virtual void Execute()
        {

        }

        public virtual void Update()
        {
            cooldown.Regenerate(Universe.speedMultiplier);
        }

        public virtual void Draw()
        {

        }
        #endregion

        public enum Type
        {
            Dash
        }
    }
}
