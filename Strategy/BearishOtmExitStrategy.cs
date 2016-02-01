using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BearishOtmExitStrategy : MoveExitStrategyBase
    {

        private double _exitAt;

        public BearishOtmExitStrategy(double exitAmount)
        {
            _exitAt = exitAmount;
        }


        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            if (low <= _exitAt)
                return true;

            return false;

        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            //always go to settlement if we have not exited.
            return false;
        }
    }
}