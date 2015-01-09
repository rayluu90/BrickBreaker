using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*
 * BallSprite.cs
 * 
 * Draw a fire ball using all of the properties from base class Sprite
 * 
*/
namespace BrickBreaker
{
    class BallSprite: Sprite
    {
        int signOfBallX, signOfBallY;

        // Call the base class default constructor
        public BallSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, 
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,
                currentFrame, sheetSize, speed, null)        
        {}

        // call the base class overloaded constructor
        public BallSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondPerFrame, null)
        {}

        // overide the direction property in the base class
        // return the speech of the ball
        public override Vector2 direction
        {
            get
            {
                return this.speed;
            }
        }

        // sign of the ball in the x direction
        public int getSignOfBallX()
        {
            return this.signOfBallX;
        }

        // sign of the ball in the y direction
        public int getSignOfBallY()
        {
            return this.signOfBallY;
        }

        // reset the the sign of the ball
        public void resetSignOfBall()
        {
            int xx = 0;
            int yy = 0;

            setSignOfBall(xx,yy);
        }

        // set the sign of the ball
        public void setSignOfBall(int x, int y)
        {
            this.signOfBallX = x;
            this.signOfBallY = y;
        }

        // overidding the update method in the base class 
        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            // draw the ball as the ball move
            position += direction;
            
            // call the base update method
            base.Update(gameTime, clientBound);
        }

    }
}
