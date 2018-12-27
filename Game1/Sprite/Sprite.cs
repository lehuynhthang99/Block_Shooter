using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStart.Sprite
{
    public class Sprite: ICloneable
    {
        public Texture2D _texture;
        public Vector2 _position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
