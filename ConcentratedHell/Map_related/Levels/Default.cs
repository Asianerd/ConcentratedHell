using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ConcentratedHell.Map_related.Levels
{
    class Default:Map
    {
        public Default():base(new List<Tile>() {
            new Tile(new Rectangle(-3024, -2512, 6048, 2032)),  // Up
            new Tile(new Rectangle(-3024, 512, 6048, 2032)),    // Down
            new Tile(new Rectangle(1024, -480, 2032, 992)),     // Right
            new Tile(new Rectangle(-3024, -480, 2032, 992)),    // Left

            new Tile(new Rectangle(-760, -120, 240, 240)),
            new Tile(new Rectangle(520, -120, 240, 240)),
        },
            Vector2.Zero, new List<Vector2>() {
                new Vector2(-400, -200),
                new Vector2(-400, 200),
                new Vector2(400, -200),
                new Vector2(400, 200)
            })
        {

        }
    }
}
