using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.States;
using MenuStart.Controls;
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
        public int _deviceWidth, _deviceHeight;

        private List<Component> _components;

        protected Player player;
        Texture2D _background;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int DeviceWidth, int DeviceHeight) : base(game, graphicsDevice, content)
        {
            _deviceHeight = DeviceHeight;
            _deviceWidth = DeviceWidth;
            spriteBatch = new SpriteBatch(graphicsDevice);

            Texture2D[] BlockTextures = new Texture2D[5];

            BlockTextures[0] = content.Load<Texture2D>("Game/rec");
            BlockTextures[1] = content.Load<Texture2D>("Game/tri1");
            BlockTextures[2] = content.Load<Texture2D>("Game/tri2");
            BlockTextures[3] = content.Load<Texture2D>("Game/tri3");
            BlockTextures[4] = content.Load<Texture2D>("Game/tri4");

            var buttonTexture = content.Load<Texture2D>("Controls/buttons");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            Texture2D BallTexture = content.Load<Texture2D>("Game/Bullet");

            SpriteFont fontContent = content.Load<SpriteFont>("Fonts/File");

            SpriteFont scorefontContent = content.Load<SpriteFont>("Fonts/Score");

            Texture2D arrowTexture = content.Load<Texture2D>("Game/Arrow-Transparent");

            Texture2D borderTexture = content.Load<Texture2D>("Game/border");

            Texture2D background = content.Load<Texture2D>("Backgrounds/background2");

            Texture2D UFOTexture = content.Load<Texture2D>("Game/UFO");

            player = new Player(BlockTextures, BallTexture, fontContent, arrowTexture, borderTexture, scorefontContent, UFOTexture, graphicsDevice);
            _background = background;

            var buttonSave = new Button(buttonTexture, buttonFont)
            {
                Position = Vector2.Zero,
                Text = "Save",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            buttonSave.Click += buttonSave_Click;

            var buttonMenu = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(50,50),
                Text = "Menu",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            buttonMenu.Click += buttonMenu_Click;

            _components = new List<Component>()
            {
                buttonSave,
                buttonMenu
            };
        }

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int DeviceWidth, int DeviceHeight, StreamReader strReader) : base(game, graphicsDevice, content)
        {
            _deviceHeight = DeviceHeight;
            _deviceWidth = DeviceWidth;
            spriteBatch = new SpriteBatch(graphicsDevice);

            Texture2D[] BlockTextures = new Texture2D[5];

            BlockTextures[0] = content.Load<Texture2D>("Game/rec");
            BlockTextures[1] = content.Load<Texture2D>("Game/tri1");
            BlockTextures[2] = content.Load<Texture2D>("Game/tri2");
            BlockTextures[3] = content.Load<Texture2D>("Game/tri3");
            BlockTextures[4] = content.Load<Texture2D>("Game/tri4");

            var buttonTexture = content.Load<Texture2D>("Controls/buttons");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            Texture2D BallTexture = content.Load<Texture2D>("Game/Bullet");

            SpriteFont fontContent = content.Load<SpriteFont>("Fonts/File");

            SpriteFont scorefontContent = content.Load<SpriteFont>("Fonts/Score");

            Texture2D arrowTexture = content.Load<Texture2D>("Game/Arrow-Transparent");

            Texture2D borderTexture = content.Load<Texture2D>("Game/border");

            Texture2D background = content.Load<Texture2D>("Backgrounds/background2");

            Texture2D UFOTexture = content.Load<Texture2D>("Game/UFO");

            int score = int.Parse(strReader.ReadLine());
            Vector2 playerPos = new Vector2(float.Parse(strReader.ReadLine()), float.Parse(strReader.ReadLine()));
            string Temp;
            List<Block> blocks = new List<Block>();
            while ((Temp = strReader.ReadLine()) != null)
            {
                    Vector2 position = new Vector2(float.Parse(Temp), float.Parse(strReader.ReadLine()));
                    int life = int.Parse(strReader.ReadLine());
                    int type = int.Parse(strReader.ReadLine());
                    Block block = new Block(BlockTextures[type], fontContent, position, type, life);
                    blocks.Add(block);

            }
            strReader.Close();
            player = new Player(BlockTextures, BallTexture, fontContent, arrowTexture, borderTexture, scorefontContent, UFOTexture, graphicsDevice, 
                score,
                playerPos,
                blocks
                );
            _background = background;

            var buttonSave = new Button(buttonTexture, buttonFont)
            {
                Position = Vector2.Zero,
                Text = "Save",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            buttonSave.Click += buttonSave_Click;

            var buttonMenu = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(50, 50),
                Text = "Menu",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            buttonMenu.Click += buttonMenu_Click;

            _components = new List<Component>()
            {
                buttonSave,
                buttonMenu
            };
        }


        private void buttonMenu_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _deviceWidth, _deviceHeight));
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter("Save.txt");
            streamWriter.WriteLine(player._Score);
            streamWriter.WriteLine(player._position.X);
            streamWriter.WriteLine(player._position.Y);
            foreach (Block block in player.Blocks)
            {
                streamWriter.WriteLine(block._position.X);
                streamWriter.WriteLine(block._position.Y);
                streamWriter.WriteLine(block._life);
                streamWriter.WriteLine(block._type);
            }
            streamWriter.Close();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            Vector2 tmp = new Vector2(1920f/_background.Width, 1080f/ _background.Height);
            spriteBatch.Draw(_background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, tmp, SpriteEffects.None, 1f);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

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

            if(player._HasDied==true)
                _game.ChangeState(new FailState(_game, _graphicsDevice, _content, player, _deviceWidth, _deviceHeight));

            // TODO: Add your update logic here

            player.Update(gameTime);
            foreach (Component component in _components)
                component.Update(gameTime);

            //base.Update(gameTime);
        }
    }
}
