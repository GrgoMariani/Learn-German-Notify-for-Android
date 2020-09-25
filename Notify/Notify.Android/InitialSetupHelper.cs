using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ICSharpCode.SharpZipLib.Zip;

namespace Notify.Droid
{
    public class InitialSetupHelper
    {
        private readonly string filename;

        public InitialSetupHelper(string filename)
        {
            this.filename = filename;
        }

        public void CopyDBFromAssets()
        {
            if (!File.Exists(GetPlatformDBPath(filename)))
            {
                CopyCompressedDatabaseIfNeeded();
                ExtractCompressedDatabase();
                DeleteCompressedDatabase();
            }
        }

        public string GetPlatformDBPath(string filepath)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filepath);
        }

        public string AppendZipExtension(string filepath)
        {
            return $"{filepath}.zip";
        }

        private void CopyCompressedDatabaseIfNeeded()
        {
            var dbPath = GetPlatformDBPath(filename);
            var zippedPath = AppendZipExtension(dbPath);
            var zipName = AppendZipExtension(filename);
            var forcecopy = false;
            if (!File.Exists(zippedPath) || forcecopy)
            {
                using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open(zipName)))
                {
                    using (var bw = new BinaryWriter(new FileStream(zippedPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }
        }

        private void ExtractCompressedDatabase()
        {
            var dbPath = GetPlatformDBPath(filename);
            var zippedPath = AppendZipExtension(dbPath);
            var forcecopy = false;
            if (File.Exists(zippedPath) || forcecopy)
            {
                UnzipFile(zippedPath, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            }
        }

        private void DeleteCompressedDatabase()
        {
            var dbPath = GetPlatformDBPath(filename);
            var zippedPath = AppendZipExtension(dbPath);
            if (File.Exists(zippedPath))
            {
                File.Delete(zippedPath);
            }
        }

        private bool UnzipFile(string zipFilePath, string unzipFolderPath)
        {
            try
            {
                var entry = new ZipEntry(Path.GetFileNameWithoutExtension(zipFilePath));
                var fileStreamIn = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
                var zipInStream = new ZipInputStream(fileStreamIn);
                entry = zipInStream.GetNextEntry();
                while (entry != null && entry.CanDecompress)
                {
                    var outputFile = unzipFolderPath + @"/" + entry.Name;
                    var outputDirectory = Path.GetDirectoryName(outputFile);
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    if (entry.IsFile)
                    {
                        var fileStreamOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                        int size;
                        byte[] buffer = new byte[2048];
                        do
                        {
                            size = zipInStream.Read(buffer, 0, buffer.Length);
                            fileStreamOut.Write(buffer, 0, size);
                        } while (size > 0);
                        fileStreamOut.Close();
                    }

                    entry = zipInStream.GetNextEntry();
                }
                zipInStream.Close();
                fileStreamIn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsSetup()
        {
            var translationDBPath = GetPlatformDBPath(filename);
            var compressedTranslationDBPath = AppendZipExtension(translationDBPath);
            return File.Exists(translationDBPath) && !File.Exists(compressedTranslationDBPath);
        }
    }
}
