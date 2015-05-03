using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GraphicsPractical1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private FrameRateCounter frameRateCounter;
        private BasicEffect effect;
        private VertexPositionColor[] vertices;
        private Camera camera;
        private float[,] heightData;
        private Terrain terrain;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.frameRateCounter = new FrameRateCounter(this);
            this.Components.Add(this.frameRateCounter);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.ApplyChanges();

            this.IsFixedTimeStep = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.effect = new BasicEffect(this.GraphicsDevice);
            this.setupVertices();
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = true;
            this.effect.DirectionalLight0.Enabled = true;
            this.effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            this.effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            this.effect.AmbientLightColor = new Vector3(0.3f);

            Texture2D map = Content.Load<Texture2D>("heightmap");
            this.terrain = new Terrain(new HeightMap(map), 0.2f);

            this.camera = new Camera(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;

            float deltaAngle = 0;
            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Left))
                deltaAngle += -3 * timeStep;
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngle += 3 * timeStep;
            if (deltaAngle != 0)
                this.camera.Eye = Vector3.Transform(this.camera.Eye, Matrix.CreateRotationY(deltaAngle));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;
            Matrix translation = Matrix.CreateTranslation(-0.5f * this.terrain.Width, 0, 0.5f * this.terrain.Width);
            this.effect.World = translation;

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            this.terrain.Draw(this.GraphicsDevice);

            base.Draw(gameTime);
        }

        private void setupVertices()
        {
            this.vertices = new VertexPositionColor[3];

            this.vertices[0].Position = new Vector3(0f, -0f, 0f);
            this.vertices[0].Color = Color.Red;
            this.vertices[1].Position = new Vector3(10f, 10f, 0f);
            this.vertices[1].Color = Color.Yellow;
            this.vertices[2].Position = new Vector3(10f, 0f, -5f);
            this.vertices[2].Color = Color.Green;
        }

        private void loadHeightData()
        {
            this.heightData = new float[4, 3];

            this.heightData[0, 0] = 0;
            this.heightData[1, 0] = 0;
            this.heightData[2, 0] = 0;
            this.heightData[3, 0] = 0;

            this.heightData[0, 1] = 0.5f;
            this.heightData[1, 1] = 0;
            this.heightData[2, 1] = -1.0f;
            this.heightData[3, 1] = 0.2f;

            this.heightData[0, 2] = 1.0f;
            this.heightData[1, 2] = 1.2f;
            this.heightData[2, 2] = 0.8f;
            this.heightData[3, 2] = 0;

        }
    }
}
