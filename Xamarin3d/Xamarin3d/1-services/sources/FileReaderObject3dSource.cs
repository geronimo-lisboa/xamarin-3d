using System;
using System.Collections.Generic;
using System.Text;
using Xamarin3d.model;
using Xamarin3d.utilities;
using System.Linq;
using OpenTK;

namespace Xamarin3d.services.sources
{
    class FileReaderObject3dSource : Object3dSource
    {
        private class FaceData
        {
            public int v0, n0, tc0;
            public int v1, n1, tc1;
            public int v2, n2, tc2;
        }
        public Object3d Fabricate()
        {
            //TODO: usar normais
            //TODO: usar texture coordinates
            //TODO: usar indexes.

            //var result = attributes.Where(attr => attr.Name.Equals(name)).ToList();
            //abre o arquivo do cubo
            //TODO: tirar esse hardcoded daqui e pas
            TextFileReaderFromResources objReader = new TextFileReaderFromResources("cube.obj3d", "objects3d");
            TextFileReaderFromResources mtlReader = new TextFileReaderFromResources("cube.mtl", "objects3d");
            //parsing dos dados
            //picota por linha;
            string[] crSeparator = { "\n" };
            List<String> lines = new List<string>(objReader.Text.Split(crSeparator, StringSplitOptions.RemoveEmptyEntries));
            //remoção dos comentários
            var noComments = lines.Where(str => !str.StartsWith("#")).ToList();

            //Cada linha em sua caixinha
            var vertexDefinitionLines = noComments.Where(str => str.StartsWith("v ")).ToList();
            var normalsDefinitionLines = noComments.Where(str => str.StartsWith("vn")).ToList();
            var textureCoordinatesDefinitionLines = noComments.Where(str => str.StartsWith("vt")).ToList();
            var facesDefinitionsLines = noComments.Where(str => str.StartsWith("f")).ToList();
            //agora extraio os vértices
            List<Vector3> vertexData = vertexDefinitionLines.Select(strVert => {
                string[] _separator = { " " };
                string[] coordsAsStr = strVert.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector3(float.Parse(coordsAsStr[1].Trim()),
                    float.Parse(coordsAsStr[2].Trim()),
                    float.Parse(coordsAsStr[3].Trim()));
            }).ToList();
            //Agora extraio as normais
            List<Vector3> normalData = normalsDefinitionLines.Select(strNormal => {
                string[] _separator = { " " };
                string[] coordsAsStr = strNormal.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector3(float.Parse(coordsAsStr[1].Trim()),
                    float.Parse(coordsAsStr[2].Trim()),
                    float.Parse(coordsAsStr[3].Trim()));
            }).ToList();
            //Agora extraio as texture coordinates
            List<Vector2> tcData = textureCoordinatesDefinitionLines.Select(strTC =>
            {
                string[] _separator = { " " };
                string[] coordsAsStr = strTC.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector2(
                    float.Parse(coordsAsStr[1].Trim()),
                    float.Parse(coordsAsStr[2].Trim())
                    );
            }).ToList();
            //Agora extraio as faces
            List<FaceData> fData = facesDefinitionsLines.Select(strF =>
            {
                string[] _sepBetweenVerts = { " " };
                string[] _strVertices = strF.Split(_sepBetweenVerts, StringSplitOptions.RemoveEmptyEntries);
                string[] _sepInsideVert = { "/" };
                string[] _strVertData0 = _strVertices[1].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                string[] _strVertData1 = _strVertices[2].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                string[] _strVertData2 = _strVertices[3].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                FaceData face = new FaceData();
                face.v0 = int.Parse(_strVertData0[0]); face.n0 = int.Parse(_strVertData0[1]); face.tc0 = int.Parse(_strVertData0[2]);
                face.v1 = int.Parse(_strVertData1[0]); face.n0 = int.Parse(_strVertData1[1]); face.tc0 = int.Parse(_strVertData1[2]);
                face.v2 = int.Parse(_strVertData2[0]); face.n0 = int.Parse(_strVertData2[1]); face.tc0 = int.Parse(_strVertData2[2]);
                return face;
            }).ToList();
            //pronto, todos os dados foram extraídos do arquivo e estão em estruturas de dados.

            //Usando os dados do fData, cria uma lista ordenada de vértices, ordenada pela informação v* do 
            //FaceData. Como eu ainda não uso indices é assim que eu tenho que mandar os dados. OBS, a lista
            //é 1-based e não 0-based como todas as outras listas do mundo.
            List<float> lstVertexBuffer = new List<float>();
            List<float> lstVertexColor = new List<float>();//TODO: isso aqui é temporário, um dia vai ser removido.
            foreach(var f in fData)
            {
                Vector3 v0 = vertexData[f.v0-1];
                lstVertexBuffer.Add(v0.X); lstVertexBuffer.Add(v0.Y); lstVertexBuffer.Add(v0.Z);
                lstVertexColor.Add(v0.X); lstVertexColor.Add(v0.Y); lstVertexColor.Add(v0.Z); lstVertexColor.Add(1);

                Vector3 v1 = vertexData[f.v1-1];
                lstVertexBuffer.Add(v1.X); lstVertexBuffer.Add(v1.Y); lstVertexBuffer.Add(v1.Z);
                lstVertexColor.Add(v1.X); lstVertexColor.Add(v1.Y); lstVertexColor.Add(v1.Z); lstVertexColor.Add(1);

                Vector3 v2 = vertexData[f.v2-1];
                lstVertexBuffer.Add(v2.X); lstVertexBuffer.Add(v2.Y); lstVertexBuffer.Add(v2.Z);
                lstVertexColor.Add(v2.X); lstVertexColor.Add(v2.Y); lstVertexColor.Add(v2.Z); lstVertexColor.Add(1);
            }

            Object3d obj = new Object3d(lstVertexBuffer.ToArray(), lstVertexColor.ToArray());
            return obj;
        }
    }
}
