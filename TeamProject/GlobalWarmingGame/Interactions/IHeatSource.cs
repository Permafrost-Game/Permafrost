using Engine;

namespace GlobalWarmingGame.Interactions
{
    interface IHeatSource
    {
        Temperature Temperature { get; set; }
        bool Heating { get; }
    }
}
