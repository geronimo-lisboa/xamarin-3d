using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
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

        public Object3d(float[] _vertexes, float[] _colors)
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
