using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Sprite.cs
 * 
 * Base class for creating different sprite sheet
*/
namespace BrickBreaker
{
    abstract class Sprite
    {
        Texture2D textureImage;
        public Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondPerFrame;
        const int defaultMillisecondsPerframe = 16;
        public Vector2 speed;
        public Vector2 position;

        // Defaul constructor
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, String collisionCueName)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerframe, collisionCueName)
        {
        }

        // Custom constructor
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondPerFrame, string collisionCueName)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.collisionCueName = collisionCueName;
            this.millisecondPerFrame = millisecondPerFrame;
        }
        
        // adjust the frame speed
        public virtual void Update(GameTime gameTme, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTme.ElapsedGameTime.Milliseconds;
            
            // if the current time frame is greater than 
            // the default or input time frame
            if (timeSinceLastFrame > millisecondPerFrame)
            {
                // set the current time back to zeros
                timeSinceLastFrame = 0;

                // move to the next frame in x direction
                ++currentFrame.X;

                // if current frame is out of bound in the x direction
                if (currentFrame.X >= sheetSize.X)
                {
                    // set the current frame in the x direction back to 0 position
                    currentFrame.X = 0;
                    
                    // move current frame down to the next row, y direction
                    ++currentFrame.Y;

                    // if current frame in the y direction is out of bound
                    if (currentFrame.Y >= sheetSize.Y)
                        // set the current frame in the y direction back to 0 position
                        currentFrame.Y = 0;
                }
            }
        }

        // draw the sprite
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                    Color.White, 
                    0, 
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None, 
                    0);
        }

        
        public string collisionCueName { 
            get; 
            private set; 
        }

        // represent which spirte direction is moving
        public abstract Vector2 direction
        {
            get;
        }
        
        // create a rectange for collision detection
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle( (int)position.X + collisionOffset,
                                      (int)position.Y + collisionOffset,
                                      frameSize.X - (collisionOffset * 2),
                                      frameSize.Y - (collisionOffset * 2));
                                        
            }
        }

    }
}
