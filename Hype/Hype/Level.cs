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

        private List<Platform> platforms = new List<Platform>();

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

        public Level(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            //TODO: Plaformid randomiga sisse laadida, vaja on head algoritmi
            //TODO: Player tuleb mingi platformi peale luua ära

            player = new Player(); //AJUTINE
        }

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (isGameRunning)
            {
                //TODO: Liiguta tegelast ja
            }
        }
        /// <summary>
        /// Draw everything in the level.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }


        /// <summary>
        /// Unload the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        public void Restart()
        {

        }
    }
}
