
using Engine.TileGrid;

namespace Engine.PathFinding
{
    public class TileCosts
    {

        ///<summary>Cost from this tile going to start through discovered paths</summary>
        public double FromStart { get; set; }
        ///<summary>Cost from this tile directly to goal</summary>
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
