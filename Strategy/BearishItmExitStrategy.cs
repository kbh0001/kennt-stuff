using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BearishItmExitStrategy : MoveExitStrategyBase
    {

        private double _exitAt;

        public BearishItmExitStrategy(double exitAmount)
        {
            _exitAt = exitAmount;
        }

        public override string ExitStategyDescr
        {
            get { return "Hold untill settlement"; }
        }


        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            return false;

        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low)
        {
            if (high >= _exitAt)
                return true;

            return false;
        }
    }
}