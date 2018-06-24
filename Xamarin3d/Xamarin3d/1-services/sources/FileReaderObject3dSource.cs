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
            var noComments = lines.Where(str => !str.First().Equals("#"));
            //Cada linha em sua caixinha
            var vertexDefinitionLines = noComments.Where(str => str.First().Equals("v"));
            var normalsDefinitionLines = noComments.Where(str => str.First().Equals("vn"));
            var textureCoordinatesDefinitionLines = noComments.Where(str => str.First().Equals("vt"));
            var facesDefinitionsLines = noComments.Where(str => str.First().Equals("f"));
            //agora extraio os vértices
            IEnumerable<Vector3> vertexData = vertexDefinitionLines.Select(strVert => {
                string[] _separator = { " " };
                string[] coordsAsStr = strVert.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector3(float.Parse(coordsAsStr[0].Trim()),
                    float.Parse(coordsAsStr[1].Trim()),
                    float.Parse(coordsAsStr[2].Trim()));
            });
            //Agora extraio as normais
            IEnumerable<Vector3> normalData = normalsDefinitionLines.Select(strNormal => {
                string[] _separator = { " " };
                string[] coordsAsStr = strNormal.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector3(float.Parse(coordsAsStr[0].Trim()),
                    float.Parse(coordsAsStr[1].Trim()),
                    float.Parse(coordsAsStr[2].Trim()));
            });
            //Agora extraio as texture coordinates
            IEnumerable<Vector2> tcData = textureCoordinatesDefinitionLines.Select(strTC =>
            {
                string[] _separator = { " " };
                string[] coordsAsStr = strTC.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
                return new Vector2(
                    float.Parse(coordsAsStr[0].Trim()),
                    float.Parse(coordsAsStr[1].Trim())
                    );
            });
            //Agora extraio as faces
            IEnumerable<FaceData> fData = facesDefinitionsLines.Select(strF =>
            {
                string[] _sepBetweenVerts = { " " };
                string[] _strVertices = strF.Split(_sepBetweenVerts, StringSplitOptions.RemoveEmptyEntries);
                string[] _sepInsideVert = { "/" };
                string[] _strVertData0 = _strVertices[0].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                string[] _strVertData1 = _strVertices[1].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                string[] _strVertData2 = _strVertices[2].Split(_sepInsideVert, StringSplitOptions.RemoveEmptyEntries);
                FaceData face = new FaceData();
                face.v0 = int.Parse(_strVertData0[0]); face.n0 = int.Parse(_strVertData0[1]); face.tc0 = int.Parse(_strVertData0[2]);
                face.v1 = int.Parse(_strVertData1[0]); face.n0 = int.Parse(_strVertData1[1]); face.tc0 = int.Parse(_strVertData1[2]);
                face.v2 = int.Parse(_strVertData2[0]); face.n0 = int.Parse(_strVertData2[1]); face.tc0 = int.Parse(_strVertData2[2]);
                return face;
            });
            //pronto, todos os dados foram extraídos do arquivo e estão em estruturas de dados.

            //struct FaceData{
            //int v0, n0, tc0;
            //int v1, n1, tc1;
            //int v2, n2, tc2;
            //};
            //IEnumerable<FaceData> faceDatat = facesDefinitionsLines.Select(strF => {
            //    string[] _separatorBetweenFaces = { " " };
            //    string[] faces = strF.Split(_separatorBetweenFaces, StringSplitOptions.RemoveEmptyEntries);
            //    string[] _separatorInsideFace = { "/" };
            //    string[] strFD = faces[0].Split(_separatorInsideFace, StringSplitOptions.RemoveEmptyEntries);
            //});


            throw new NotImplementedException();
        }
    }
}
