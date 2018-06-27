using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Xamarin3d.utilities
{
    public class RawImageLoader
    {
        public byte[] ImageBuffer { get; private set; }
        /// <summary>
        /// Todas as imagens são quadradas pra facilitar o problema
        /// </summary>
        public int ImageWidth { get; private set; }
        public RawImageLoader(string filename)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(RawImageLoader)).Assembly;
            string fullName = "Xamarin3d.rawtex.__FILE__";
            fullName = fullName.Replace("__FILE__", filename);
            Stream stream = assembly.GetManifestResourceStream(fullName);
            if (stream == null)
            {
                throw new FileNotFoundException("Arquivo " + fullName + " não encontrado.");
            }
            long length = stream.Length;
            long pos = 0;
            var reader = new System.IO.BinaryReader(stream);
            List<byte> data = new List<byte>();
            while (pos<length)
            {
                byte[] current = reader.ReadBytes(3);
                data.Add(current[0]);
                data.Add(current[1]);
                data.Add(current[2]);
                pos += 3;
            }
            ImageBuffer = data.ToArray();
            ImageWidth = (int)Math.Sqrt( data.Count );
        }
    }
}
