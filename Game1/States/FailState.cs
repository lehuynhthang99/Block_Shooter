﻿using MenuStart;
using MenuStart.Controls;
using MenuStart.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MenuStart.Sprite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.States
{
    class FailState : State
    {
        private List<Component> _components;
        public int _deviceWidth, _deviceHeight;

        protected Player _player;

        private Texture2D background;
        private SpriteFont Fail;

        public FailState(MenuStart.Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Player player, int DeviceWidth, int DeviceHeight) : base(game, graphicsDevice, content)
        {
            _deviceHeight = DeviceHeight;
            _deviceWidth = DeviceWidth;
            _player = player;
            background = _content.Load<Texture2D>("Backgrounds/Background2");
            Fail = _content.Load<SpriteFont>("Fonts/Fail");

            var buttonTexture = _content.Load<Texture2D>("Controls/buttons");

            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var restartGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - buttonTexture.Width) / 2,
                    (graphicsDevice.PresentationParameters.BackBufferHeight - buttonTexture.Height) / 2),
                Text = "Restart",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            restartGameButton.Click += restartGameButton_Click;

            var exitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.PresentationParameters.BackBufferWidth - buttonTexture.Width) / 2,
                    (graphicsDevice.PresentationParameters.BackBufferHeight + buttonTexture.Height) / 2),
                Text = "Quit",
                Height = graphicsDevice.PresentationParameters.BackBufferHeight,
                Width = graphicsDevice.PresentationParameters.BackBufferWidth,
                DeviceHeight = _deviceHeight,
                DeviceWidth = _deviceWidth,
            };

            exitGameButton.Click += exitGameButton_Click;

            _components = new List<Component>
            {
                restartGameButton,
                exitGameButton,
            };
        }

        private void exitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void restartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _deviceWidth, _deviceHeight));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2((spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth - background.Width) / 2, 0), Color.White);

            Vector2 posStr = Fail.MeasureString("You fail");

            spriteBatch.DrawString(Fail, "You Fail", new Vector2((spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth - posStr.X) / 2f, 10), Color.White);

            string str = "YOUR SCORE  " + _player._Score.ToString();
            posStr = _player._scorefontContent.MeasureString(str);
            spriteBatch.DrawString(_player._scorefontContent, str, new Vector2((spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth - posStr.X)/2f,200), Color.White);


            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
