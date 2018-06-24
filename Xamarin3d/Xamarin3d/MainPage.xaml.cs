﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OpenTK.Graphics.ES30;
using System.Reflection;
using System.IO;
using Xamarin3d.utilities;
using Xamarin3d.model;
using Xamarin3d.model.OpenGLInfrastructure;
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
        private void OpenGLViewOnDisplay(Rectangle rect)
        {
            if(!Initialized)
            {
                InitializeScene();
            }
            GL.ClearColor(currentR, 0, 0, 1.0f);
            GL.Clear((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            currentR += 0.01f;
            if (currentR >= 1.0f)
                currentR = 0;
            if(Initialized)
            {
                RenderScene();
            }
        }


        private void InitializeScene()
        {
            ShaderSourceLoader shaderSource = new ShaderSourceLoader("simpleVertexShader.glsl", "simpleFragmentShader.glsl");
            ShaderProgram shaderProgram = new ShaderProgram(shaderSource.VertexShaderSourceCode, shaderSource.FragmentShaderSourceCode);
           


            Initialized = true;
        }

        private void RenderScene()
        {

        }
	}
}
