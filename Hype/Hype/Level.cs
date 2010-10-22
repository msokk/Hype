using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hype
{
    class Level : IDisposable
    {
        public bool isGameRunning = false;
        public float gameSpeed = 1f;
        private LinkedList<Platform> platforms = new LinkedList<Platform>();
        private float platformYSpacer = 0;
        private SpriteFont hudFont;

        // Level width.      
        public int levelWidth
        {
            get { return width; }
        }
        int width;

        // Level height.       
        public int levelHeight
        {
            get { return height; }
        }
        int height;

        //Player on the level
        public Player Player
        {
            get { return player; }
        }
        Player player;


        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public Level(IServiceProvider serviceProvider, int w, int h)
        {
            content = new ContentManager(serviceProvider, "Content");
            width = w;
            height = h;
            hudFont = Content.Load<SpriteFont>("Fonts/HUD");
            InitPlatforms();
            platforms.AddFirst(new Platform(this, new Vector2(150, 40), 0));
            //TODO: Player tuleb mingi platformi peale luua ära

            player = new Player(this); //AJUTINE
        }

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (isGameRunning)
            {
                foreach (Platform p in platforms)
                {
                    p.Update(gameTime);
                }
                LoadPlatforms();
                DisposePlatforms();
                Player.Update(gameTime, keyboardState, gamePadState);
                
                gameSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds*0.01f;
            }
        }

        /// <summary>
        /// Generates initial platforms
        /// </summary>
        private void InitPlatforms()
        {
            float heightToFill = (int)levelHeight;
            Random r = new Random();
            while (heightToFill > 0)
            {
                platformYSpacer = (float)r.Next(40, 70);
                heightToFill -= platformYSpacer;

                int platformCount = r.Next(1, 3);
                int sectorWidth = levelWidth / platformCount;

                for (int i = 0; i < platformCount; i++)
                {
                    int platformType = r.Next(0, 4);
                    Platform p = new Platform(this, Vector2.Zero, platformType);
                    float platFormX = (float)r.Next(i * sectorWidth, i * sectorWidth + sectorWidth - p.Size.Width);
                    Vector2 platformLocation = new Vector2(platFormX, heightToFill + MathHelper.Clamp((float)r.Next(0, (int)platformYSpacer), 0, p.Size.Height));
                    p = new Platform(this, platformLocation, platformType);
                    platforms.AddFirst(p);
                }
            }
        }

        /// <summary>
        /// Generates platforms on random locations and in random sizes
        /// </summary>
        private void LoadPlatforms()
        {
            if (platforms.First.Value.Location.Y > platformYSpacer)
            {
                Random r = new Random();
                int platformCount = r.Next(1, 3);
                int sectorWidth = levelWidth / platformCount;
                for (int i = 0; i < platformCount; i++)
                {
                    int platformType = r.Next(0, 4);
                    Platform p = new Platform(this, Vector2.Zero, platformType);
                    float platFormX = (float)r.Next(i * sectorWidth, i * sectorWidth + sectorWidth - p.Size.Width);
                    Vector2 platformLocation = new Vector2(platFormX, -50f+(float)r.Next(-15, 15));
                    p = new Platform(this, platformLocation, platformType);
                    platforms.AddFirst(p);
                }

                platformYSpacer = (float)r.Next(40, 70);
            }
        }

        /// <summary>
        /// Destroys old platforms
        /// </summary>
        private void DisposePlatforms()
        {
            if (platforms.Last.Value.Location.Y > levelHeight + 50)
            {
                platforms.RemoveLast();
            }
        }

        /// <summary>
        /// Draw everything in the level.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            foreach (Platform p in platforms)
            {
                p.Draw(gameTime, spriteBatch);
            }
            Player.Draw(gameTime, spriteBatch);
        }


        /// <summary>
        /// Unload the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        /// <summary>
        /// Restart the game
        /// </summary>
        public void Restart()
        {
            //TODO:: Kõik asukohad nulli, populeerida platformid, mängija ellu

        }
    }
}
