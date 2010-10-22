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
        private Texture2D texture;

        public Vector2 Location
        {
            get { return location; }
        }
        Vector2 location = Vector2.Zero;

        public Rectangle Size
        {
            get { return size; }
        }
        Rectangle size;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Platform(Level level, Vector2 loc, int type)
        {
            this.level = level;
            this.location = loc;
            this.texture = Level.Content.Load<Texture2D>("Platforms/plate"+type);
            this.size = texture.Bounds;
        }

        public void Update(GameTime gameTime)
        {
            location.Y += Level.gameSpeed;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }
    }
}
