using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
 * Util.cs
 * 
 * Contains methods to help with controlling the game
 * 
*/

namespace BrickBreaker
{
    class Util
    {
        Random random = new Random();

        // load all the bricks 
        // place the bricks over by half of brick's length in the even row
        public void LoadBricks(Game game, ref List<BrickSprite> spriteBricksList, float windowWidth,
            float windowHeight, int brickLength, int brickWidth, ref int numOfBricks)
        {
            // positioning the bricks at half screen
            for (int j = 0; j < (windowHeight / 2) / brickWidth; j++)
            {
                // determine how many bricks can fit accross the screen
                // for each bricks
                for (int i = 0; i <= windowWidth / brickLength; i++)
                {
                    // if the row is an ood (1,3,5,7...)
                    if (j % 2 != 0)
                    {
                        spriteBricksList.Add(new BrickSprite(
                        game.Content.Load<Texture2D>(@"Images/bricks"),
                        // Place the bricks over by half of brick's length
                        new Vector2((i * brickLength - brickLength / 2), j * brickWidth),
                        new Point(brickLength, brickWidth),
                        10,
                        new Point(0, 0),
                        new Point(1, 1),
                        Vector2.Zero));
                        numOfBricks++;
                    }
                    // if the row is even (0,2,4,6..)
                    else
                    {
                        spriteBricksList.Add(new BrickSprite(
                        game.Content.Load<Texture2D>(@"Images/bricks"),
                        // Place the bricks next to each to each other accoording to the bricks' length
                        new Vector2(i * brickLength, j * brickWidth),
                        new Point(brickLength, brickWidth),
                        10,
                        new Point(0, 0),
                        new Point(1, 1),
                        Vector2.Zero));
                        numOfBricks++;
                    }
                } // end second for loop
            } // end first for loop
        } // end loadBricks

        // start the game when the user press the space key bar
        public void startGame(ref bool startUpToken, ref bool removeToken, ref float ballPositionX, float playerPositionX,
            int playerFrameSizeX, int ballFrameSizeX, float putBallOnTopBarY, ref Vector2 ballSpeed,
            Texture2D explosionImage, ref ExplosionSprite explosion, GameTime gameTime, Rectangle gameWindowClientBounds)
        {
            // if first time launch
            if (startUpToken)
            {
                // Update the position of ball where the bar is
                ballPositionX = playerPositionX + ((playerFrameSizeX - ballFrameSizeX) >> 1);

                // if the player presses the space key  
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    // set the explosion where the ball is
                    float putExplosionWithBallX = ballPositionX;
                    float putExplosionWithBallY = putBallOnTopBarY;

                    // set random speed for first time launch
                    float randomSpeedX = (float)random.Next(3, 4);
                    float randomSpeedY = (float)random.Next(-4, -3);

                    ballSpeed = new Vector2(3, -4);

                    // Create an explosion when the bar is launched
                    explosion = new ExplosionSprite(explosionImage,
                    new Vector2(putExplosionWithBallX, putExplosionWithBallY),
                    new Point(64, 64),
                    10,
                    new Point(0, 0),
                    new Point(5, 5),
                    Vector2.Zero, "explosion");

                    // set the startUpToken to false
                    startUpToken = false;
                    removeToken = true;
                }
            }
            // if the game has already started
            else
                // draw the explosion animation 
                explosion.Update(gameTime, gameWindowClientBounds);
        }

        // Reset the ball to the middle of the screen
        public void resetBallPositionMiddle(ref BallSprite ball, float putBallAtMiddleX, float putBallOnTopBarY)
        {
            // the previous speed is not 0, set the speed to 0
            if (ball.speed.X != 0 || ball.speed.Y != 0)
            {
                // set ball speed to 0
                ball.speed = Vector2.Zero;
                // put the ball on top of the bar in the middle of the screen
                ball.position = new Vector2(putBallAtMiddleX, putBallOnTopBarY);
            }
        }

        // Reset the bar to the middle of the screen
        public void resetBarPositionMiddle(ref UserControlSprite player, float putBarAtMiddleX, float putBarAtBottomY) 
        {
            player.position = new Vector2(putBarAtMiddleX, putBarAtBottomY);
        }

        // allow user to remove the some bricks once by pressing down the 'R' key
        public void userRemoveBricks(ref bool removeToken, int numBrickAllowRemoved,
            ref List<BrickSprite> spriteBricksList, ref int Score, ref int numOfBricks)
        {
            // if removeToken is True
            if (removeToken)
            {
                // if there is Brick Sprite in the List
                if (spriteBricksList.Count != 0)
                {
                    // if the user hold the key "R" down
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        // if the removeBrick is >= the bricks in the list, remove all the bricks in the list
                        if (numBrickAllowRemoved >= spriteBricksList.Count)
                        {
                            // Add the whatever the total bricks in the list to the score
                            // remove all the brick in the list
                            Score += spriteBricksList.Count;
                            spriteBricksList.Clear();
                            numOfBricks = 0;
                        }
                        // if the number of removeBrick is < the number of brick in the list
                        else 
                        {
                            // Remove the number of removeBrick
                            for (int i = 0; i < numBrickAllowRemoved; i++)
                            {
                                spriteBricksList.RemoveAt(spriteBricksList.Count - 1);
                                Score++;
                                numOfBricks--;
                            }
                        }
                        // set  theremove token to false so user cannot remove anymore bricks
                        removeToken = false;
                    }
                }
            }
        }

        // create explosion when collide with the bricks
        public void createExplosion(ref List<ExplosionSprite> spriteExplosionList, Texture2D explosionImage ,
            float brickExplosionPositionX, float brickExplosionPositionY)
        {
            spriteExplosionList.Add(new ExplosionSprite(explosionImage,
            new Vector2(brickExplosionPositionX, brickExplosionPositionY),
            new Point(64, 64),
            10,
            new Point(0, 0),
            new Point(5, 5),
            Vector2.Zero,
            1, "explosion"));
        }

        // controlling the ball bounch direction after collide with the bar
        public void ballBounceDirection(ref BallSprite ball, ref UserControlSprite player, Game game )
        {
            // if the ball hit the left side of the screen
            if (ball.position.X < 0)
            {
                //If the X == 0 && Y == 0
                if (ball.getSignOfBallX() == 0 && ball.getSignOfBallY() == 0)
                {
                    // Set ball X && Y to 0
                    ball.resetSignOfBall();

                    // Set X == 1 && Y == -1
                    ball.setSignOfBall(1, -1);
                }
                else
                {
                    ball.resetSignOfBall();
                    ball.setSignOfBall(1,1);
                }
                // reverse the X direction of the ball
                ball.speed.X *= -1;
            }
            // if the ball hit the right side of the screen
            else if (ball.position.X > game.Window.ClientBounds.Width - ball.frameSize.X)
            {
                //If the X == 0 && Y == 0
                if ( ball.getSignOfBallX() == 0 &&  ball.getSignOfBallY() == 0)
                {
                     ball.resetSignOfBall();
                     ball.setSignOfBall(1,-1);
                }
                else
                {
                     ball.resetSignOfBall();
                     ball.setSignOfBall(-1,1);
                }
                // reverse the X direction of the ball
                ball.speed.X *= -1;
            }
            // if the y position of the ball hit the top of the screen
            else if (ball.position.Y < 0)
            {

                //If the X == -1 && Y == -1
                if ( ball.getSignOfBallX() < 0 &&  ball.getSignOfBallY() < 0)
                {
                     ball.resetSignOfBall();
                     ball.setSignOfBall(-1,1);
                }
                //If the X == 1 && Y == -1
                else if (ball.getSignOfBallX() > 0 &&  ball.getSignOfBallY() < 0)
                {
                     ball.resetSignOfBall();
                     ball.setSignOfBall(1,1);
                }
                // reverse the Y direction of the ball
                ball.speed.Y *= -1;
            }

            // if the ball collide with bar
            if (ball.collisionRect.Intersects(player.collisionRect))
            {
                // if the bar is stationary
                // reverse the Y direction of the ball
                if (player.getSignOfBarSpeed() == 0)
                    ball.speed.Y *= -1;

                // if the bar is comning to the right
                else if (player.getSignOfBarSpeed() > 0)
                {
                    // If the X == 1 && Y == 1
                    if (ball.getSignOfBallX() > 0 && ball.getSignOfBallY() > 0) 
                    {
                        // reverse the Y direction of the ball
                        ball.speed.Y *= -1;
                    }
                    // if the sign of the ball, x and y, are both 0 
                    else if (ball.getSignOfBallX() == 0 && ball.getSignOfBallY() == 0)
                    {
                        // reverse the Y direction of the ball
                        ball.speed.Y *= -1;
                    }
                    else
                    {
                        // reverse the direction of the ball
                        ball.speed *= -1;
                    }
                    ball.resetSignOfBall();
                }

                // if the bar is comning to the from the left, bounce the ball back to the left
                else if (player.getSignOfBarSpeed() < 0)
                {
                    //If the X == -1 && Y == 1
                    if (ball.getSignOfBallX() < 0 && ball.getSignOfBallY() > 0)
                        // reverse the Y direction of the ball
                        ball.speed.Y *= -1;
                    else if (ball.getSignOfBallX() == 0 && ball.getSignOfBallY() == 0)
                        // reverse the Y direction of the ball
                        ball.speed.Y *= -1;
                    else
                        // reverse the direction of the ball
                        ball.speed *= -1;
                }

                ball.resetSignOfBall();
            }
        }   
    }
}
