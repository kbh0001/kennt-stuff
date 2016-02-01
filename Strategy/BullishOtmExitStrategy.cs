using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BullishOtmExitStrategy : MoveExitStrategyBase
    {

        private double _exitAt;

        public BullishOtmExitStrategy(double exitAmount)
        {
            _exitAt = exitAmount;
        }
        

        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            if (high >= _exitAt)
                return true;

            return false;

        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            return false;
        }
    }
}