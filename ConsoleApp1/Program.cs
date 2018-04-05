using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using System.IO;
using Ionic.Zip;
using Autofac;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
			var container = AutofacInit();

			Download d = new DownloadFromBossa(new ErrorMessageToFile());
			d.DownloadFile();

			IFileManager fm;
			using (var scope = container.BeginLifetimeScope())
			{
				 fm = scope.Resolve<Factory>().CreateFileManager();
			}
            var x = fm.ReadFile(@"zz\PKNORLEN.mst");

            ISignals s1 = Factory.CreateSignal("a");
            Operations op = new Operations(s1);
            s1.xx(new List<(string, double)>() { ("a", 32), ("b", 34), ("c", 38) });
            s1.dates = new string[] { "a", "b", "c" };

            Console.Write(Helper.Ctrt(s1));

            Stream TestFileStream = File.Create("ser");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, s1);
            TestFileStream.Close();
            s1.dates = new string[] { "h", "h", "j","h"};

            ISignals s2;
            if (File.Exists("ser"))
            {
                TestFileStream = File.OpenRead("ser");
                BinaryFormatter deserializer = new BinaryFormatter();
                s2 = (ISignals)deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();
            }
        }

		static IContainer AutofacInit()
		{
			var builder = new ContainerBuilder();

			// Register individual components
			builder.RegisterType<ErrorMessageToFile>()
				   .As<ErrorMessage>();

			builder.RegisterType<Factory>()
				   .AsSelf();

			return builder.Build();
		}

    }
}
