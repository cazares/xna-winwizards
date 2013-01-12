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

namespace WinWizards
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;
        private Wizard _mWizardSprite;
        private const string SQUARE_SPRITE_NAME = @"SquareGuy";       
        private HealthBar _healthBar1;
        private HealthBar _healthBar2;

        private HorizontallyScrollingBackground _mScrollingBackground;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private const float BACKGROUND_SCALE = 1.6f;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _mWizardSprite = new Wizard();
            _healthBar1 = new HealthBar()
                              {
                                  MaxWidth = Window.ClientBounds.Width / 2,
                                  StartPosX = 10,
                              };
            _healthBar2 = new HealthBar
                              {
                                  MaxWidth = Window.ClientBounds.Width/2,
                                  SpriteFx = SpriteEffects.FlipHorizontally
                              };
            
            base.Initialize();
        }

        private const string _1Str = @"Background01";
        private const string _2Str = @"Background02";
        private const string _3Str = @"Background03";
        private const string _4Str = @"Background04";
        private const string _5Str = @"Background05";

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _mWizardSprite.LoadContent(Content);

            _mScrollingBackground = new HorizontallyScrollingBackground(GraphicsDevice.Viewport);
            _mScrollingBackground.AddBackground(_1Str);
            _mScrollingBackground.AddBackground(_2Str);
            _mScrollingBackground.AddBackground(_3Str);
            _mScrollingBackground.AddBackground(_4Str);
            _mScrollingBackground.AddBackground(_5Str);

            _mScrollingBackground.LoadContent(Content);

            _healthBar1.LoadContent(Content);
            _healthBar2.LoadContent(Content);
            _healthBar2.StartPosX = Window.ClientBounds.Width - _healthBar2.Source.Width / 2 - 10;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            _mWizardSprite.Update(gameTime);
            _mScrollingBackground.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);
            _healthBar1.Update(gameTime);
            _healthBar2.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _mScrollingBackground.Draw(_spriteBatch);

            _healthBar1.Draw(_spriteBatch);
            _healthBar2.Draw(_spriteBatch);

            _mWizardSprite.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
