using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WinWizards
{
    class Fireball : Sprite
    {
        private const int MAX_DISTANCE = 500;
        public bool Visible = false;
        private Vector2 _mStartPos;
        private Vector2 _mSpeed;
        private Vector2 _mDir;
        private const string FIREBALL_NAME = @"Fireball";
        private const int START_POS_X = 125;
        private const int START_POS_Y = 245;

        public void LoadContent(ContentManager contentMgr)
        {            
            base.LoadContent(contentMgr, FIREBALL_NAME, new Vector2(START_POS_X, START_POS_Y));
            Scale = 0.8f;
        }

        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(_mStartPos, Position) > MAX_DISTANCE)
            {
                Visible = false;
            }

            if (Visible)
            {
                base.Update(gameTime, _mSpeed, _mDir);
            }
        }

        public override void  Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                base.Draw(spriteBatch);
            } 
        }

        public void Fire(Vector2 startPos, Vector2 speed, Vector2 direction)
        {
            Position = startPos;
            _mStartPos = startPos;
            _mSpeed = speed * 3;
            _mDir = direction;
            Visible = true;
        }
    }
}
