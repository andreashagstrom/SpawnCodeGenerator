namespace ArkSpawnCodeGen
{
    public enum SummeryEnum
    {
        BLUEPRINT,
        ENGRAM,
        PRIMAL_ITEM,
        SPAWNCODE_ITEM,
        SPAWNCODE_CREATURE
    }

    public class Summery
    {
        public SummeryEnum Type { get; set; }
        public string Value { get; set; }
    }
}
