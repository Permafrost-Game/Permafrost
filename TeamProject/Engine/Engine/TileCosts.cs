
namespace Engine
{
    public class TileCosts
    {
        private double total; //Total cost of tile
        private double fromStart; //Cost from this tile going to start through discovered paths
        private double toGoal; //Cost from this tile directly to goal

        public TileCosts(double fromStart, double toGoal)
        {
            this.fromStart = fromStart;
            this.toGoal = toGoal;
            total = fromStart + toGoal; //initial calculations          
        }

        //Setter methods

        public void setFromStart(double fromStart) //Update start
        {
            this.fromStart = fromStart;
            total = fromStart + toGoal; //re-calculation
        }

        //Getter methods

        public double getFromStartCost()
        {
            return fromStart;
        }

        public double getToGoalCost()
        {
            return toGoal;
        }

        public double getTotalCost()
        {
            return total;
        }
    }
}
