using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.States;
using MenuStart.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MenuStart.States
{
    public class GameState : State
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float timer;

        protected Player player;
        Texture2D _background;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);

            Texture2D[] BlockTextures = new Texture2D[5];

            BlockTextures[0] = content.Load<Texture2D>("Game/rec");
            BlockTextures[1] = content.Load<Texture2D>("Game/tri1");
            BlockTextures[2] = content.Load<Texture2D>("Game/tri2");
            BlockTextures[3] = content.Load<Texture2D>("Game/tri3");
            BlockTextures[4] = content.Load<Texture2D>("Game/tri4");

            Texture2D BallTexture = content.Load<Texture2D>("Game/Bullet");

            SpriteFont fontContent = content.Load<SpriteFont>("Fonts/File");

            SpriteFont scorefontContent = content.Load<SpriteFont>("Fonts/Score");

            Texture2D arrowTexture = content.Load<Texture2D>("Game/Arrow-Transparent");

            Texture2D borderTexture = content.Load<Texture2D>("Game/border");

            Texture2D background = content.Load<Texture2D>("Backgrounds/background2");

            Texture2D UFOTexture = content.Load<Texture2D>("Game/UFO");

            player = new Player(BlockTextures, BallTexture, fontContent, arrowTexture, borderTexture, scorefontContent, UFOTexture, graphicsDevice);
            _background = background;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            Vector2 tmp = new Vector2(1920f/_background.Width, 1080f/ _background.Height);
            spriteBatch.Draw(_background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, tmp, SpriteEffects.None, 1f);
            player.Draw(spriteBatch);

            spriteBatch.End();

            //base.Draw(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            if(player._HasDied == true)
                _game.ChangeState(new FailState(_game, _graphicsDevice, _content));

            // TODO: Add your update logic here

            player.Update(gameTime);

            //base.Update(gameTime);
        }
    }
}
