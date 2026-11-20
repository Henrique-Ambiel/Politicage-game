
public static class PresidentData
{
    public static string PresidentName = "";
    public static int SkinIndex = -1;

    public static void SavePresident(string name, int skinIndex)
    {
        PresidentName = name.Trim();
        SkinIndex = skinIndex;
    }

    public static void Reset()
    {
        PresidentName = "";
        SkinIndex = -1;
    }
}
