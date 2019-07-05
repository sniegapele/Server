using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Response
    {
        private Byte[] data = null;

        private Response(Byte[] data)
        {
            this.data = data;
        }

        public static Response From(Request request)
        {
            try
            {
                if (request == null)
                    return MakeNullRequest();

                if (request.Type == "GET")
                {
                    String file = Environment.CurrentDirectory + request.Url;
                    FileInfo f = new FileInfo(file);
                    if (f.Exists && f.Extension.Contains("."))
                    {
                        return MakeFromFile(f);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(f + "/");
                        if (!di.Exists)
                            return MakePageNotFound();

                        FileInfo[] files = di.GetFiles();
                        foreach (FileInfo ff in files)
                        {
                            String n = ff.Name;
                            if (n.Contains("default.html"))
                                return MakeFromFile(ff);
                        }
                    }
                }
                else
                {
                    return MakeMethodNotAllowed();
                }
                return MakePageNotFound();

            } catch (Exception e)
            {
                throw e;
            }
        }

        private static bool CheckErrorFiles(String path)
        {
            FileInfo f = new FileInfo(path);
            DirectoryInfo di = new DirectoryInfo(f + "/");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo ff in files)
            {
                String n = ff.Name;
                if (n.Contains("404.html"))
                    return true;
            }
            return false;
        }
        private static Response MakeFromFile(FileInfo f)
        {
            FileStream fileStream = f.OpenRead();
            BinaryReader reader = new BinaryReader(fileStream);
            Byte[] d = new byte[fileStream.Length];
            reader.Read(d, 0, d.Length);
            fileStream.Close();
            return new Response(d);
        }

        private static Response MakeNullRequest()
        {
            String file = Environment.CurrentDirectory + "/400.html";
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                return MakePageNotFound();
            return MakeFromFile(fi);
        }

        private static Response MakePageNotFound()
        {
            String file = Environment.CurrentDirectory + "/404.html";
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                throw new InvalidOperationException("404 Error Page Not Found file does not exist");
            return MakeFromFile(fi);
        }

        private static Response MakeMethodNotAllowed()
        {
            String file = Environment.CurrentDirectory + "/405.html";
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                return MakePageNotFound();
            return MakeFromFile(fi);
        }

        public void Post(NetworkStream stream)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}
