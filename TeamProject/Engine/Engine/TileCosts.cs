
using Engine.TileGrid;

namespace Engine
{
    public class TileCosts
    {

        //Cost from this tile going to start through discovered paths
        public double FromStart { get; set; }
        //Cost from this tile directly to goal
        public double ToGoal { get; }
        public Tile Parent { get; set; }

        public TileCosts(double FromStart, double ToGoal)
        {
            Parent = null;
            this.FromStart = FromStart;
            this.ToGoal = ToGoal;
        }

        public double GetTotalCost()
        {
            return (FromStart + ToGoal);
        }

        public bool HasParent() {
            return (Parent != null);            
        }
    }
}
