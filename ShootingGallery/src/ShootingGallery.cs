namespace GameLibrary
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class ShootingGallery : Game
    {
        private const int TargetRadius = 45;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D targetSprite;
        private Texture2D crosshairsSprite;
        private Texture2D backgroundSprite;

        private SpriteFont gameFont;
        private Vector2 targetPosition = new Vector2(400, 250);
        private MouseState mouseState;
        private int score = 0;
        private bool mouseReleased = true;
        private float mouseTargetDist;
        private float timer = 10f;
        private bool gameRunning = true;

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
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");

            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (timer > 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                gameRunning = false;
            }

            mouseState = Mouse.GetState();

            mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mouseState.X, mouseState.Y));

            if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased == true)
            {
                if (mouseTargetDist < TargetRadius && gameRunning)
                {
                    score++;
                    Random random = new Random();
                    targetPosition.X = random.Next(TargetRadius, graphics.PreferredBackBufferWidth - TargetRadius + 1);
                    targetPosition.Y = random.Next(TargetRadius, graphics.PreferredBackBufferHeight - TargetRadius + 1);
                }
                mouseReleased = false;
            }

            mouseReleased |= mouseState.LeftButton == ButtonState.Released;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            if (gameRunning)
            {
                spriteBatch.Draw(targetSprite, CenterProsition(targetPosition.X, targetPosition.Y), Color.White);
            }
            spriteBatch.DrawString(gameFont, $"Score: {score.ToString()}", new Vector2(3, 3), Color.White);
            spriteBatch.DrawString(gameFont, $"Time: {Math.Ceiling(timer).ToString()}", new Vector2(3, 40), Color.White);

            spriteBatch.Draw(crosshairsSprite, new Vector2(mouseState.X - 25, mouseState.Y - 25), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 CenterProsition(float x_position, float y_position)
        {
            return new Vector2(x_position - TargetRadius, y_position - TargetRadius);
        }
    }
}
