using ServerApplication.modules;
using Shared;

namespace ServerApplication
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Logging.LogInformation("Server Application started.");
            
            // .json Config File aus .exe-Directory auslesen
            ConfigObject obj = ConfigReader.ReadConfigFromJson();
            // .json Inhalt ausgeben
            ConfigReader.PrintJsonConfig(obj);
            SocketServerService.objectConfiguration = obj;
            
            // taskgraph.json aus .exe-Directory auslesen
            var taskGraph = ConfigReader.ReadJsonTaskGraphFromJson().ConvertToTaskGraph();
            Console.WriteLine(taskGraph.PrintConnections());


            SocketServerService.Start(8000, taskGraph);

            Console.WriteLine("Socket server started. Press any key to stop...");
            Console.ReadKey(true);

            // Stop the socket server
            SocketServerService.Stop();

            Console.WriteLine("Socket server stopped. Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
