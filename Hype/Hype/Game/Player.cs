using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hype.Game
{
    class Player : Entity
    {
        float movement = 20f;
        float elasticity = 1.5f;
        public Player()
        {
            this.ResourceName = "test/dummy";
            this.Speed = new Vector2(100.0f, 200.0f);
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Speed.X -= movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Speed.X += movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.Speed.Y -= movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.Speed.Y += movement;
            }
            
            int MaxX = graphics.PreferredBackBufferWidth - this.texture.Width;
            int MaxY = graphics.PreferredBackBufferHeight - this.texture.Height;
            if (this.Position.X < 0)
            {
                this.Speed.X *= -1;
                this.Speed.X /= elasticity;
                this.Position.X = 0;
            }
            if (this.Position.Y < 0)
            {
                this.Speed.Y *= -1;
                this.Speed.Y /= elasticity;
                this.Position.Y = 0;
            }
            if (this.Position.X > MaxX)
            {
                this.Speed.X *= -1;
                this.Speed.X /= elasticity;
                this.Position.X = MaxX;
            }
            if (this.Position.Y > MaxY)
            {
                this.Speed.Y *= -1;
                this.Speed.Y /= elasticity;
                this.Position.Y = MaxY;
            }
            this.Position += this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
