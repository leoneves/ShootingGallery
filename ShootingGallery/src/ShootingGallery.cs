using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary
{
    public class ShootingGallery : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D targetSprite;
        Texture2D crosshairs_sprite;
        Texture2D background_sprite;

        SpriteFont game_font;
        Vector2 target_position = new Vector2(400, 250);
        const int TARGET_RADIUS = 45;
        MouseState mouse_state;
        int score = 0;
        bool mouse_released = true;
        float mouse_target_dist;
        float timer = 10f;
        bool game_running = true;

        public ShootingGallery()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            targetSprite = Content.Load<Texture2D>("target");
            crosshairs_sprite = Content.Load<Texture2D>("crosshairs");
            background_sprite = Content.Load<Texture2D>("sky");

            game_font = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (timer > 0)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                game_running = false;

            mouse_state = Mouse.GetState();

            mouse_target_dist = Vector2.Distance(target_position, new Vector2(mouse_state.X, mouse_state.Y));

            if (mouse_state.LeftButton == ButtonState.Pressed && mouse_released == true)
            {
                if (mouse_target_dist < TARGET_RADIUS && game_running)
                {
                    score++;
                    Random random = new Random();
                    target_position.X = random.Next(TARGET_RADIUS, graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                    target_position.Y = random.Next(TARGET_RADIUS, graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                }
                mouse_released = false;
            }

            mouse_released |= mouse_state.LeftButton == ButtonState.Released;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(background_sprite, new Vector2(0, 0), Color.White);
            if (game_running)
                spriteBatch.Draw(targetSprite, CenterProsition(target_position.X, target_position.Y), Color.White);

            spriteBatch.DrawString(game_font, $"Score: {score.ToString()}", new Vector2(3, 3), Color.White);
            spriteBatch.DrawString(game_font, $"Time: {Math.Ceiling(timer).ToString()}", new Vector2(3, 40), Color.White);

            spriteBatch.Draw(crosshairs_sprite, new Vector2(mouse_state.X - 25, mouse_state.Y - 25), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        Vector2 CenterProsition(float x_position, float y_position)
        {
            return new Vector2(x_position - TARGET_RADIUS, y_position - TARGET_RADIUS);
        }
    }
}
