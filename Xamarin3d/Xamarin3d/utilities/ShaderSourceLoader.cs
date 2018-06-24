using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Xamarin3d.utilities
{
    /// <summary>
    /// Serve pra carregar o código fonte de fragment shader e vertex shader, com a arquivo determinado quando
    /// o objeto é criado. Ele é imutável. Sempre retornará o código dos arquivos passados como parâmetro no começo
    /// </summary>
    class ShaderSourceLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vsFileName">O nome do arquivo. Não é o nome totalmente qualificado. A classe se vira pra saber onde ele está realmente.</param>
        /// <param name="fsFileName">O nome do arquivo. Não é o nome totalmente qualificado. A classe se vira pra saber onde ele está realmente.</param>
        public ShaderSourceLoader(string vsFileName, string fsFileName)
        {
            //System.Diagnostics.Debug.WriteLine(FragmentShaderSourceCode);
            TextFileReaderFromResources vsReader = new TextFileReaderFromResources(vsFileName, "shaders");
            VertexShaderSourceCode = vsReader.Text;
            TextFileReaderFromResources fsReader = new TextFileReaderFromResources(fsFileName, "shaders");
            FragmentShaderSourceCode = fsReader.Text;
        }

        public string VertexShaderSourceCode { get; private set; }
        public string FragmentShaderSourceCode { get; private set; }
    }
}
