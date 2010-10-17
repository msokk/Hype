using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hype
{
    class Platform
    {
        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Platform(Level level)
        {
            this.level = level;
        }

        public void Update(GameTime gameTime)
        {

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
