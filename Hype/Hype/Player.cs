using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hype
{
    class Player
    {
        private Texture2D texture;
        public bool isDead
        {
            get { return dead; }
        }
        bool dead = false;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Player(Level level)
        {
            this.level = level;
            texture = Level.Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        { 
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
        }
    }
}
