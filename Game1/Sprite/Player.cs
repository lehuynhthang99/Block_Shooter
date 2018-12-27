using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStart.Sprite
{
    public class Player
    {
        int _move = 0;
        float _Timer = 0f;
        int _Score = 0;
        public bool _HasDied = false;
        float _UFOmove;
        Vector2 _prevPos;
        Vector2 _position;
        Vector2 _direction;
        Vector2 _origin;
        Vector2 _border;
        float _rotation = MathHelper.ToRadians(-90f);
        float rotattionVelocity = 2f;
        GraphicsDevice _graphicsDevice;

        List<Ball> Balls = new List<Ball>();
        List<Block> Blocks = new List<Block>();

        Ball CloneBall;

        bool _BlockUpdate = false;
        bool _CreateBlock = true;
        bool _BallUpdate = false;
        bool _DirectionUpdate = false;
        int _AddBallUpdate = 0;

        Random rnd = new Random();

        Texture2D[] _blockTexture;
        Texture2D _ballTexture;
        SpriteFont _fontContent;
        SpriteFont _scorefontContent;
        Texture2D _arrowTexture;
        Texture2D _borderTexture;
        Texture2D _UFOTexture;

        GraphicsDevice _graphicsDevice;

        public Player(Texture2D[] blockTexture, Texture2D ballTexture, SpriteFont font, Texture2D arrowTexture, Texture2D borderTexture, SpriteFont scorefont,
            Texture2D UFOTexture, GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _borderTexture = borderTexture;
            _blockTexture = blockTexture;
            _ballTexture = ballTexture;
            _fontContent = font;
            _arrowTexture = arrowTexture;
            _scorefontContent = scorefont;
            _UFOTexture = UFOTexture;
            _border = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - _borderTexture.Width) / 2f + 8, 
                (graphicsDevice.PresentationParameters.BackBufferHeight - _borderTexture.Height) / 2f + 145);
            _position = new Vector2(_border.X + 456 / 2f - ballTexture.Width / 2f, _border.Y + 545 - ballTexture.Height);
            _prevPos = _position * 1f;
            _origin = new Vector2(5, _arrowTexture.Height / 2f);
            CloneBall = new Ball(_ballTexture, _position, 456, 545, _border);

        }

        public void Update(GameTime gameTime)
        {
            if (!_HasDied)
            {
                if (_CreateBlock)
                {
                    _Timer = 0;
                    AddBall();
                    CreateBlocks();
                    _CreateBlock = false;
                    _BlockUpdate = false;
                    _BallUpdate = false;
                    _DirectionUpdate = true;
                    return;
                }

                if (_DirectionUpdate)
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        _rotation -= MathHelper.ToRadians(rotattionVelocity);
                        if (_rotation < MathHelper.ToRadians(-170f)) _rotation = MathHelper.ToRadians(-170f);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        _rotation += MathHelper.ToRadians(rotattionVelocity);
                        if (_rotation > MathHelper.ToRadians(-10f)) _rotation = MathHelper.ToRadians(-10f);
                    }

                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        _direction.X = (float)Math.Cos(_rotation);
                        _direction.Y = (float)Math.Sin(_rotation);
                        Balls[0]._direction = _direction;
                        Balls[0]._direction.Normalize();
                        CloneBall = Balls[0].Clone() as Ball;
                        _AddBallUpdate = _Score;
                        _DirectionUpdate = false;
                        _CreateBlock = false;
                        _BlockUpdate = false;
                        _BallUpdate = true;
                    }
                }

                if (_BallUpdate)
                {
                    _Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_AddBallUpdate > 0 && _Timer >= 0.1f)
                    {
                        AddBall();
                        _Timer = 0;
                        _AddBallUpdate--;
                    }

                    for (int i = 0; i < Balls.Count; i++)
                    {
                        Balls[i].Update(gameTime, Blocks);
                        if (Balls[i]._direction == Vector2.Zero)
                        {
                            _position = Balls[i]._position;
                            Balls.RemoveAt(i);
                            i--;
                        }
                    }

                    if (Balls.Count == 0)
                    {
                        _BlockUpdate = true;
                        _BallUpdate = false;
                        _CreateBlock = false;
                        _DirectionUpdate = false;
                        _Timer = 0;
                        CloneBall._position = _position;
                        CloneBall._direction = Vector2.Zero;
                        _UFOmove = _position.X - _prevPos.X;
                    }
                }

                if (_BlockUpdate)
                {
                    _Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_Timer >= 0.00001f)
                    {
                        if (_move == 0)
                        {
                            _move = (_blockTexture[0].Height + 4) / 3;
                            _UFOmove /= _move;
                        }
                        _prevPos.X += _UFOmove;
                        foreach (Block block in Blocks)
                            block.Update(gameTime);
                        _move--;
                        if (_move == 0)
                        {
                            _BlockUpdate = false;
                            _CreateBlock = true;
                            _BallUpdate = false;
                            _Timer = 0;
                            _Score++;
                            _rotation = MathHelper.ToRadians(-90f);
                            if (Blocks.Count != 0 && Blocks[0]._position.Y >= _border.Y + 545 - 80) _HasDied = true;
                        }
                    }
                    else return;

                }
            }
        }

        private void AddBall()
        {
            Ball tmpBall = CloneBall.Clone() as Ball;
            Balls.Add(tmpBall);
        }

        private void CreateBlocks()
        {
            for (int i = 0; i < 7; i++)
            {
                int a = rnd.Next(0, 2);
                if (a == 0) continue;

                a = rnd.Next(0, 5);
                Block tmp = new Block(_blockTexture[a], _fontContent, new Vector2(i * 60 + (i + 1) * 4 + _border.X, 33 + _border.Y), a, _Score + 1);
                Blocks.Add(tmp);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_borderTexture, new Vector2((_graphicsDevice.PresentationParameters.BackBufferWidth - _borderTexture.Width) / 2f, 
                (_graphicsDevice.PresentationParameters.BackBufferHeight - _borderTexture.Height) / 2f), Color.White);
            foreach (var Block in Blocks)
            {
                Block.Draw(spriteBatch);
            }
            
            string str = "SCORE  " + _Score.ToString();
            Vector2 posStr = _scorefontContent.MeasureString(str);
            posStr.X = (-posStr.X + 456)/2f + _border.X;
            posStr.Y = (142 - posStr.Y) / 2f + _border.Y - 145;
            spriteBatch.DrawString(_scorefontContent, str, posStr, Color.White);

            foreach (var Ball in Balls)
            {
                Ball.Draw(spriteBatch);
            }

            spriteBatch.Draw(_UFOTexture, new Vector2(_prevPos.X + (_ballTexture.Width - _UFOTexture.Width)/2f, _prevPos.Y + _ballTexture.Height + 1), Color.White);

            if (_DirectionUpdate)
            {
                Vector2 tmp = _position;
                tmp.X += _ballTexture.Width / 2f;
                tmp.Y += _ballTexture.Height / 2f;
                spriteBatch.Draw(_arrowTexture, tmp, null, Color.White, _rotation, _origin, 1, SpriteEffects.None, 0);
            }

            foreach (var Ball in Balls)
            {
                Ball.Draw(spriteBatch);
            }
        }
    }
}
