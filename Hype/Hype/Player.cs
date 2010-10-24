using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Hype
{
    class Player
    {
        private Texture2D texture;
        private Vector2 location;
        private Vector2 speed = Vector2.Zero;
        private SpriteEffects flip = SpriteEffects.None;

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;
        private const float MaxXSpeed = 3f;

        //Sound effect test
        private SoundEffect dieSound;
        private SoundEffect jumpSound;

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

        public Player(Level level, Vector2 location, String playerIndex)
        {
            this.level = level;
            this.location = location;
            texture = Level.Content.Load<Texture2D>("Player/" + playerIndex);
            dieSound = Level.Content.Load<SoundEffect>("Sounds/playerDeath");
            jumpSound = Level.Content.Load<SoundEffect>("Sounds/jump");
        }
        public Player(Level level, String playerIndex)
            : this(level, new Vector2(level.levelWidth - 160, level.levelHeight - 143), playerIndex)
        {
        }
        public Player(Level level)
            : this(level, new Vector2(level.levelWidth - 160, level.levelHeight - 143), "1.1")
        {
        }

        private void ApplyPhysics()
        {
            //Apply here gravity and air drag for X axis
            speed.Y += 0.1f;
            speed.X = MathHelper.Clamp(speed.X, MaxXSpeed * -1, MaxXSpeed);
            if (speed.X > 0.1f)
            {
                speed.X -= 0.1f;
             }
            if (speed.X < -0.1f)
            {
                speed.X += 0.1f;
            }
        }

        private bool CheckCollision()
        {
            if (speed.Y > 0 && location.Y + texture.Height > 0)
            {
                Rectangle playerBounds = new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
                foreach (Platform p in Level.Platforms)
                {
                    Rectangle platformBounds = new Rectangle((int)p.Location.X, (int)p.Location.Y, p.Size.Width, p.Size.Height);
                    if (playerBounds.Bottom >= platformBounds.Top + 8f && playerBounds.Bottom < platformBounds.Top + 15f &&
                        playerBounds.Right - playerBounds.Width / 2 > platformBounds.Left && playerBounds.Left + playerBounds.Width / 2 < platformBounds.Right)
                    {
                        //speed.Y = 0f;
                        return true;
                    }
                }
            }
            return false;
        }

        private void DoJump()
        {
            jumpSound.Play(0.5f, 0f, 0f);
            speed.Y = -5f * level.gameSpeed;
        }

        /// <summary>
        /// Player can cross sides
        /// </summary>
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

        /// <summary>
        /// Player is dead when he is over the bottom more than 50px
        /// </summary>
        private void CheckDeath()
        {
            if (this.location.Y > Level.levelHeight + 50)
            {
                dieSound.Play(0.5f, 0f, 0f);
                dead = true;
            }
        }

        public void ResetPosition()
        {
            this.location = new Vector2(Level.levelWidth - 160, Level.levelHeight - 143);
            this.dead = false;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, GamePadState genericPadState)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool hasCollision = CheckCollision();
            if (hasCollision)
            {
                DoJump();
            }
            ApplyPhysics();

            if (keyboardState.IsKeyDown(Keys.Right) || Math.Round(gamePadState.ThumbSticks.Left.X, 1) > 0 ||
                Math.Round(genericPadState.ThumbSticks.Left.X, 1) > 0)
            {
                speed.X += 1f + 1 * elapsed;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || Math.Round(gamePadState.ThumbSticks.Left.X, 1) < 0 ||
                Math.Round(genericPadState.ThumbSticks.Left.X, 1) < 0)
            {
                speed.X -= 1f + 1 * elapsed;
            }


            location.Y += Level.gameSpeed;
            location += speed;
            SideReEntry();
            CheckDeath();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (speed.X > 0)
            {
                flip = SpriteEffects.None;
            }
            else if (speed.X < 0)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(texture, location, null, Color.White, 0f, Vector2.Zero, 1f, flip, 0f);
        }
    }
}
