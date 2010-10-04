using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Hype
{
    public class Hype : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //TEST
        Texture2D dummy;
        Texture2D bg;
        Vector2 dummyPos = Vector2.Zero;
        Vector2 dummySpeed = new Vector2(100.0f, 200.0f);
        float movement = 20f;
        float elasticity = 1.5f;
        //END TEST

        public Hype()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dummy = Content.Load<Texture2D>("test/dummy");
            bg = Content.Load<Texture2D>("test/bg");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                dummySpeed.X -= movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                dummySpeed.X += movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                dummySpeed.Y -= movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                dummySpeed.Y += movement;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {

                if (graphics.IsFullScreen)
                {
                    graphics.IsFullScreen = false;
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 600;
                    graphics.ApplyChanges();
                }
                else
                {
                    graphics.IsFullScreen = true;
                    graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                    graphics.ApplyChanges();
                }
            }
            int MaxX = graphics.PreferredBackBufferWidth - dummy.Width;
            int MaxY = graphics.PreferredBackBufferHeight - dummy.Height;
            if (dummyPos.X < 0)
            {
                dummySpeed.X *= -1;
                dummySpeed.X /= elasticity;
                dummyPos.X = 0;
            }
            if (dummyPos.Y < 0)
            {
                dummySpeed.Y *= -1;
                dummySpeed.Y /= elasticity;
                dummyPos.Y = 0;
            }
            if (dummyPos.X > MaxX)
            {
                dummySpeed.X *= -1;
                dummySpeed.X /= elasticity;
                dummyPos.X = MaxX;
            }
            if (dummyPos.Y > MaxY)
            {
                dummySpeed.Y *= -1;
                dummySpeed.Y /= elasticity;
                dummyPos.Y = MaxY;
            }
            dummyPos += dummySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
            spriteBatch.Draw(dummy, dummyPos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
