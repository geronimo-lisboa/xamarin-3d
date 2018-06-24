using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin3d.model.OpenGLInfrastructure;
using Xamarin3d.utilities;

namespace Xamarin3d.model
{
    class Object3d
    {
        private float[] modelMatrix;
        private float[] vertices;
        private float[] colors;
        //TODO: no futuro isso deve ser incorporado à uma classe de Material, mas no momento pode ficar assim.
        private ShaderProgram shaderProgram;

        public Object3d()
        {
            modelMatrix = new float[]
            {
                1,0,0,0,
                0,1,0,0,
                0,0,1,0,
                0,0,0,1
            };
            //TODO: esses vertices estão aqui só pra teste. Mais pra frente eles virão de sources.
            vertices = new float[] {
                    0.0f, 0.5f, 0.0f,
                    -0.5f, -0.5f, 0.0f,
                    0.5f, -0.5f, 0.0f
            };
            colors = new float[]
{
                1.0f, 0.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f, 1.0f,
            };
            //TODO: no futuro isso deve ser incorporado à uma classe de Material, mas no momento pode ficar assim.
            ShaderSourceLoader shaderSource = new ShaderSourceLoader("simpleVertexShader.glsl", "simpleFragmentShader.glsl");
            shaderProgram = new ShaderProgram(shaderSource.VertexShaderSourceCode, shaderSource.FragmentShaderSourceCode);
        }

        public void Render(Camera camera)
        {
            //TODO: vai ficar dentro do material.
            shaderProgram.Use();

            int vpositionIndex = shaderProgram.GetAttributeByName("vPosition").Id;
            GL.VertexAttribPointer(vpositionIndex, 3, All.Float, false, 0, vertices);
            GL.EnableVertexAttribArray(vpositionIndex);
            shaderProgram.BindAttribute("vPosition");

            int colorIndex = shaderProgram.GetAttributeByName("vColor").Id;
            GL.VertexAttribPointer(colorIndex, 4, All.Float, false, 0, colors);
            GL.EnableVertexAttribArray(colorIndex);
            shaderProgram.BindAttribute("vColor");

            float[] matrix = camera.ViewProjectionMatrix;
            int mvp = shaderProgram.GetUniformByName("mvpMatrix").Id;
            GL.UniformMatrix4(mvp, 1, false, matrix);

            GL.DrawArrays(All.Triangles, 0, 3);
        }
    }
}
