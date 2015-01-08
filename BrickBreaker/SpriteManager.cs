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
using System.IO;

/*
 * SpriteManager.cs
 * 
 * Managing all of the sprite classes
 * 
*/
namespace BrickBreaker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        UserControlSprite player;
        BallSprite ball;
        ExplosionSprite explosion;
        Util util;
        SpriteFont front;
        Texture2D explosionImage;

        List<ExplosionSprite> spriteExplosionList = new List<ExplosionSprite>();

        List<BrickSprite> spriteBricksList = new List<BrickSprite>();

        // Set the Length and Width for the Bricks
        readonly int brickLength = 60;
        readonly int brickWidth = 17;

        // Set FramSize (X and Y) for the Player Sprite
        readonly int playerFrameSizeX = 80;
        readonly int playerFrameSizeY = 10;
        
        // Set FrameSize (X and Y) for the Ball Sprite
        readonly int ballFrameSizeX = 45;
        readonly int ballFramSizeY = 45;

        // allow user to remove bricks
        private int numBrickAllowRemoved = 7;

        private bool startUpToken = true;
        private bool removeToken;

        private string fileName = "score.txt";
        private string fileContent;
        private int readScore;
       

        public static int Score;
        public static int numOfBricks;
     
        float putBarAtMiddleX;
        float putBarAtBottomY;
        
        float putBallAtMiddleX;
        float putBallOnTopBarY;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // if file doesn't exist
            // set bestscore to 0
            if (!File.Exists(fileName))
                readScore = 0;
            else
            {
                //read from the file 
                fileContent = File.ReadAllText(fileName);

                //Convert the content to integer
                readScore = Convert.ToInt32(fileContent);
            }
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // instantiate Util class
            util = new Util();

            front = Game.Content.Load<SpriteFont>("Front");

            explosionImage = Game.Content.Load<Texture2D>(@"Images/explosion");

            // Set Bar at middile of the screen
            putBarAtMiddleX = (float)(( Game.Window.ClientBounds.Width - playerFrameSizeX ) >> 1);

            // Set Bar at the Bottom
            putBarAtBottomY = (float)( Game.Window.ClientBounds.Height - (playerFrameSizeY << 1));

            // Set ball at the middle of the screen
            putBallAtMiddleX = (float)(( Game.Window.ClientBounds.Width - ballFrameSizeX ) >> 1);
            // Set ball on top of the bar
            putBallOnTopBarY = putBarAtBottomY - (float)ballFramSizeY;

            // Create the fireball 
            ball = new BallSprite(
                Game.Content.Load<Texture2D>( @"Images/FlameBall" ),
                new Vector2( putBallAtMiddleX, putBallOnTopBarY ),
                new Point( ballFrameSizeX, ballFramSizeY ),
                0,
                new Point(0,0),
                new Point(6,8),
                Vector2.Zero);

            // Create the bar
            player = new UserControlSprite(
                Game.Content.Load<Texture2D>( @"Images/bar" ),
                new Vector2( putBarAtMiddleX, putBarAtBottomY ),
                new Point( playerFrameSizeX, playerFrameSizeY ), 
                10, 
                new Point(0, 0), 
                new Point(1,1), 
                new Vector2(7,0));
         
            // load the bricks 
            util.LoadBricks(Game, ref spriteBricksList, 
                Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height,
                brickLength,brickWidth, ref numOfBricks);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // update player/bar
            player.Update(gameTime, Game.Window.ClientBounds);

            // update the ball
            ball.Update(gameTime, Game.Window.ClientBounds);

            // Initialize all the components of the game

            // start the game when the user press the space bar key
            util.startGame(ref startUpToken, ref removeToken, 
                            ref ball.position.X, player.position.X, 
                                playerFrameSizeX, ballFrameSizeX, 
                                putBallOnTopBarY, ref ball.speed, 
                                explosionImage, ref explosion, 
                                gameTime, Game.Window.ClientBounds);

            // allow user to remove the bricks
            util.userRemoveBricks(ref removeToken, numBrickAllowRemoved, 
                ref spriteBricksList, ref Score, ref numOfBricks);

            // if the brick list is empty
            if (spriteBricksList.Count == 0)
            {
                // Clear the list that contains the explosion sprite
                spriteExplosionList.Clear();
                 
                startUpToken = true;

                // Reload all the bricks to the screen
                util.LoadBricks(Game, ref spriteBricksList, 
                    Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height, 
                    brickLength, brickWidth, ref numOfBricks);

                // Reset the ball to the middle of the screen
                util.resetBallPositionMiddle(ref ball, putBallAtMiddleX, putBallOnTopBarY);

                // Reset the bar to the middle of the screen
                util.resetBarPositionMiddle(ref player, putBarAtMiddleX, putBarAtBottomY);

                // Reinitialize all of the component of the game
                // Reset removeToken == true and allow the user to remove bricks 
                util.startGame(ref startUpToken, 
                    ref removeToken, ref ball.position.X, 
                    player.position.X, playerFrameSizeX,
                    ballFrameSizeX, putBallOnTopBarY, 
                    ref ball.speed, explosionImage, 
                    ref explosion, gameTime, Game.Window.ClientBounds);
            }

            // for all the Bricks sprite in the list
            for (int i = 0; i < spriteBricksList.Count; i++)
            {
                BrickSprite s = spriteBricksList[i];
                
                // update each
                s.Update(gameTime, Game.Window.ClientBounds);

                // if the ball collide with the bricks
                if (s.collisionRect.Intersects(ball.collisionRect))
                {
                    float brickExplosionPositionX = spriteBricksList[i].position.X;
                    float brickExplosionPositionY = spriteBricksList[i].position.Y - (float)(ballFramSizeY >> 1);

                    // Play collision sound
                    if (explosion.collisionCueName != null)
                        ((Game1)Game).PlayCue(explosion.collisionCueName);

                    //Create explosions
                    util.createExplosion(ref spriteExplosionList, 
                        explosionImage, 
                        brickExplosionPositionX, 
                        brickExplosionPositionY);
                
                    // remove the brick from the game
                    spriteBricksList.RemoveAt(i);
                    --i;

                    // increment the score
                    Score++;
                    // decrement the number of bricks
                    numOfBricks--;

                    // reverse the direction of the ball
                    ball.speed.Y *= -1;
                }
            }

           // for each of the explosion sprite in the list, update/draw it
           foreach (ExplosionSprite explosionList in spriteExplosionList)
                explosionList.Update(gameTime, Game.Window.ClientBounds);

           // Determine which direction is going to go
           util.ballBounceDirection(ref ball, ref player, Game);

            // if fail to catch the ball, exit the game
           if (ball.position.Y > Game.Window.ClientBounds.Height)
           {
               // if the current score is > the best score
               if (readScore < Score)
               {
                   // open the text file that keep the score
                   FileStream stream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                   StreamWriter writer = new StreamWriter(stream);

                   // write the new score
                   writer.WriteLine(Score);
                 
                   writer.Close();
                   stream.Close();
               }
               Game.Exit();
           }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // draw the ball sprite
            ball.Draw(gameTime, spriteBatch);

            // draw the bar
            player.Draw(gameTime, spriteBatch);

            // create explosion when the ball first launch
            if (!startUpToken)
                explosion.Draw(gameTime, spriteBatch);

            // draw all the bricks
            foreach (BrickSprite s in spriteBricksList)
                s.Draw(gameTime, spriteBatch);

            // for each of the explosion sprite in the list, draw it
            foreach (ExplosionSprite s in spriteExplosionList)
                s.Draw(gameTime, spriteBatch);

            // Display score
            spriteBatch.DrawString(
                front,
                "Score: " + Score.ToString(),
                new Vector2(10, 300),
                Color.Black);

            // Display number of bricks left
            spriteBatch.DrawString(
                front,
                "Brick(s) Left: " + numOfBricks.ToString(),
                new Vector2(10, 325),
                Color.Black);

            // Display best score
            spriteBatch.DrawString(
                front,
                "Best Score: " + readScore.ToString(),
                new Vector2(10, 350),
                Color.Black);
 
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
