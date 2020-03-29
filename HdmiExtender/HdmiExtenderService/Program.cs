using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using HdmiExtenderLib;

namespace HdmiExtenderService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // If the user starts this program with the argument "cmd", we will run as a console application.  Otherwise we will run as a Windows Service.
            if (args.Length >= 1 && args[0] == "cmd")
            {
                string localIP = "192.168.168";
                if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
                {
                    localIP = args[1];
                }

                string remoteIP = "192.168.168.55";
                if (args.Length >= 3 && !string.IsNullOrEmpty(args[2]))
                {
                    remoteIP = args[2];
                }

                ushort port = 18080;
                if (args.Length >= 4 && !string.IsNullOrEmpty(args[3]))
                {
                    ushort.TryParse(args[3], out port);
                }

                MainService svc = new MainService();
                VideoWebServer server = new VideoWebServer(port, -1, remoteIP, localIP);
                server.Start();
                Console.WriteLine("This service was run with the command line argument \"cmd\".");
                Console.WriteLine("When run without arguments, this application acts as a Windows Service.");
                Console.WriteLine();
                Console.WriteLine("Jpeg still image:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\thttp://localhost:" + port + "/image.jpg");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Motion JPEG:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\thttp://localhost:" + port + "/image.mjpg");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("PCM 48kHz, Signed 32 bit, Big Endian");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\thttp://localhost:" + port + "/audio.wav");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("When you see ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("netdrop1");
                Console.ResetColor();
                Console.Write(" or ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("netdrop2");
                Console.ResetColor();
                Console.WriteLine(" in the console, this means a frame was dropped due to data loss between the Sender device and this program.");
                Console.WriteLine();
                Console.WriteLine("Http server running on port " + port + ". Press ENTER to exit.");
                Console.ReadLine();
                Console.WriteLine("Shutting down...");
                server.Stop();
            }
            else if (args.Length >= 1 && args[0] == "help")
            {
                Console.WriteLine("There are two options to run this program: ");
                Console.WriteLine("  1. Run a service (To install the app as a service, run the following command: \"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\installutil.exe\" \"c:\\HdmiExtenderService.exe\\\")");
                Console.WriteLine("  2. Run in commandline (To run the app in commandline, run the following command: HdmiExtenderService cmd <static IP of networkadapter> <IP of HMDI sender> <port local webserver>)");
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MainService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
