using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStart.Sprite
{
    class Block : Sprite
    {
        public int _life;
        public int _type;
        protected SpriteFont _fontContent;

        public Block(Texture2D texture, SpriteFont font, Vector2 position, int type, int life)
            : base(texture, position)
        {
            _type = type;
            _life = life;
            _fontContent = font;
        }

        public void Update(GameTime gameTime)
        {
            _position.Y += 3;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
            string str = _life.ToString();
            Vector2 tmp = _fontContent.MeasureString(str);
            Vector2 fontPos = new Vector2();
            switch (_type)
            {
                case 0:
                    fontPos.X = _texture.Width - tmp.X;
                    fontPos.Y = _texture.Height - tmp.Y;
                    fontPos /= 2;
                    break;
                case 1:
                    fontPos.X = _texture.Width - tmp.X - 10;
                    fontPos.Y = _texture.Height - tmp.Y - 10;
                    break;
                case 2:
                    fontPos.X = 10;
                    fontPos.Y = _texture.Height - tmp.Y - 10;
                    break;
                case 3:
                    fontPos.X = _texture.Width - tmp.X - 10;
                    fontPos.Y = 10;
                    break;
                case 4:
                    fontPos.X = 10;
                    fontPos.Y = 10;
                    break;
            }

            spriteBatch.DrawString(_fontContent, str, _position + fontPos, Color.White);
        }
    }
}
