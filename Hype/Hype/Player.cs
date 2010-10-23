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

        public Player(Level level, Vector2 location)
        {
            this.level = level;
            this.location = location;
            texture = Level.Content.Load<Texture2D>("player");
            dieSound = Level.Content.Load<SoundEffect>("Sounds/playerDeath");
            jumpSound = Level.Content.Load<SoundEffect>("Sounds/jump");
        }
        public Player(Level level)
            : this(level, Vector2.Zero)
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
                    if (playerBounds.Bottom >= platformBounds.Top + 8f && //playerBounds.Bottom < platformBounds.Top + 12f &&
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
            jumpSound.Play();
            speed.Y = -5f * level.gameSpeed;
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
                dieSound.Play();
                dead = true;
            }
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            bool hasCollision = CheckCollision();
            if (hasCollision)
            {
                DoJump();
            }
            ApplyPhysics();
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
                }
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
