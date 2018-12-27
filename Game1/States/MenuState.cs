using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuStart.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MenuStart.States
{
    public class MenuState : State
    {
        private List<Component> _components;

        private Texture2D background, gameName;

        public int _deviceWidth, _deviceHeight;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int DeviceWidth, int DeviceHeight) : base(game, graphicsDevice, content)
        {
            _deviceHeight = DeviceHeight;
            _deviceWidth = DeviceWidth;
            
            background = _content.Load<Texture2D>("Backgrounds/Background");
            gameName = _content.Load<Texture2D>("Backgrounds/Block_Shooter");

            var buttonTexture = _content.Load<Texture2D>("Controls/buttons");

            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - buttonTexture.Width) / 2,
                    (graphicsDevice.PresentationParameters.BackBufferHeight - buttonTexture.Height) / 2),
                Text = "New Game",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight =_deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - buttonTexture.Width) / 2,
                    (graphicsDevice.PresentationParameters.BackBufferHeight + buttonTexture.Height) / 2),
                Text = "Load Game",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - buttonTexture.Width) / 2,
                    (graphicsDevice.PresentationParameters.BackBufferHeight + 3*buttonTexture.Height) / 2),
                Text = "Quit Game",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game"); 
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _deviceWidth, _deviceHeight));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2((spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth - background.Width)/2, 0), Color.White);

            spriteBatch.Draw(gameName, new Vector2((spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth - gameName.Width) / 2, 10), Color.White); 

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //Remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
