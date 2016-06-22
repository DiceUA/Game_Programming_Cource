using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeddyMineExplosion;
using System.Collections.Generic;
using System;

namespace ProgrammingAssignment5
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Window params
        const int WindowWidth = 800;
        const int WindowHeight = 600;

        // Game const
        const int SpawnTimerMIN = 1000;
        const int SpawnTimerMAX = 3000;
        const float TeddyVelocityMIN = -0.5f;
        const float TeddyVelocityMAX = 0.5f;

        // Game sprites
        Texture2D mineSprite;
        Texture2D teddyBearSprite;
        Texture2D explosionSprite;

        // List of mines and teddies
        List<Mine> mines = new List<Mine>();
        List<TeddyBear> teddies = new List<TeddyBear>();
        List<Explosion> explosions = new List<Explosion>();

        // Click processing
        bool leftClickStarted = false;
        bool leftButtonReleased = true;

        // Teddy Timer
        int spawnTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set the resolution and made the mouse visible
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load mine sprite
            mineSprite = Content.Load<Texture2D>(@"graphics\mine");
            // Load teddy sprite
            teddyBearSprite = Content.Load<Texture2D>(@"graphics\teddybear");
            // Load teddy sprite
            explosionSprite = Content.Load<Texture2D>(@"graphics\explosion.png");

            // Set spawn timer
            spawnTimer = new Random().Next(SpawnTimerMIN, SpawnTimerMIN + 1);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            // Get mouse state
            MouseState mouse = Mouse.GetState();

            // Set timer
            if(spawnTimer <= 0)
            {
                spawnTimer = new Random().Next(SpawnTimerMIN, SpawnTimerMIN + 1);
                teddies.Add(new TeddyBear(teddyBearSprite, GenerateRandomVector(), WindowWidth, WindowHeight));
            }
            spawnTimer -= gameTime.ElapsedGameTime.Milliseconds;   

            // Detect LeftClick
            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased)
            {
                leftClickStarted = true;
                leftButtonReleased = false;
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                leftButtonReleased = true;

                // if right click finished, add new pickup to list
                if (leftClickStarted)
                {
                    leftClickStarted = false;

                    // Add mine when left mouse clicked
                    mines.Add(new Mine(mineSprite, mouse.X, mouse.Y));                    
                }
            }
            // Update teddie position
            if(teddies.Count != 0)
            {
                foreach (var teddy in teddies)
                {
                    teddy.Update(gameTime);
                    foreach (var mine in mines)
                    {
                        if (teddy.CollisionRectangle.Intersects(mine.CollisionRectangle) && teddy.Active && mine.Active)
                        {
                            teddy.Active = false;
                            mine.Active = false;
                            explosions.Add(new Explosion(explosionSprite, mine.CollisionRectangle.Center.X, mine.CollisionRectangle.Center.Y));
                        }
                    }            
                }
            }
            // Update Explosion
            if(explosions.Count != 0)
            {
                foreach (var expl in explosions)
                {
                    expl.Update(gameTime);                     
                }
            }

            // Remove explosion when it stopped playing
            if (explosions.Count > 0)
            {
                for (int i = 0; i < explosions.Count; i++)
                {
                    if (explosions[i].Playing == false)
                        explosions.RemoveAt(i);
                }
            }

            // Remove inactive mine
            if (mines.Count > 0)
            {
                for (int i = 0; i < mines.Count; i++)
                {
                    if (mines[i].Active == false)
                        mines.RemoveAt(i);
                }
            }

            // Remove inactive teddy
            if (teddies.Count > 0)
            {
                for (int i = 0; i < teddies.Count; i++)
                {
                    if (teddies[i].Active == false)
                        teddies.RemoveAt(i);
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draw mine
            if(mines.Count != 0)
            {
                foreach (var item in mines)
                {
                    item.Draw(spriteBatch);
                }
            }
            // Draw teddie
            if(teddies.Count != 0)
            {
                foreach (var item in teddies)
                {
                    item.Draw(spriteBatch);
                }
            }
            // Draw explosion
            if (explosions.Count != 0)
            {
                foreach (var item in explosions)
                {
                    item.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Generates random float
        /// using TeddyVelocity constants
        /// </summary>
        /// <returns></returns>
        Vector2 GenerateRandomVector()
        {
            Random rand = new Random();
            int velocityMin = (int)(TeddyVelocityMIN * 1000);
            int velocityMax = (int)(TeddyVelocityMAX * 1000);
            Vector2 result = new Vector2((float)rand.Next(velocityMin,velocityMax)/1000, (float)rand.Next(velocityMin, velocityMax)/1000);
            return result;
        }
    }
}
