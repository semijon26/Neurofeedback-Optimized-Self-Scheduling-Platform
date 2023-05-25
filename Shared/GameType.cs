using System.ComponentModel;
using System.Reflection;

namespace Shared;

[Serializable]
public enum GameType
{
    [Description("/Assets/tornado_normal.png")]
    TextGame,
    [Description("/Assets/brick_normal.png")]
    BricketBraker,
    [Description("/Assets/plane_normal.png")]
    PathPilot,
    [Description("/Assets/back_normal.png")]
    MemoMaster,
    [Description("/Assets/brain_normal.png")]
    ColorEcho
}

public static class GameTypeHelper
{
    public static string GetEnumDescription(GameType value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            DescriptionAttribute? attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        return "";
    }
}

