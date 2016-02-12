using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BullishOtmExitStrategy : MoveExitStrategyBase
    {
        private readonly double _exitAt;

        public BullishOtmExitStrategy(double exitAmount)
        {
            _exitAt = exitAmount;
        }


        public override string ExitStategyDescr
        {
            get { return "Exit at: " + _exitAt; }
        }

        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low)
        {
            if (high >= _exitAt)
                return true;

            return false;
        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low)
        {
            return false;
        }
    }
}