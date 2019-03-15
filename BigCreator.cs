using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigCreator
{
    static class BigCreator
    {
        static List<byte[]> _allFiles = new List<byte[]>();
        static List<byte> _bigData = new List<byte>();
        static List<string> _filenames;


        public static void CreateBig(string dataFolder,string outputFile,bool dontIncludeParentDir = false)
        {
            _allFiles = new List<byte[]>();
            _bigData = new List<byte>();
            _filenames = new List<string>();

            byte[] empty = new byte[] { 0, 0, 0, 0 };
            _bigData.AddRange(Encoding.UTF8.GetBytes("BIGF"));

            if (!dontIncludeParentDir)
            {
                string[] datSplit = dataFolder.Split('\\');
                ReadDir(dataFolder, datSplit[datSplit.Length - 1]);
            }
            else
            {
                ReadDir(dataFolder, "");
            }

            foreach (string s in _filenames) Console.WriteLine(s);

            uint archiveSize = 24 + (uint)_allFiles.Count * 9;
            uint headerSize = 24 + (uint)_allFiles.Count * 9;
            foreach (string s in _filenames)
            {
                archiveSize += (uint)Encoding.UTF8.GetBytes(s).Length;
                headerSize += (uint)Encoding.UTF8.GetBytes(s).Length;
            }

            foreach (byte[] a in _allFiles)
            {
                archiveSize += (uint)a.Length;
            }



            Console.WriteLine($"Number of files: {_allFiles.Count}");
            Console.WriteLine($"Header size: {headerSize}");
            Console.WriteLine(archiveSize);

            _bigData.AddRange(BitConverter.GetBytes(archiveSize)); //little endian archive size
            _bigData.AddRange(buint((uint)_allFiles.Count));
            _bigData.AddRange(buint(headerSize - 1));
            uint offset = headerSize;
            for(int i =0; i< _filenames.Count; i++)
            {
                _bigData.AddRange(buint(offset));
                _bigData.AddRange(buint(_allFiles[i].Length));
                _bigData.AddRange(Encoding.UTF8.GetBytes(_filenames[i]));
                _bigData.Add(0);
                offset += (uint)_allFiles[i].Length;
            }
            _bigData.AddRange(Encoding.UTF8.GetBytes("L225"));
            _bigData.AddRange(empty);

            foreach (byte[] a in _allFiles) _bigData.AddRange(a);
            
            Console.WriteLine($"File size: {_bigData.Count} bytes");

            File.WriteAllBytes(outputFile, _bigData.ToArray());
        }

        private static byte[] buint(uint a)
        {
            byte[] res = BitConverter.GetBytes(a);
            Array.Reverse(res);
            return res;
        }

        private static byte[] buint(int a)
        {
            byte[] res = BitConverter.GetBytes((uint)a);
            Array.Reverse(res);
            return res;
        }

        private static void ReadDir(string folder, string split)
        {
            foreach (String file in Directory.EnumerateFiles(folder))
            {
                string[] splitted = file.Split(new string[] { @"\" + split + @"\" }, StringSplitOptions.None);
                _filenames.Add(split + @"\" + splitted[splitted.Length - 1]);
                Console.WriteLine(file);
                byte[] filebytes = File.ReadAllBytes(file);
                _allFiles.Add(filebytes);
            }
            string[] dirs = Directory.GetDirectories(folder);
            foreach (String dir in dirs)
            {
                string[] datSplit = dir.Split('\\');
                string newSplit;
                if (split != "")
                    newSplit = split + @"\" + datSplit[datSplit.Length - 1];
                else newSplit = datSplit[datSplit.Length - 1];
                ReadDir(dir, newSplit  );
            }
        }
    }
}
