using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)Global_Info.AccessWindowSize.X;
            graphics.PreferredBackBufferHeight = (int)Global_Info.AccessWindowSize.Y;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Global_Info.AccessContentManager = Content;

            Global_Info.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))

            Global_Info.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(Global_Info.AccessScreenScale));

            Global_Info.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void ControlFullScreen(bool becomeFullScreen)
        {
            graphics.IsFullScreen = becomeFullScreen;

            graphics.ApplyChanges();
        }
    }
}
