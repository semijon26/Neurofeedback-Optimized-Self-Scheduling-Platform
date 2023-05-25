namespace Shared;

[Serializable]
public class ConfigObject
{
    public string Host { get; set; }
    public List<string> Participants { get; set; }
    public string ServerIP { get; set; }
}