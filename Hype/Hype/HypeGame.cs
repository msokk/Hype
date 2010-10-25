using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nuclex.Input;

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

        //Splash screen
        private Texture2D splash;
        private float splashFade = 255.0f;
        private float splashFadeSpeed = 200.0f;

        private Level level;
        private CharacterSelect charselect;

        //Custom input
        private InputManager inputManager;
        private GamePadState gamePadState;
        private GamePadState genericPadState;
        private KeyboardState keyboardState;

        //Game start delay
        private double startDelay = 5;

        public HypeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            inputManager = new InputManager();
            Components.Add(inputManager);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// Load overlays, fonts, character selection and level
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            uiFont = Content.Load<SpriteFont>("Fonts/UI");
            bigUIFont = Content.Load<SpriteFont>("Fonts/BigUI");

            startOverlay = Content.Load<Texture2D>("Overlays/start");
            endOverlay = Content.Load<Texture2D>("Overlays/end");
            splash = Content.Load<Texture2D>("Overlays/splash");
            background = Content.Load<Texture2D>("bg");


            charselect = new CharacterSelect(Services);
            level = new Level(Services, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Checks for global input, hides splash screen, updates character selection and updates Level.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Get new state from Nuclex
            inputManager.Update();

            //Fade out splash screen
            if (splashFade > 0f)
            {
                splashFade -= (float)gameTime.ElapsedGameTime.TotalSeconds * splashFadeSpeed;
                splashFade = (float)Math.Floor(splashFade);
            }

            GlobalInput(gameTime);

            //Update character selection and then level
            if (!charselect.Selected)
            {
                charselect.Update(gameTime, keyboardState, gamePadState, genericPadState, level);
            }
            else
            {
                level.Update(gameTime, keyboardState, gamePadState, genericPadState);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Game exits and restarts here
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void GlobalInput(GameTime gameTime)
        {
            //Get the latest controller states
            keyboardState = Keyboard.GetState();
            gamePadState = inputManager.GetGamePad(PlayerIndex.One).GetState();
            genericPadState = inputManager.GetGamePad(ExtendedPlayerIndex.Five).GetState();

            //Exit the game with GamePad Back or with Keyboard Escape
            if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape) || genericPadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Disable input until character is selected
            if (charselect.Selected && level.Player != null)
            {

                //Restart the level - should be removed?
                if (gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Back) || genericPadState.Buttons.BigButton == ButtonState.Pressed)
                    level.Restart();

                //Player is dead - stop the game and wait for restart
                if (level.Player.isDead)
                {
                    level.isGameRunning = false;
                    bool anykeyPressed = keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || genericPadState.IsButtonDown(Buttons.A);
                    if (anykeyPressed)
                    {
                        ResetElapsedTime();
                        level.Restart();
                    }
                }
                //Player isn't dead, game starts if initial 5 seconds is over
                else
                {
                    if (startDelay + 1 <= 2)
                    {
                        level.isGameRunning = true;
                    }
                    else
                    {
                        startDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(background, Vector2.Zero, Color.LightYellow);

            //Draw splash initially
            if (splashFade > 0f)
            {
                DrawSplash(gameTime);
            }
            else
            {
                //Draw selection screen if not selected
                if (!charselect.Selected)
                {
                    charselect.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.TitleSafeArea);
                }
                else
                {
                    //Start drawing level and UI
                    level.Draw(gameTime, spriteBatch);

                    DrawUI(gameTime);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw initial splash screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void DrawSplash(GameTime gameTime)
        {
            Rectangle screenArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 position = new Vector2(screenArea.X, screenArea.Y);
            Vector2 center = new Vector2(screenArea.X + screenArea.Width / 2.0f,
                                         screenArea.Y + screenArea.Height / 2.0f);
            Vector2 splashSize = new Vector2(splash.Width, splash.Height);
            Vector2 splashPosition = center - splashSize / 2;

            spriteBatch.Draw(splash, splashPosition, Color.White);
        }

        /// <summary>
        /// Draw overlay dialogs
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void DrawUI(GameTime gameTime)
        {
            Rectangle screenArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 position = new Vector2(screenArea.X, screenArea.Y);

            //Show overlays if needed
            Vector2 center = new Vector2(screenArea.X + screenArea.Width / 2.0f,
                                         screenArea.Y + screenArea.Height / 2.0f);

            //Show game over dialog
            if (!level.isGameRunning && level.Player.isDead)
            {
                Vector2 overlaySize = new Vector2(endOverlay.Width, endOverlay.Height);
                Vector2 overlayPosition = center - overlaySize / 2;
                spriteBatch.Draw(endOverlay, overlayPosition, Color.White);

                float scoreScale = (level.gameScore < 100000) ? 1f : 0.7f;
                Vector2 scoreSize = bigUIFont.MeasureString(level.gameScore.ToString()) * scoreScale;

                spriteBatch.DrawString(uiFont, "Your score is", center - uiFont.MeasureString("Your score is") / 2 - new Vector2(0, 180), Color.Black, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigUIFont, level.gameScore.ToString(), center - scoreSize / 2, Color.White, 0f, Vector2.Zero, scoreScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(uiFont, "Press Space(A)! ", overlayPosition + new Vector2(15, 340), Color.Black, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            //Show game starting dialog
            if (!level.isGameRunning && !level.Player.isDead)
            {
                Vector2 overlaySize = new Vector2(startOverlay.Width, startOverlay.Height);
                Vector2 overlayPosition = center - overlaySize / 2;
                spriteBatch.Draw(startOverlay, overlayPosition, Color.White);

                String timeLeft = Math.Floor(startDelay).ToString();
                spriteBatch.DrawString(bigUIFont, timeLeft, center - bigUIFont.MeasureString(timeLeft) / 2, Color.White);
                spriteBatch.DrawString(uiFont, "Game starts in:", overlayPosition + new Vector2(15, 10), Color.Black, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(uiFont, "Get ready!", overlayPosition + new Vector2(70, 180), Color.Black, 0.06f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

        }
    }
}
