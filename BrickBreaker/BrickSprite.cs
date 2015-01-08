using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * BrickSprite.cs
 * 
 * Draw the bricks using all of the properties from base class Sprite 
 * 
*/
namespace BrickBreaker
{
    class BrickSprite : Sprite
    {
        // Call the base class default constructor
        public BrickSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, null)
        { }

        // call the base class overloaded constructor
        public BrickSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondPerFrame, null)
        { }

        public override Vector2 direction
        {
            get
            {
                return speed;
            }
        }

        // overriding the update method in the base class 
        public override void Update(GameTime gameTme, Rectangle clientBounds)
        {
            position += direction;
            base.Update(gameTme, clientBounds);
        }

    }
}