public class DataBuffer
{
    public static DataBuffer Instance { get; } = new();

    private DataBuffer() { }

    public int level;
    public LevelInfo levelInfo;
}
