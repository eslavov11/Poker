namespace Poker.Models.Rules
{
    using Interfaces;

    public abstract class Rule : IRule
    {
        protected Rule(double current, double power)
        {
            this.Current = current;
            this.Power = power;
        }

        public double Current { get; set; }

        public double Power { get; set; }
    }
}
