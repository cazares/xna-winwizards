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
    class Wizard : Sprite
    {
        private const string WIZARD_ASSETNAME = "WizardSquare";
        private const int START_POS_X = 125;
        private const int START_POS_Y = 245;
        private const int WIZARD_SPEED = 160;
        private const int MOVE_UP = -1;
        private const int MOVE_DOWN = 1;
        private const int MOVE_LEFT = -1;
        private const int MOVE_RIGHT = 1;
        private const Keys JUMP_KEY = Keys.Space;
        readonly List<Fireball> _mFireballs = new List<Fireball>();
        private ContentManager _contentManager;

        enum State
        {
            Walking,
            Jumping,
            Ducking
        }

        State _mCurrentState = State.Walking;

        private Vector2 _mDir = Vector2.Zero;
        private Vector2 _mSpeed = Vector2.Zero;
        private Vector2 _mStartingPos = Vector2.Zero;

        private KeyboardState _mPrevKeyboardState;

        public float GetCurrHorizDirection()
        {
            return _mDir.X;
        }

        public void LoadContent(ContentManager contentMgr)
        {
            _contentManager = contentMgr;           
            Position = new Vector2(START_POS_X, START_POS_Y);
            _mFireballs.ForEach(fb => fb.LoadContent(contentMgr));
            base.LoadContent(contentMgr, WIZARD_ASSETNAME);
            Source = new Rectangle(0,0,200,Source.Height);
            SpriteCenter = new Vector2(Source.Width / 2, Source.Height / 2);
        }

        private TimeSpan _lastShot = new GameTime().TotalGameTime;       
        public void Update(GameTime gameTime)
        {
            var currKeystate = Keyboard.GetState();            
            UpdateMovement(currKeystate);
            //UpdateMouseMovement(gameTime);
            UpdateJump(currKeystate);
            UpdateDuck(currKeystate);
            UpdateFireball(gameTime, currKeystate);
            _mPrevKeyboardState = currKeystate;
            
            base.Update(gameTime, _mSpeed, _mDir);
        }

        private const int RotationMultiplier = 25;
        private MouseState _lastMouseState;
        private void UpdateMouseMovement(GameTime gameTime)
        {
            var currMouseState = Mouse.GetState();
            Position.X = currMouseState.X;
            Position.Y = currMouseState.Y;
            if (currMouseState.LeftButton == ButtonState.Pressed 
                && _lastMouseState.LeftButton == ButtonState.Released)
            {
                _spEffects ^= SpriteEffects.FlipHorizontally;
            }

            if (currMouseState.RightButton == ButtonState.Pressed
                && _lastMouseState.RightButton == ButtonState.Released)
            {
                _spEffects ^= SpriteEffects.FlipVertically;
            }

            if (currMouseState.ScrollWheelValue > _lastMouseState.ScrollWheelValue)
            {
                Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * RotationMultiplier;
            }
            else if (currMouseState.ScrollWheelValue < _lastMouseState.ScrollWheelValue)
            {
                Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * RotationMultiplier;
            }
            _lastMouseState = currMouseState;
        }

        const Keys FIREBALL_KEY = Keys.F;
        private void UpdateFireball(GameTime gameTime, KeyboardState currKeystate)
        {
            _mFireballs.ForEach(fb => fb.Update(gameTime));
            if (!ShouldShoot(currKeystate, gameTime)) return;
            _lastShot = gameTime.TotalGameTime;
            ShootFireball();
        }

        private bool ShouldShoot(KeyboardState currKey, GameTime gameTime)
        {
            return currKey.IsKeyDown(FIREBALL_KEY) &&
                   gameTime.TotalGameTime.Subtract(_lastShot) > TimeSpan.FromMilliseconds(150);
        }

        private void ShootFireball()
        {
            if (_mCurrentState != State.Walking) return;
            var createNew = true;
            foreach (var fb in _mFireballs.Where(fb => !fb.Visible))
            {
                createNew = false;
                fb.Fire(Position + new Vector2(Size.Width / 4, 0),
                        new Vector2(200,200), new Vector2(1,0));
                break;
            }

            if (!createNew) return;
            var newFb = new Fireball();
            newFb.LoadContent(_contentManager);
            newFb.Fire(Position + new Vector2(Size.Width / 4, 0), 
                    new Vector2(200,200), new Vector2(1,0));
            _mFireballs.Add(newFb);
        }

        private const Keys DUCK_KEY = Keys.LeftShift;
        private void UpdateDuck(KeyboardState currKeystate)
        {
            if (currKeystate.IsKeyDown(DUCK_KEY))
            {
                Duck();
            }
            else
            {
                StopDucking();
            }
        }

        private void StopDucking()
        {
            switch (_mCurrentState)
            {
                case State.Ducking:
                    Source = new Rectangle(0,0, 200, Source.Height);
                    _mCurrentState = State.Walking;
                    break;
                case State.Jumping:
                case State.Walking:
                    Source = new Rectangle(0, 0, 200, Source.Height);
                    break;
            }
        }

        private void Duck()
        {
            if (_mCurrentState == State.Jumping || _mCurrentState == State.Walking)
            {
                Source = new Rectangle(200,0,200, Source.Height);
                return;
            }
            if (_mCurrentState != State.Walking) return;
            _mSpeed = Vector2.Zero;
            _mDir = Vector2.Zero;
            Source = new Rectangle(200, 0, 200, Source.Height);
            _mCurrentState = State.Ducking;
        }

        private const int MAX_JUMP_HEIGHT = 150;
        private void UpdateJump(KeyboardState currKeystate)
        {
            if (_mCurrentState == State.Walking)
            {
                if (currKeystate.IsKeyDown(JUMP_KEY) && !_mPrevKeyboardState.IsKeyDown(JUMP_KEY))
                {
                   Jump();
                }
            }

            if (_mCurrentState != State.Jumping) return;

            if (currKeystate.IsKeyDown(Keys.Left))
            {
                _mDir.X = MOVE_LEFT;
            }
            if (currKeystate.IsKeyDown(Keys.Right))
            {
                _mDir.X = MOVE_RIGHT;
            }
            if (_mStartingPos.Y - Position.Y > MAX_JUMP_HEIGHT)
            {
                _mDir.Y = MOVE_DOWN;
            }
            if (Position.Y <= _mStartingPos.Y) return;
            Position.Y = _mStartingPos.Y;
            _mCurrentState = State.Walking;
            _mDir = Vector2.Zero;
        }

        private void Jump()
        {
            if (_mCurrentState != State.Jumping)
            {
                _mCurrentState = State.Jumping;
                _mStartingPos = Position;
                _mDir.Y = MOVE_UP;
                _mSpeed = new Vector2(WIZARD_SPEED, WIZARD_SPEED);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _mFireballs.ForEach(fb => fb.Draw(spriteBatch));
            base.Draw(spriteBatch);
        }

        private void UpdateMovement(KeyboardState currKeystate)
        {
            if (_mCurrentState != State.Walking) return;
            _mSpeed = Vector2.Zero;
            _mDir = Vector2.Zero;

            if (currKeystate.IsKeyDown(Keys.Left))
            {
                _mSpeed.X = WIZARD_SPEED;
                _mDir.X = MOVE_LEFT;
            }
            else if (currKeystate.IsKeyDown(Keys.Right))
            {
                _mSpeed.X = WIZARD_SPEED;
                _mDir.X = MOVE_RIGHT;
            }

            if (currKeystate.IsKeyDown(Keys.Up))
            {
                _mSpeed.Y = WIZARD_SPEED;
                _mDir.Y = MOVE_UP;
            }
            else if (currKeystate.IsKeyDown(Keys.Down))
            {
                _mSpeed.Y = WIZARD_SPEED;
                _mDir.Y = MOVE_DOWN;
            }
        }
    }
}
