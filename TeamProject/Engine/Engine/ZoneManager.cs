
namespace Engine
{
    public static class ZoneManager
    {
        public static int GlobalTemperature { get; set; } = -5;

        public static Zone[,] Zones { get; set; }
        public static Zone CurrentZone { get; set; }

    }
}
