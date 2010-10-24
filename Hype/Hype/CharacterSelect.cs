using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Hype
{
    class CharacterSelect
    {
        private Texture2D playerTexture;
        private Texture2D overlayTexture;
        private Texture2D overlayUp;
        private Texture2D overlayDown;
        private Texture2D overlayRight;
        private Texture2D overlayLeft;
        private SpriteFont uiFont;
        private ContentManager content;

        public bool Selected
        {
            get { return characterselected; }
        }
        private bool characterselected = false;

        private float typeIndex = 1f;
        private float colorIndex = 1f;
        private const float maxColors = 4f;
        private const float minColors = 1f;
        private const float maxTypes = 7f;
        private const float minTypes = 1f;
        private const float t = 0.5f;

        //Keypress simulation
        private KeyboardState oldKeyboardState;
        private GamePadState oldGamePadState;
        private GamePadState oldGenericPadState;

        public CharacterSelect(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            overlayTexture = content.Load<Texture2D>("Overlays/end");
            overlayUp = content.Load<Texture2D>("CharSelect/up");
            overlayDown = content.Load<Texture2D>("CharSelect/down");
            overlayRight = content.Load<Texture2D>("CharSelect/right");
            overlayLeft = content.Load<Texture2D>("CharSelect/left");
            uiFont = content.Load<SpriteFont>("Fonts/UI");
            playerTexture = content.Load<Texture2D>("Player/" + getPlayerIndex());
        }

        public String getPlayerIndex()
        {
            return typeIndex + "." + colorIndex;
        }


        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, GamePadState genericPadState, Level level)
        {

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.IsButtonDown(Buttons.Start)
                || genericPadState.IsButtonDown(Buttons.A))
            {
                characterselected = true;
                level.Start(getPlayerIndex());
            }

            //Normalized Thumbstick controls
            Vector2 gamePadAxis = new Vector2((float)Math.Round(gamePadState.ThumbSticks.Left.X, 1),
                (float)Math.Round(gamePadState.ThumbSticks.Left.Y, 1));
            Vector2 genericPadAxis = new Vector2((float)Math.Round(genericPadState.ThumbSticks.Left.X, 1),
                (float)Math.Round(genericPadState.ThumbSticks.Left.Y, 1));
            Vector2 oldGamePadAxis = new Vector2((float)Math.Round(oldGamePadState.ThumbSticks.Left.X, 1),
                (float)Math.Round(oldGamePadState.ThumbSticks.Left.Y, 1));
            Vector2 oldGenericPadAxis = new Vector2((float)Math.Round(oldGenericPadState.ThumbSticks.Left.X, 1),
                (float)Math.Round(oldGenericPadState.ThumbSticks.Left.Y, 1));
            Console.WriteLine(gamePadAxis.X);

            //Keypresses
            //Up
            if ((genericPadAxis.Y <= -t && oldGenericPadAxis.Y > -t)
                || (gamePadAxis.Y <= -t && oldGamePadAxis.Y > -t) 
                || (keyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up)))
            {
                colorIndex = MathHelper.Clamp(--colorIndex, minColors, maxColors);
            }

            //Down
            if ((genericPadAxis.Y >= t && oldGenericPadAxis.Y < t)
                || (gamePadAxis.Y >= t && oldGamePadAxis.Y < t)
                || (keyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down)))
            {
                colorIndex = MathHelper.Clamp(++colorIndex, minColors, maxColors);
            }

            //Right
            if ((genericPadAxis.X >= t && oldGenericPadAxis.X < t)
                || (gamePadAxis.X >= t && oldGamePadAxis.X < t)
                || (keyboardState.IsKeyDown(Keys.Right) && !oldKeyboardState.IsKeyDown(Keys.Right)))
            {
                typeIndex = MathHelper.Clamp(++typeIndex, minTypes, maxTypes);
            }

            //Left
            if ((genericPadAxis.X <= -t && oldGenericPadAxis.X > -t)
                || (gamePadAxis.X <= -t && oldGamePadAxis.X > -t)
                || (keyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left)))
            {
                typeIndex = MathHelper.Clamp(--typeIndex, minTypes, maxTypes);
            }

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
            oldGenericPadState = genericPadState;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle screenArea)
        {
            //Draw the overlay, buttons and character
            Vector2 position = new Vector2(screenArea.X, screenArea.Y);
            Vector2 center = new Vector2(screenArea.X + screenArea.Width / 2.0f,
                             screenArea.Y + screenArea.Height / 2.0f);

            //Overlay
            Vector2 overlaySize = new Vector2(overlayTexture.Width, overlayTexture.Height);
            Vector2 overlayPosition = center - overlaySize / 2;
            spriteBatch.Draw(overlayTexture, center - overlaySize / 2, Color.White);

            //Arrows
            Vector2 overlayUpSize = new Vector2(overlayUp.Width, overlayUp.Height);
            spriteBatch.Draw(overlayUp, center - overlayUpSize / 2 - new Vector2(0, 130), Color.White);

            Vector2 overlayDownSize = new Vector2(overlayDown.Width, overlayDown.Height);
            spriteBatch.Draw(overlayDown, center - overlayDownSize / 2 + new Vector2(0, 130), Color.White);

            Vector2 overlayRightSize = new Vector2(overlayRight.Width, overlayRight.Height);
            spriteBatch.Draw(overlayRight, center - overlayRightSize / 2 + new Vector2(120, 0), Color.White);

            Vector2 overlayLeftSize = new Vector2(overlayLeft.Width, overlayLeft.Height);
            spriteBatch.Draw(overlayLeft, center - overlayLeftSize / 2 - new Vector2(120, 0), Color.White);
            spriteBatch.DrawString(uiFont, "Choose your character", overlayPosition + new Vector2(-60, -50), Color.Black, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            //Help
            float helpScale = 0.6f;
            Vector2 typeSize = uiFont.MeasureString("type") * helpScale;
            Vector2 colorSize = uiFont.MeasureString("color") * helpScale;
            spriteBatch.DrawString(uiFont, "type", center - typeSize / 2 - new Vector2(50, 15), Color.Black, 1.57f, Vector2.Zero, helpScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(uiFont, "color", center - colorSize / 2 - new Vector2(0, 100), Color.Black, 0f, Vector2.Zero, helpScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(uiFont, "type", center - typeSize / 2 + new Vector2(120, -15), Color.Black, 1.57f, Vector2.Zero, helpScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(uiFont, "color", center - colorSize / 2 + new Vector2(0, 100), Color.Black, 0f, Vector2.Zero, helpScale, SpriteEffects.None, 0f);

            //Player image
            playerTexture = content.Load<Texture2D>("Player/" + getPlayerIndex());
            Vector2 playerLocation = new Vector2(center.X - (float)Math.Floor((double)playerTexture.Width / 2)
                , center.Y - (float)Math.Floor((double)playerTexture.Height / 2));
            spriteBatch.Draw(playerTexture, playerLocation, Color.White);
        }
    }
}
