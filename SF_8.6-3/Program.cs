using System;
using System.IO;
using System.Threading;

namespace SF_8._6_3
{
    class Program
    {
        static long size;
        static void Main(string[] args)
        {
            Console.Write("Введите путь до папки:\t");
            string path = string.Format(@"{0}", Console.ReadLine());
            long sizeBefore = GetFiles(path, size);
            Console.WriteLine("Исходный размер папки {0} байт", sizeBefore);
            DeleteFiles(path);
            Thread.Sleep(1000);
            long sizeAfter = GetFiles(path, size);
            Console.WriteLine("Освобождено {0} байт", sizeBefore-sizeAfter);
            Console.WriteLine("Текущий размер папки {0} байт", sizeAfter);

            Console.ReadKey();
        }
        static void DeleteFiles(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    foreach (var file in Directory.GetFiles(path))
                    {
                        DateTime fileCreationTime = File.GetCreationTime(file);
                        TimeSpan t = DateTime.Now - fileCreationTime;
                        if (t > TimeSpan.FromMinutes(30))
                        {
                            File.Delete(file);
                            Console.WriteLine("{0} - удален как устаревший", file);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static long GetFiles(string path, long size)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dirArray = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    size += file.Length;
                }
                foreach (DirectoryInfo subFile in dirArray)
                    GetFiles(subFile.FullName, size);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return size;
        }
    }
}
