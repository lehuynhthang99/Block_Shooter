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
    class Ball: Sprite
    {
        public Vector2 _direction;
        public int widthWindows;
        public int heightWindows;

        public Vector2 _border;

        protected float _speed = 15f;

        public Ball(Texture2D texture, Vector2 position, int width, int height, Vector2 border)
            : base(texture, position)
        {
            this.widthWindows = width;
            this.heightWindows = height;
            _border = border;
        }

        public void Update(GameTime gameTime, List<Block> Blocks)
        {
            BallMove();

            for (int i = 0; i < Blocks.Count; i++)
            {
                float rW = (float)(Blocks[i]._texture.Width) / 2;
                float rH = (float)(Blocks[i]._texture.Height) / 2;

                Vector2 tmp = new Vector2((float)_texture.Width / 2, (float)_texture.Height / 2);
                Vector2 circleCenter = _position + tmp;
                float radius = _texture.Height / 2.0f;

                float DisX = Math.Abs(circleCenter.X - (Blocks[i]._position.X + rW));
                float DisY = Math.Abs(circleCenter.Y - (Blocks[i]._position.Y + rH));

                if (DisX > radius + rW || DisY > radius + rH) continue;

                Vector2 Inverse = _direction * -1;
                Vector2 Reflect = new Vector2(0, 0);
                float n = 0;

                switch (Blocks[i]._type)
                {
                    case 0:

                        if (DisY < rH)
                        {
                            if (circleCenter.X < Blocks[i]._position.X + rW) n = (Blocks[i]._position.X - radius - circleCenter.X) * 1.0f / Inverse.X;
                            else n = (Blocks[i]._position.X + 2 * rW + radius - circleCenter.X) * 1.0f / Inverse.X;
                            Reflect.X = 1;
                            Reflect.Y = 0;
                        }
                        else if (DisX < rW)
                        {
                            if (circleCenter.Y < Blocks[i]._position.Y + rH) n = (Blocks[i]._position.Y - radius - circleCenter.Y) * 1.0f / Inverse.Y;
                            else n = (Blocks[i]._position.Y + 2 * rH + radius - circleCenter.Y) * 1.0f / Inverse.Y;
                            Reflect.X = 0;
                            Reflect.Y = 1;
                        }
                        else if (Math.Pow(DisX - rW, 2) + Math.Pow(DisY - rH, 2) <= radius * radius)
                        {
                            float x = Blocks[i]._position.X + rW;
                            float y = Blocks[i]._position.Y + rH;
                            float a, b;
                            a = b = 0;
                            if (circleCenter.X < x)
                            {
                                a = x - rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }
                            else
                            {
                                a = x + rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }

                            float tmp1 = Inverse.X * Inverse.X + Inverse.Y + Inverse.Y;
                            float tmp2 = 2 * (Inverse.X * (circleCenter.X - a) + Inverse.Y * (circleCenter.Y - b));
                            float delta = (float)(tmp2 * tmp2 - tmp1 * (Math.Pow(circleCenter.X - a, 2) + Math.Pow(circleCenter.Y - b, 2) - radius * radius));
                            if (delta < 0) continue;
                            n = (float)(-tmp2 + Math.Sqrt(delta)) / tmp1;

                            Vector2 tmpPosition = circleCenter + n * Inverse;
                            Reflect.X = tmpPosition.X - a;
                            Reflect.Y = tmpPosition.Y - b;

                        }
                        if (Reflect != Vector2.Zero)
                        {
                            Reflect.Normalize();
                            _position += Inverse * n;
                            _direction = Vector2.Reflect(_direction, Reflect);
                            Blocks[i]._life--;
                            if (Blocks[i]._life == 0)
                            {
                                Blocks.RemoveAt(i);
                                i--;
                            }

                        }
                        break;
                    case 1:
                        if (DisY < rH)
                        {
                            if (DisX < rW)
                            {
                                float tmpDist = Math.Abs(circleCenter.Y + circleCenter.X - Blocks[i]._position.X - Blocks[i]._position.Y - 2 * rW) / (float)Math.Sqrt(2);
                                if (tmpDist <= radius)
                                {
                                    n = ((float)Math.Sqrt(2) * radius + Blocks[i]._position.X + (Blocks[i]._position.Y + 2 * rH) - circleCenter.X - circleCenter.Y) / (Inverse.X + Inverse.Y);
                                    if (n < 0)
                                        n = (-1f * (float)Math.Sqrt(2) * radius + Blocks[i]._position.X + (Blocks[i]._position.Y + 2 * rH) - circleCenter.X - circleCenter.Y) / (Inverse.X + Inverse.Y);
                                    Reflect.X = 1;
                                    Reflect.Y = 1;
                                }
                            }
                            else if (circleCenter.X > Blocks[i]._position.X + rW)
                            {
                                n = (Blocks[i]._position.X + 2 * rW + radius - circleCenter.X) * 1.0f / Inverse.X;
                                Reflect.X = 1;
                                Reflect.Y = 0;
                            }
                        }
                        else if (DisX < rW)
                        {
                            if (circleCenter.Y > Blocks[i]._position.Y + rH)
                            {
                                n = (Blocks[i]._position.Y + 2 * rH + radius - circleCenter.Y) * 1.0f / Inverse.Y;
                                Reflect.X = 0;
                                Reflect.Y = 1;
                            }
                        }
                        else if (Math.Pow(DisX - rW, 2) + Math.Pow(DisY - rH, 2) <= radius * radius)
                        {
                            float x = Blocks[i]._position.X + rW;
                            float y = Blocks[i]._position.Y + rH;
                            float a, b;
                            a = b = 0;
                            if (circleCenter.X < x)
                            {
                                a = x - rW;
                                if (circleCenter.Y < y)
                                    continue;
                                else b = y + rH;
                            }
                            else
                            {
                                a = x + rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }

                            float tmp1 = Inverse.X * Inverse.X + Inverse.Y + Inverse.Y;
                            float tmp2 = 2 * (Inverse.X * (circleCenter.X - a) + Inverse.Y * (circleCenter.Y - b));
                            float delta = (float)(tmp2 * tmp2 - tmp1 * (Math.Pow(circleCenter.X - a, 2) + Math.Pow(circleCenter.Y - b, 2) - radius * radius));
                            if (delta < 0) continue;
                            n = (float)(-tmp2 + Math.Sqrt(delta)) / tmp1;

                            Vector2 tmpPosition = circleCenter + n * Inverse;
                            Reflect.X = tmpPosition.X - a;
                            Reflect.Y = tmpPosition.Y - b;

                        }

                        if (Reflect != Vector2.Zero)
                        {
                            Reflect.Normalize();
                            _position += Inverse * n;
                            _direction = Vector2.Reflect(_direction, Reflect);
                            Blocks[i]._life--;
                            if (Blocks[i]._life == 0)
                            {
                                Blocks.RemoveAt(i);
                                i--;
                            }
                        }
                        break;
                    case 2:
                        if (DisY < rH)
                        {
                            if (DisX < rW)
                            {
                                float tmpDist = Math.Abs(circleCenter.Y - circleCenter.X + Blocks[i]._position.X - Blocks[i]._position.Y) / (float)Math.Sqrt(2);
                                if (tmpDist <= radius)
                                {
                                    n = ((float)Math.Sqrt(2) * radius + Blocks[i]._position.X - Blocks[i]._position.Y - circleCenter.X + circleCenter.Y) / (Inverse.X - Inverse.Y);
                                    if (n < 0)
                                        n = (-1f * (float)Math.Sqrt(2) * radius + Blocks[i]._position.X - Blocks[i]._position.Y - circleCenter.X + circleCenter.Y) / (Inverse.X - Inverse.Y);
                                    Reflect.X = -1;
                                    Reflect.Y = 1;
                                }
                            }
                            else if (circleCenter.X < Blocks[i]._position.X + rW)
                            {
                                n = (Blocks[i]._position.X - radius - circleCenter.X) * 1.0f / Inverse.X;
                                Reflect.X = 1;
                                Reflect.Y = 0;
                            }
                        }
                        else if (DisX < rW)
                        {
                            if (circleCenter.Y > Blocks[i]._position.Y + rH)
                            {
                                n = (Blocks[i]._position.Y + 2 * rH + radius - circleCenter.Y) * 1.0f / Inverse.Y;
                                Reflect.X = 0;
                                Reflect.Y = 1;
                            }
                        }
                        else if (Math.Pow(DisX - rW, 2) + Math.Pow(DisY - rH, 2) <= radius * radius)
                        {
                            float x = Blocks[i]._position.X + rW;
                            float y = Blocks[i]._position.Y + rH;
                            float a, b;
                            a = b = 0;
                            if (circleCenter.X < x)
                            {
                                a = x - rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }
                            else
                            {
                                a = x + rW;
                                if (circleCenter.Y < y)
                                    continue;
                                else b = y + rH;
                            }

                            float tmp1 = Inverse.X * Inverse.X + Inverse.Y + Inverse.Y;
                            float tmp2 = 2 * (Inverse.X * (circleCenter.X - a) + Inverse.Y * (circleCenter.Y - b));
                            float delta = (float)(tmp2 * tmp2 - tmp1 * (Math.Pow(circleCenter.X - a, 2) + Math.Pow(circleCenter.Y - b, 2) - radius * radius));
                            if (delta < 0) continue;
                            n = (float)(-tmp2 + Math.Sqrt(delta)) / tmp1;

                            Vector2 tmpPosition = circleCenter + n * Inverse;
                            Reflect.X = tmpPosition.X - a;
                            Reflect.Y = tmpPosition.Y - b;

                        }

                        if (Reflect != Vector2.Zero)
                        {
                            Reflect.Normalize();
                            _position += Inverse * n;
                            _direction = Vector2.Reflect(_direction, Reflect);
                            Blocks[i]._life--;
                            if (Blocks[i]._life == 0)
                            {
                                Blocks.RemoveAt(i);
                                i--;
                            }
                        }
                        break;
                    case 3:
                        if (DisY < rH)
                        {
                            if (DisX < rW)
                            {
                                float tmpDist = Math.Abs(circleCenter.Y - circleCenter.X + Blocks[i]._position.X - Blocks[i]._position.Y) / (float)Math.Sqrt(2);
                                if (tmpDist <= radius)
                                {
                                    n = ((float)Math.Sqrt(2) * radius + Blocks[i]._position.X - Blocks[i]._position.Y - circleCenter.X + circleCenter.Y) / (Inverse.X - Inverse.Y);
                                    if (n < 0)
                                        n = (-1f * (float)Math.Sqrt(2) * radius + Blocks[i]._position.X - Blocks[i]._position.Y - circleCenter.X + circleCenter.Y) / (Inverse.X - Inverse.Y);
                                    Reflect.X = -1;
                                    Reflect.Y = 1;
                                }
                            }
                            else if (circleCenter.X > Blocks[i]._position.X + rW)
                            {
                                n = (Blocks[i]._position.X + 2 * rW + radius - circleCenter.X) * 1.0f / Inverse.X;
                                Reflect.X = 1;
                                Reflect.Y = 0;
                            }
                        }
                        else if (DisX < rW)
                        {
                            if (circleCenter.Y < Blocks[i]._position.Y + rH)
                            {
                                n = (Blocks[i]._position.Y - radius - circleCenter.Y) * 1.0f / Inverse.Y;
                                Reflect.X = 0;
                                Reflect.Y = 1;
                            }
                        }
                        else if (Math.Pow(DisX - rW, 2) + Math.Pow(DisY - rH, 2) <= radius * radius)
                        {
                            float x = Blocks[i]._position.X + rW;
                            float y = Blocks[i]._position.Y + rH;
                            float a, b;
                            a = b = 0;
                            if (circleCenter.X < x)
                            {
                                a = x - rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else continue;
                            }
                            else
                            {
                                a = x + rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }

                            float tmp1 = Inverse.X * Inverse.X + Inverse.Y + Inverse.Y;
                            float tmp2 = 2 * (Inverse.X * (circleCenter.X - a) + Inverse.Y * (circleCenter.Y - b));
                            float delta = (float)(tmp2 * tmp2 - tmp1 * (Math.Pow(circleCenter.X - a, 2) + Math.Pow(circleCenter.Y - b, 2) - radius * radius));
                            if (delta < 0) continue;
                            n = (float)(-tmp2 + Math.Sqrt(delta)) / tmp1;

                            Vector2 tmpPosition = circleCenter + n * Inverse;
                            Reflect.X = tmpPosition.X - a;
                            Reflect.Y = tmpPosition.Y - b;

                        }

                        if (Reflect != Vector2.Zero)
                        {
                            Reflect.Normalize();
                            _position += Inverse * n;
                            _direction = Vector2.Reflect(_direction, Reflect);
                            Blocks[i]._life--;
                            if (Blocks[i]._life == 0)
                            {
                                Blocks.RemoveAt(i);
                                i--;
                            }
                        }
                        break;
                    case 4:
                        if (DisY < rH)
                        {
                            if (DisX < rW)
                            {
                                float tmpDist = Math.Abs(circleCenter.Y + circleCenter.X - Blocks[i]._position.X - Blocks[i]._position.Y - 2 * rW) / (float)Math.Sqrt(2);
                                if (tmpDist <= radius)
                                {
                                    n = ((float)Math.Sqrt(2) * radius + Blocks[i]._position.X + (Blocks[i]._position.Y + 2 * rH) - circleCenter.X - circleCenter.Y) / (Inverse.X + Inverse.Y);
                                    if (n < 0)
                                        n = (-1f * (float)Math.Sqrt(2) * radius + Blocks[i]._position.X + (Blocks[i]._position.Y + 2 * rH) - circleCenter.X - circleCenter.Y) / (Inverse.X + Inverse.Y);
                                    Reflect.X = 1;
                                    Reflect.Y = 1;
                                }
                            }
                            else if (circleCenter.X < Blocks[i]._position.X + rW)
                            {
                                n = (Blocks[i]._position.X - radius - circleCenter.X) * 1.0f / Inverse.X;
                                Reflect.X = 1;
                                Reflect.Y = 0;
                            }
                        }
                        else if (DisX < rW)
                        {
                            if (circleCenter.Y < Blocks[i]._position.Y + rH)
                            {
                                n = (Blocks[i]._position.Y - radius - circleCenter.Y) * 1.0f / Inverse.Y;
                                Reflect.X = 0;
                                Reflect.Y = 1;
                            }
                        }
                        else if (Math.Pow(DisX - rW, 2) + Math.Pow(DisY - rH, 2) <= radius * radius)
                        {
                            float x = Blocks[i]._position.X + rW;
                            float y = Blocks[i]._position.Y + rH;
                            float a, b;
                            a = b = 0;
                            if (circleCenter.X < x)
                            {
                                a = x - rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else b = y + rH;
                            }
                            else
                            {
                                a = x + rW;
                                if (circleCenter.Y < y)
                                    b = y - rH;
                                else continue;
                            }

                            float tmp1 = Inverse.X * Inverse.X + Inverse.Y + Inverse.Y;
                            float tmp2 = 2 * (Inverse.X * (circleCenter.X - a) + Inverse.Y * (circleCenter.Y - b));
                            float delta = (float)(tmp2 * tmp2 - tmp1 * (Math.Pow(circleCenter.X - a, 2) + Math.Pow(circleCenter.Y - b, 2) - radius * radius));
                            if (delta < 0) continue;
                            n = (float)(-tmp2 + Math.Sqrt(delta)) / tmp1;

                            Vector2 tmpPosition = circleCenter + n * Inverse;
                            Reflect.X = tmpPosition.X - a;
                            Reflect.Y = tmpPosition.Y - b;

                        }

                        if (Reflect != Vector2.Zero)
                        {
                            Reflect.Normalize();
                            _position += Inverse * n;
                            _direction = Vector2.Reflect(_direction, Reflect);
                            Blocks[i]._life--;
                            if (Blocks[i]._life == 0)
                            {
                                Blocks.RemoveAt(i);
                                i--;
                            }
                        }
                        break;

                }
            }
        }

        private void BallMove()
        {
            _position += _speed * _direction;
            float n;

            if (_position.X <= _border.X)
            {
                Vector2 tmp = new Vector2(-2, 0);
                tmp.Normalize();
                //n = (_texture.Width / 2f - (_position.X + _texture.Width/2f)) / (-_direction.X);
                n = (_border.X - _position.X) / (-_direction.X);
                _position += -_direction * n;
                _direction = Vector2.Reflect(_direction, tmp);
            }
            if (_position.Y <= _border.Y)
            {
                Vector2 tmp = new Vector2(0, 1);
                tmp.Normalize();
                //n = (_texture.Height / 2f - (_position.Y + _texture.Height / 2f)) / (-_direction.Y);
                n = (_border.Y - _position.Y) / (-_direction.Y);
                _position += -_direction * n;
                _direction = Vector2.Reflect(_direction, tmp);
            }
            if (_position.X + _texture.Width >= this.widthWindows + _border.X)
            {
                Vector2 tmp = new Vector2(-1, 0);
                tmp.Normalize();
                //n = (widthWindows - _texture.Width / 2f - (_position.X + _texture.Width / 2f)) / (-_direction.X);
                n = (_border.X + widthWindows - _position.X - _texture.Width) / (-_direction.X);
                _position += -_direction * n;
                _direction = Vector2.Reflect(_direction, tmp);
            }
            if (_position.Y + _texture.Height >= this.heightWindows + _border.Y)
            {
                //n = (heightWindows - _texture.Height / 2f - (_position.Y + _texture.Height / 2f)) / (-_direction.Y);
                n = (_border.Y + heightWindows - _position.Y - _texture.Height) / (-_direction.Y);
                _position += -_direction * n;
                _direction = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
        public void setWH(int width, int height)
        {
            this.widthWindows = width;
            this.heightWindows = height;
        }
    }
}
