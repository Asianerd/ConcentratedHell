using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell
{
    class Dash : Skill
    {
        GameValue dashPower;
        bool dashing = false;
        float dashSpeed = 30f;

        public Dash():base()
        {
            dashPower = new GameValue(0, 5, -1, 100);
        }

        public override void Execute()
        {
            if(!Player.Instance.hasMovementInput)
            {
                return;
            }
            if (cooldown.Percent() >= 0.5f)
            {
                cooldown.I -= cooldown.PercentToValue(0.5f);
                dashing = true;
            }
        }

        public override void Update()
        {
            base.Update();

            if (dashing)
            {
                ApplyDash();

                dashPower.Regenerate(Universe.speedMultiplier);
                if (dashPower.Percent() <= 0)
                {
                    dashing = false;
                }
            }
            else
            {
                if (dashPower.Percent() < 1)
                {
                    dashPower.AffectValue(1d);
                }
            }
        }

        public void ApplyDash()
        {
            /*Point increment = new Point(
                (int)(MathF.Cos(Player.Instance.direction) * dashSpeed * Universe.speedMultiplier),
                (int)(MathF.Sin(Player.Instance.direction) * dashSpeed * Universe.speedMultiplier)
                );*/
            Vector2 increment = new Vector2(
                MathF.Cos(Player.Instance.direction) * dashSpeed * Universe.speedMultiplier,
                MathF.Sin(Player.Instance.direction) * dashSpeed * Universe.speedMultiplier
                );

            Rectangle target = new Rectangle(
                (Player.Instance.position + increment).ToPoint(),
                Player.Instance.rect.Size
                );

            if(Map.IsValidPosition(target))
            {
                Player.Instance.rect.Location = target.Location;
                Player.Instance.position += increment;
            }
        }
    }
}
