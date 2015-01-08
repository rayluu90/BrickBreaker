using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
 * UserControlSprite.cs
 * 
 * draw a bar to play the game
 * 
*/
namespace BrickBreaker
{
    class UserControlSprite : Sprite
    {
        int signOfBarSpeed;

        // default constructor
        public UserControlSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, 
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, 
                currentFrame, sheetSize, speed, null)        
        {}

        // overloaded constructor
        public UserControlSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, 
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondPerFrame, null)
        {}

        // constrol the direction of the bar
        public override Vector2 direction
        {
            get 
            {
                Vector2 inputDirection = Vector2.Zero;

                // if left arrow key is pressed
                // move the bar to the left
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    signOfBarSpeed = 0;
                    inputDirection.X -= 1;
                    signOfBarSpeed--;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                // if right arrow key is pressed
                // move the bar to the right
                {
                    signOfBarSpeed = 0;
                    inputDirection.X += 1;
                    signOfBarSpeed++;
                }

                if(Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left))
                    signOfBarSpeed = 0;

                return inputDirection * speed;
            }
        }

        public int getSignOfBarSpeed()
        {
            return this.signOfBarSpeed;
        }

        // // overidding the update method in the base class 
        public override void Update(GameTime gameTime, Rectangle clientBound)
        {
            position += direction;

            // if the bar hit the end of the left screen
            // keep the bar stay still
            if (position.X < 0)
                position.X = 0;

            // if the bar hit the end of the right screen
            // keep the bar
            if (position.X > clientBound.Width - frameSize.X)
                position.X = clientBound.Width - frameSize.X;

            base.Update(gameTime, clientBound);
        }
    }
}
