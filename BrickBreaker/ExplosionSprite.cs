using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * ExplosionSprite.cs
 * 
 * Draw the explosion
 * Using all of the properties of the DrawSpriteOnce base class
 * 
*/
namespace BrickBreaker
{
    class ExplosionSprite : DrawSpriteOnce
    {
        // default constructor
        public ExplosionSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, collisionCueName)
        {}

        // overloaded constructor
        public ExplosionSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondPerFrame, string collisionCueName)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondPerFrame, collisionCueName)
        {}

        public override Vector2 direction
        {
            get
            {
                return speed;
            }
        }

        // overidding the update method in the base class 
        // draw the explosion sprite
        public override void Update(GameTime gameTme, Rectangle clientBounds)
        {
            position += direction;
            base.Update(gameTme, clientBounds);
        }
    }
}
