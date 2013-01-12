using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinWizards
{
    class HealthBar : Sprite
    {
        private int _mCurrentHealth = 100;
        private const string HEALTH_NAME = @"HealthBar";
        public int MaxWidth;
        public int StartPosX;
        private const Keys DOWN_HEALTH_KEY = Keys.D;
        private const Keys UP_HEALTH_KEY = Keys.U;
        private bool _isFlipped;

        public void LoadContent(ContentManager contentMgr)
        {
            if (SpriteFx == SpriteEffects.FlipHorizontally)
            {
                _isFlipped = true;
            }
            
            base.LoadContent(contentMgr, HEALTH_NAME);
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            var healthFraction = (double) _mCurrentHealth/100;
            var healthOffset = (int) (Source.Width/2*healthFraction);
            var damageOffset = (int) (Source.Width/2*(1 - healthFraction));
            var healthStart = _isFlipped ? StartPosX + damageOffset : StartPosX;
            var damageStart = _isFlipped ? StartPosX : StartPosX + healthOffset;
            spriteBatch.Draw(_mSpriteTexture, new Rectangle(damageStart,
                 30, damageOffset, 44), new Rectangle(0, 45, Source.Width, 44), Color.Gray, 
                 Rotation, SpriteCenter, SpriteFx, 0);


            //Draw the current health level based on the current Health
            spriteBatch.Draw(_mSpriteTexture, new Rectangle(healthStart,
                 30, healthOffset, 44),
                 new Rectangle(0, 45, Source.Width, 44), Color.Red, 
                 Rotation, SpriteCenter, SpriteEffects.FlipHorizontally, 0);

            //Draw the box around the health bar
            spriteBatch.Draw(_mSpriteTexture, new Rectangle(StartPosX,
                30, Source.Width / 2, 44), new Rectangle(0, 0, Source.Width, 44), Color.White, 
                Rotation, SpriteCenter, SpriteFx, 0);
        }

        private TimeSpan _lastDraw = new GameTime().TotalGameTime;
        public void Update(GameTime gameTime)
        {
            //Get the current keyboard state (which keys are currently pressed and released)
            var mKeys = Keyboard.GetState();

            //If pressed, increase the Health bar
            if (mKeys.IsKeyDown(UP_HEALTH_KEY))
            {
                _mCurrentHealth += 1;
            }

            //If pressed, decrease the Health bar
            if (mKeys.IsKeyDown(DOWN_HEALTH_KEY))
            {
                _mCurrentHealth -= 1;
            }

            //Force the health to remain between 0 and 100
            _mCurrentHealth = (int)MathHelper.Clamp(_mCurrentHealth, 0, 100);
        }
    }
}
