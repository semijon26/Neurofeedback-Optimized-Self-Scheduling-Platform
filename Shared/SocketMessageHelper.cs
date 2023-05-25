using System.Runtime.Serialization.Formatters.Binary;

namespace Shared;

public static class SocketMessageHelper
{
    public static T DeserializeFromByteArray<T>(byte[] data)
    {
        T deserializedObject;

        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream(data))
        {
            deserializedObject = (T)bf.Deserialize(ms);
        }

        return deserializedObject;
    }
    
    public static byte[] SerializeToByteArray<T>(T obj) {
        byte[] data;

        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            data = ms.ToArray();
        }

        return data;
    }
}