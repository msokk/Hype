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
using System.Collections;

namespace Hype
{
    public class HypeGame : Microsoft.Xna.Framework.Game
    {
        //Graphics resources
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Globals
        private SpriteFont uiFont;
        private SpriteFont bigUIFont;
        private Texture2D startOverlay;
        private Texture2D endOverlay;
        private Texture2D background;
        private Level level;
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private int startDelay = 5;

        public HypeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            uiFont = Content.Load<SpriteFont>("Fonts/UI");
            bigUIFont = Content.Load<SpriteFont>("Fonts/BigUI");

            startOverlay = Content.Load<Texture2D>("Overlays/start");
            endOverlay = Content.Load<Texture2D>("Overlays/end");
            background = Content.Load<Texture2D>("bg");

            level = new Level(Services);
        }
        /// <summary>
        /// Checks for global input and updates Level.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GlobalInput(gameTime);

            level.Update(gameTime, keyboardState, gamePadState);
            base.Update(gameTime);
        }

        private void GlobalInput(GameTime gameTime)
        {
            //Get the latest controller states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            //Exit the game with GamePad Back or with Keyboard Escape
            if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (level.Player.isDead)
            {
                bool anykeyPressed = keyboardState.GetPressedKeys().Length > 0 || gamePadState.IsButtonDown(Buttons.A);
                if (anykeyPressed)
                {
                    ResetElapsedTime();
                    level.isGameRunning = false;
                    level.Restart();
                }
            }
            else
            {
                if (gameTime.TotalGameTime.Seconds >= startDelay)
                {
                    level.isGameRunning = true;
                }
            }

        }


        /// <summary>
        /// Draw the background, level and UI.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.LightYellow);

            level.Draw(gameTime, spriteBatch);

            DrawUI(gameTime);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawUI(GameTime gameTime)
        {
            Rectangle screenArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 position = new Vector2(screenArea.X, screenArea.Y);
            //TODO: Skoor + Aeg joonistada



            //Show overlays if needed
            Vector2 center = new Vector2(screenArea.X + screenArea.Width / 2.0f,
                                         screenArea.Y + screenArea.Height / 2.0f);

            if (!level.isGameRunning && level.Player.isDead)
            {
                Vector2 overlaySize = new Vector2(endOverlay.Width, endOverlay.Height);
                Vector2 overlayPosition = center - overlaySize / 2;
                spriteBatch.Draw(endOverlay, center - overlaySize / 2, Color.White);
                spriteBatch.DrawString(uiFont, "Your score", overlayPosition + new Vector2(15, 10), Color.Red, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            if (!level.isGameRunning && !level.Player.isDead)
            {
                Vector2 overlaySize = new Vector2(startOverlay.Width, startOverlay.Height);
                Vector2 overlayPosition = center - overlaySize / 2;
                spriteBatch.Draw(startOverlay, overlayPosition, Color.White);

                String timeLeft = (startDelay - gameTime.TotalGameTime.Seconds).ToString();
                spriteBatch.DrawString(bigUIFont, timeLeft, center - new Vector2(30, 45), Color.White);
                spriteBatch.DrawString(uiFont, "Game starts in:", overlayPosition + new Vector2(15, 10), Color.Red, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(uiFont, "Get ready!", overlayPosition + new Vector2(70, 180), Color.Red, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

        }
    }
}
