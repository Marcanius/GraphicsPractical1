using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GraphicsPractical1
{
    public class Camera
    {
        private Matrix viewMatrix, projectionMatrix;
        private Vector3 up, eye, focus;
        Vector3 cameraReference;
        Matrix rotationMatrix;
        float angle;
        Vector3 transformedReference;

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;
            this.updateViewMatrix();
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);

            cameraReference = new Vector3(0, 0, 1);
        }

        public void Update(GameTime gT)
        {

            float timeStep = (float)gT.ElapsedGameTime.TotalSeconds;



            // Checking for keyboard input and applying its interaction.
            float deltaAngle = 0;
            KeyboardState kbState = Keyboard.GetState();

            // Check to see if the left key is pressed.
            if (kbState.IsKeyDown(Keys.Left))
                deltaAngle += -3 * timeStep;
            // Check to see if the right key is pressed.
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngle += 3 * timeStep;
            // Check to see if the matrix needs to be adjusted with the new angle.


            if (deltaAngle != 0)
            {
                angle += deltaAngle;
                rotationMatrix = Matrix.CreateRotationY(angle);
                transformedReference = Vector3.Transform(cameraReference, rotationMatrix);
                this.Eye = this.Eye + transformedReference / 100;
            }
        }

        private void updateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
        }

        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        public Vector3 Eye
        {
            get { return this.eye; }
            set
            {
                this.eye = value;
                this.updateViewMatrix();
            }
        }

        public Vector3 Focus
        {
            get { return this.focus; }
            set
            {
                this.focus = value;
                this.updateViewMatrix();
            }
        }
    }
}

