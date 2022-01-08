using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcentratedHell.Combat.Weapons
{
    class Gatling_gun:Weapon
    {
        public Gatling_gun():base(Type.Gatling_Gun, Projectiles.Projectile.Type.Small_round, 100f, 0.5f, Ammo.Type.Small, new GameValue(0, 1, 1), _ammoUsage:3)
        {

        }

        public override void ExecuteFire()
        {
            Player.Instance.ammoInventory[ammoType] -= ammoUsage;
            Vector2 origin = Vector2.Zero;
            Vector2 mainOrigin = new Vector2(
                    (MathF.Cos(Cursor.Instance.playerToCursor) * projectileSpawnDistance) + Player.Instance.rect.Center.X,
                    (MathF.Sin(Cursor.Instance.playerToCursor) * projectileSpawnDistance) + Player.Instance.rect.Center.Y
                    );
            float _directionalCorrecter = MathF.Abs(Cursor.Instance.playerToCursor) > MathF.PI / 2f ? -1 : 1;
            for (int i = 0; i < 3; i++)
            {
                origin = new Vector2(
                    (MathF.Cos(Cursor.Instance.playerToCursor + (i * 0.10f * _directionalCorrecter)) * projectileSpawnDistance) + Player.Instance.rect.Center.X,
                    (MathF.Sin(Cursor.Instance.playerToCursor + (i * 0.10f * _directionalCorrecter)) * projectileSpawnDistance) + Player.Instance.rect.Center.Y
                    );
                Fire(origin);
            }
            cooldown.AffectValue(0f);

            Player.Instance.Knockback(
                MathF.Atan2(
                    Player.Instance.rect.Center.Y - mainOrigin.Y,
                    Player.Instance.rect.Center.X - mainOrigin.X
                    ),
                knockback * (Main.random.Next(97, 103) / 100f)
                );
        }

        public override void Fire(Vector2 origin)
        {
            base.Fire(origin);

            var x = new Projectiles.Small_round(origin, Cursor.Instance.playerToCursor + (MathF.PI * 2f * (Main.random.Next(995, 1005) / 1000f)));
            for (int i = 0; i < 3; i++)
            {
                var y = new Particles.MuzzleFlashParticle(origin, Cursor.Instance.playerToCursor + (Main.random.Next(-400, 400) / 1000f), Main.random.Next(400, 1600) / 100f);
            }
        }
    }
}
