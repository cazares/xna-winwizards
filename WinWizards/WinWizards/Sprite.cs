using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WinWizards
{
    class Sprite
    {       
        public Vector2 SpriteCenter;
        protected float Rotation;

        protected SpriteEffects _spEffects = SpriteEffects.None;
        public SpriteEffects SpriteFx
        {
            get { return _spEffects; }
            set { _spEffects ^= value; }
        }

        private Rectangle _mSource;
        public Rectangle Source
        {
            get { return _mSource; }
            set 
            { 
                _mSource = value;
                Size = new Rectangle(0,0, (int)(_mSource.Width * Scale), (int)(_mSource.Height * Scale));
            }
        }

        public Rectangle Size;
        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        private float _mScale = 1.0f;
        public float Scale
        {
            get { return _mScale; }
            set
            {
                _mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }
        public Vector2 Position = new Vector2(0,0);
        protected Texture2D _mSpriteTexture;
        public string AssetName;

        public void LoadContent(ContentManager contentMgr, string asset)
        {
            _mSpriteTexture = contentMgr.Load<Texture2D>(asset);
            AssetName = asset;
            Source = new Rectangle(0,0, _mSpriteTexture.Width, _mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(_mSpriteTexture.Width * Scale), (int)(_mSpriteTexture.Height * Scale));
        }

        public void LoadContent(ContentManager contentMgr, string asset, Vector2 position)
        {
            _mSpriteTexture = contentMgr.Load<Texture2D>(asset);
            LoadContent(contentMgr, asset);
            Position = position;
            Size = new Rectangle(0,0,(int)(_mSpriteTexture.Width * Scale), (int)(_mSpriteTexture.Height * Scale));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mSpriteTexture, Position, Source,
                             Color.White, Rotation, SpriteCenter, Scale, _spEffects, 0);
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
