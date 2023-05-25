using System.ComponentModel;
using System.Reflection;

namespace Shared;

[Serializable]
public enum GameType
{
    [Description("/Assets/TextGame/logo_normal.png")]
    TextGame,
    [Description("/Assets/BricketBraker/logo_normal.png")]
    BricketBraker,
    [Description("/Assets/PathPilot/logo_normal.png")]
    PathPilot,
    [Description("/Assets/MemoMaster/logo_normal.png")]
    MemoMaster,
    [Description("/Assets/ColorEcho/logo_normal.png")]
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

