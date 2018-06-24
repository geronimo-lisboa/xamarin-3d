using System;
using System.Collections.Generic;
using System.Text;
using Xamarin3d.utilities;

namespace Xamarin3d.model
{
    class Camera
    {
 
        public float[] ViewMatrix { get; private set; }
        public float[] ProjectionMatrix { get; private set; }
        public float[] ViewProjectionMatrix
        {
            get {
                return GLCommon.Matrix3DMultiply(ProjectionMatrix, ViewMatrix); ;
            }
        }

        public Camera(float width, float height)
        {
            ViewMatrix = MakeIdentity();
            ProjectionMatrix = MakeIdentity();

            this.SetViewPort(width, height);
        }

        public void SetViewPort(float width, float height)
        {
            var localMat = MakeIdentity();
            GLCommon.Matrix3DSetPerspectiveProjectionWithFieldOfView(ref localMat, 45.0f, 0.1f, 100.0f,
(float)(width / height));
            ProjectionMatrix = localMat;
        }

        public void LookAt(float[] focus, float[] eye, float[] viewUp)
        {
            ViewMatrix = GLCommon.LookAt(focus, eye, viewUp);
        }

        private float[] MakeIdentity()
        {
            float[] identityMatrix = new float[] {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1,
            };
            return identityMatrix;
        }
    }
}
