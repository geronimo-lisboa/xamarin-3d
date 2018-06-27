using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin3d.model.OpenGLInfrastructure;
using Xamarin3d.utilities;

namespace Xamarin3d.model
{
    public class Object3d : IRotatable
    {
        private float[] modelMatrix, rotationMatrix, translationMatrix;
        private float[] vertices;
        private float[] colors;
        //TODO: no futuro isso deve ser incorporado à uma classe de Material, mas no momento pode ficar assim.
        private ShaderProgram shaderProgram;
        private int[] idTextura = { -1 };
        float[] b = new float[64 * 3];
        public Object3d(float[] _vertexes, float[] _colors, RawImageLoader imageLoader)
        {
            modelMatrix = GLCommon.Identity();
            rotationMatrix = (float[])modelMatrix;
            translationMatrix = (float[])modelMatrix;
            //TODO: esses vertices estão aqui só pra teste. Mais pra frente eles virão de sources.
            vertices = _vertexes;
            colors = _colors;
            //TODO: no futuro isso deve ser incorporado à uma classe de Material, mas no momento pode ficar assim.
            ShaderSourceLoader shaderSource = new ShaderSourceLoader("simpleVertexShader.glsl", "simpleFragmentShader.glsl");
            shaderProgram = new ShaderProgram(shaderSource.VertexShaderSourceCode, shaderSource.FragmentShaderSourceCode);
            //TODO: criação da textura no opengl tem que ir pra uma classe apropriada pra isso.
            GL.GenTextures(1, idTextura);
            GL.BindTexture(All.Texture2D, idTextura[0]);
            GL.TexParameter(All.Texture2D, All.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(All.Texture2D, All.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(All.Texture2D, All.TextureWrapS, (int)All.ClampToEdge);
            GL.TexParameter(All.Texture2D, All.TextureWrapT, (int)All.ClampToEdge);
            //(OpenTK.Graphics.ES30.All,int,OpenTK.Graphics.ES30.All,int,int,int,OpenTK.Graphics.ES30.All,OpenTK.Graphics.ES30.All,!!0[])            
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(b.Length);
            Marshal.Copy(b, 0, unmanagedPointer, b.Length);

            //GL.TexImage2D(All.Texture2D, 0, All.Rgb, imageLoader.ImageWidth, imageLoader.ImageWidth, 0, All.Rgb, All.Byte, unmanagedPointer); 
            //GL.TexImage2D(All.Texture2D, 0, All.Rgb, imageLoader.ImageWidth, imageLoader.ImageWidth, 0, All.Rgb, All.Byte, imageLoader.ImageBuffer);


            GL.BindTexture(All.Texture2D, 0);//pára de trabalhar com a textura.
        }
        //TODO: modelMatrix tem que virar uma property;
        public void CalculateModelMatrix()
        {
            modelMatrix = GLCommon.Matrix3DMultiply(translationMatrix, rotationMatrix);
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

            CalculateModelMatrix();//TODO: quando virar propriedade isso aqui vaza
            float[] vpMat = camera.ViewProjectionMatrix;
            float[] mvpMat = GLCommon.Matrix3DMultiply(vpMat, modelMatrix);
            int mvpId = shaderProgram.GetUniformByName("mvpMatrix").Id;
            GL.UniformMatrix4(mvpId, 1, false, mvpMat);

            int numVertices = vertices.Length / 3;

            GL.DrawArrays(All.Triangles, 0, numVertices);
        }

        public void Rotate(Vector3 axis, float angleInDegree)
        {
            var newRotMat = GLCommon.Identity();
            GLCommon.Matrix3DSetRotationByDegrees(ref newRotMat, angleInDegree, axis);
            rotationMatrix = newRotMat;
        }
    }
}
