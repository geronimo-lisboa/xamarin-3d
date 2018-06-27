using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OpenTK.Graphics.ES30;
using OpenTK;
using System.Reflection;
using System.IO;
using Xamarin3d.utilities;
using Xamarin3d.model;
using Xamarin3d.model.OpenGLInfrastructure;
using Xamarin3d.services.sources;
//TODO: Se basear no código de https://github.com/xamarin/monodroid-samples/blob/master/GLTriangle20/PaintingView.cs pra escrever o programa básico
namespace Xamarin3d
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Initialized = false;
            openglView.HasRenderLoop = true;//Sem isso aqui ele não vai usar meu ponteiro de função.
            openglView.OnDisplay = this.OpenGLViewOnDisplay;//Passa o ponteiro de função.
        }

        private float currentR = 0;
        public bool Initialized { get; private set; }

        /// <summary>
        /// Essa aqui é a função que trata da renderização da view de opengl;
        /// </summary>
        /// <param name="rect"></param>
        private void OpenGLViewOnDisplay(Xamarin.Forms.Rectangle rect)
        {
            GL.Viewport((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
            if (!Initialized)
            {
                InitializeScene(rect);
            }
            GL.ClearColor(currentR, 0, 0, 1.0f);
            GL.Clear((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            currentR += 0.01f;
            if (currentR >= 1.0f)
                currentR = 0;
            if (Initialized)
            {
                RenderScene(rect);
            }
        }


        private Object3d firstObject;
        private Camera camera;

        private void InitializeScene(Xamarin.Forms.Rectangle rect)
        {
            GL.Enable(All.Texture2D);
            GL.Enable(All.DepthTest);
            GL.Disable(All.CullFace);
            GL.Enable(All.Texture2D);
            GL.Enable(All.Blend);
            GL.BlendFunc(All.One, All.Zero);

            camera = new Camera((float)rect.Width, (float)rect.Height);
            float[] focus = new float[] { 0, 0, 0 };
            float[] eye = new float[] { 5, 0, 100 };
            float[] vup = new float[] { 0, 1, 0 };
            camera.LookAt(focus, eye, vup);

            Object3dSource firstObjectSource = new FileReaderObject3dSource();
            firstObject = firstObjectSource.Fabricate();
            Initialized = true;
        }


        float ang = 0;
        private void RenderScene(Xamarin.Forms.Rectangle rect)
        {
            camera.SetViewPort((float)rect.Width, (float)rect.Height);
            firstObject.Rotate(new Vector3(0, 1, 0), ang);
            ang = ang + 1;
            firstObject.Render(camera);
            GL.Finish();

        }
    }
}
