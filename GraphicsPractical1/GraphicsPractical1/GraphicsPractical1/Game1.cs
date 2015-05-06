using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GraphicsPractical1
{
    public class Game1 : Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private FrameRateCounter frameRateCounter;
        private BasicEffect effect;
        private Camera camera;
        private Terrain terrain;

        #endregion

        // The construct for the game class.
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Adding of the FrameRateCounter.
            this.frameRateCounter = new FrameRateCounter(this);
            this.Components.Add(this.frameRateCounter);
        }

        #region Methods

        // The initialize method of the game class. Used for graphics options.
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.ApplyChanges();

            this.IsFixedTimeStep = false;

            base.Initialize();
        }

        // The loadContent method of the game class. Used to load all objects/effects.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading of all the effect properties.
            this.effect = new BasicEffect(this.GraphicsDevice);
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = true;
            this.effect.DirectionalLight0.Enabled = true;
            this.effect.DirectionalLight0.DiffuseColor = new Color(202, 173, 66).ToVector3();
            this.effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            this.effect.AmbientLightColor = new Vector3(0.3f);

            // Loading of the heightmap.
            Texture2D map = Content.Load<Texture2D>("heightmap");
            this.terrain = new Terrain(new HeightMap(map), 0.2f, this.GraphicsDevice);

            // Loading of the camera and its position.
            this.camera = new Camera(new Vector3(0, 60, 0), new Vector3(0, 0, -10), new Vector3(0, 1, 0));
        }

        #region GameLoop
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Info used for the FrameRateCounter.
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;

            camera.Update(gameTime);
            base.Update(gameTime);
        }

        // The Draw method of the game class.
        protected override void Draw(GameTime gameTime)
        {
            // Configuration of how to draw the triangles.
            this.GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

            // Coloring the background.
            GraphicsDevice.Clear(Color.DarkBlue);

            // Passing the matrices in order to draw everyhitng at the right place.
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;
            Matrix translation = Matrix.CreateTranslation(-0.5f * this.terrain.Width, 0, 0.5f * this.terrain.Width);
            this.effect.World = translation;

            // Processing the passes in the effect.
            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            // Drawing the terrain
            this.terrain.Draw(this.GraphicsDevice);

            base.Draw(gameTime);
        }
        #endregion
        #endregion
    }
}
