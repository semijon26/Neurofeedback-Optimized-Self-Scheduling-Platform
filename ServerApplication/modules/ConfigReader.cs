using System.Text.Json;
using ServerApplication.Model;
using Shared;

namespace ServerApplication.modules;

public static class ConfigReader
	// Klasse für das Einlesen der Config-File
{

	private static StreamReader _reader;
	private const string PathToProjectRoot = "../../../";

	public static ConfigObject ReadConfigFromJson()
	{
		string json = ReadJsonFile("config.json");
		ConfigObject obj = JsonSerializer.Deserialize<ConfigObject>(json);
        return obj;
    }
	
	public static JsonTaskGraph ReadJsonTaskGraphFromJson()
	{
		string json = ReadJsonFile("taskgraph.json");
		JsonTaskGraph jsonTaskGraph = JsonSerializer.Deserialize<JsonTaskGraph>(json);
		return jsonTaskGraph;
	}

	private static string ReadJsonFile(string fileName)
	{
		try
		{ 
			// Es wird erst versucht, die File im Verzeichnis der .exe Datei einzulesen.
			return File.ReadAllText(fileName);
		}
		catch (Exception e)
		{
			// Wenn das fehlschlägt, z.B. weil die Datei nicht vorhanden ist, wird im voraussichtlichen Projekt-Root-Verzeichnis geschaut
			Console.WriteLine(fileName + " not found in directory of .exe file. Trying to read it from project root...");
			return File.ReadAllText(PathToProjectRoot + fileName);
		}
	}

	public static void PrintJsonConfig(ConfigObject obj)
    {
        Console.WriteLine($"Host: {obj.Host}");
        Console.WriteLine("Participants:");
        foreach (var participant in obj.Participants)
        {
            Console.WriteLine($"- {participant}");
        }
        Console.WriteLine($"ServerIP: {obj.ServerIP}");
    }

}

