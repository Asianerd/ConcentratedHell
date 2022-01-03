using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Particles
{
    class ParticleGroup
    {
        #region Static
        public static List<ParticleGroup> collection;

        public static void Initialize()
        {
            collection = new List<ParticleGroup>();

            Main.UpdateEvent += GroupUpdate;
            Main.DrawEvent += GroupDraw;
        }

        public static void GroupUpdate()
        {
            foreach (ParticleGroup x in collection)
            {
                x.Update();
            }
            collection.RemoveAll(n => !n.active);
        }

        public static void GroupDraw()
        {
            foreach (ParticleGroup x in collection)
            {
                x.Draw();
            }
        }

        public static List<Particle> GenerateGroupParticles(Type type, Vector2 origin)
        {
            List<Particle> final = new List<Particle>();

            switch(type)
            {
                case Type.Smoke:
                    for (int i = 0; i <= 30; i++)
                    {
                        final.Add(new SmokeParticle(origin, (Main.random.Next(0, 100) / 100f) * MathF.PI * 2f, 5f + (Main.random.Next(-200, 200) / 100f)));
                    }
                    break;
                default:
                    break;
            }

            return final;
        }
        #endregion


        public List<Particle> particles;
        public bool active;
        public Type type;

        public ParticleGroup(Type _type, Vector2 origin)
        {
            type = _type;
            particles = GenerateGroupParticles(type, origin);

            collection.Add(this);
        }

        public void Update()
        {
            foreach(Particle x in particles)
            {
                x.Update();
            }
            particles.RemoveAll(n => !n.active);

            active = particles.Where(n => n.active).Count() >= 1;
        }

        public void Draw()
        {
            foreach(Particle x in particles)
            {
                x.Draw();
            }
        }

        public enum Type
        {
            Smoke
        }
    }
}
