using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Xamarin3d.utilities
{
    public class TextFileReaderFromResources
    {
        private string CreateFullyQualifiedName(string smallName, string folder)
        {
            //string fullName = "Xamarin3d.shaders.__FILE__";
            string fullName = "Xamarin3d.__FOLDER__.__FILE__";
            fullName = fullName.Replace("__FILE__", smallName);
            fullName = fullName.Replace("__FOLDER__", folder);
            return fullName;
        }

        private string ExtractTextFromFile(Assembly assembly, string fullName)
        {

            Stream stream = assembly.GetManifestResourceStream(fullName);
            if (stream == null)
            {
                throw new FileNotFoundException("Arquivo " + fullName + " não encontrado.");
            }
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }

        public TextFileReaderFromResources(string smallName, string folder)
        {
            string fullName = CreateFullyQualifiedName(smallName,folder);
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TextFileReaderFromResources)).Assembly;
            string data = ExtractTextFromFile(assembly, fullName);
            Text = data;
        }

        public string Text { get; private set; }
    }
}
