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
        private Vector2 location = Vector2.Zero;
        private Vector2 speed = Vector2.Zero;
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

        private void ApplyPhysics()
        {

        }

        private void CheckCollision()
        {

        }

        private void SideReEntry()
        {
            if (this.location.X > Level.levelWidth)
            {
                this.location.X = 0 - this.texture.Width;
            }
            if (this.location.X + this.texture.Width < 0)
            {
                this.location.X = Level.levelWidth;
            }
        }

        private void CheckDeath()
        {
            if (this.location.Y > Level.levelHeight + 50)
            {
                dead = true;
            }
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            ApplyPhysics();
            CheckCollision();

            //TODO: Move the player

            Keys[] k = keyboardState.GetPressedKeys();
            for (int i = 0; i < k.Length; i++)
            {
                switch (k[i])
                {
                    case Keys.Right:
                        speed.X += 1f;
                        break;
                    case Keys.Left:
                        speed.X -= 1f;
                        break;
                    case Keys.Up:
                        speed.Y -= 1f;
                        break;
                    case Keys.Down:
                        speed.Y += 1f;
                        break;

                }
            }
            location.Y += Level.gameSpeed;
            location += speed;
            SideReEntry();
            CheckDeath();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }
    }
}
