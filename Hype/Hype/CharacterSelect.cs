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

            if (keyboardState.IsKeyDown(Keys.Up) || Math.Round(gamePadState.ThumbSticks.Left.Y, 1) < -0.5f ||
                Math.Round(genericPadState.ThumbSticks.Left.Y, 1) < -0.5f)
            {
                colorIndex = MathHelper.Clamp(--colorIndex, minColors, maxColors);
            }
            else if (keyboardState.IsKeyDown(Keys.Down) || Math.Round(gamePadState.ThumbSticks.Left.Y, 1) > 0.5f ||
              Math.Round(genericPadState.ThumbSticks.Left.Y, 1) > 0.5f)
            {
                colorIndex = MathHelper.Clamp(++colorIndex, minColors, maxColors);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || Math.Round(gamePadState.ThumbSticks.Left.X, 1) > 0.5f ||
              Math.Round(genericPadState.ThumbSticks.Left.X, 1) > 0.5f)
            {
                typeIndex = MathHelper.Clamp(++typeIndex, minTypes, maxTypes);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) || Math.Round(gamePadState.ThumbSticks.Left.X, 1) < -0.5f ||
              Math.Round(genericPadState.ThumbSticks.Left.X, 1) < -0.5f)
            {
                typeIndex = MathHelper.Clamp(--typeIndex, minTypes, maxTypes);
            }

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle screenArea)
        {
            //Draw the overlay, buttons and character
            Vector2 position = new Vector2(screenArea.X, screenArea.Y);
            Vector2 center = new Vector2(screenArea.X + screenArea.Width / 2.0f,
                             screenArea.Y + screenArea.Height / 2.0f);

            Vector2 overlaySize = new Vector2(overlayTexture.Width, overlayTexture.Height);
            Vector2 overlayPosition = center - overlaySize / 2;
            spriteBatch.Draw(overlayTexture, center - overlaySize / 2, Color.White);

            Vector2 overlayUpSize = new Vector2(overlayUp.Width, overlayUp.Height);
            spriteBatch.Draw(overlayUp, center - overlayUpSize / 2 - new Vector2(0, 130), Color.White);

            Vector2 overlayDownSize = new Vector2(overlayDown.Width, overlayDown.Height);
            spriteBatch.Draw(overlayDown, center - overlayDownSize / 2 + new Vector2(0, 130), Color.White);

            Vector2 overlayRightSize = new Vector2(overlayRight.Width, overlayRight.Height);
            spriteBatch.Draw(overlayRight, center - overlayRightSize / 2 + new Vector2(120, 0), Color.White);

            Vector2 overlayLeftSize = new Vector2(overlayLeft.Width, overlayLeft.Height);
            spriteBatch.Draw(overlayLeft, center - overlayLeftSize / 2 - new Vector2(120, 0), Color.White);
            spriteBatch.DrawString(uiFont, "Choose your character:", overlayPosition + new Vector2(-60, -50), Color.Red, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


            playerTexture = content.Load<Texture2D>("Player/" + getPlayerIndex());
            Vector2 playerTextureSize = new Vector2(playerTexture.Width, playerTexture.Height);
            spriteBatch.Draw(playerTexture, center - playerTextureSize / 2, Color.White);
        }
    }
}
