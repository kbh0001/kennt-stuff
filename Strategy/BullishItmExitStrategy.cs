using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BullishItmExitStrategy : MoveExitStrategyBase
    {

        private double _exitAt;

        public BullishItmExitStrategy(double exitAmount)
        {
            _exitAt = exitAmount;
        }


        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
            return false;

        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close, double high, double low)
        {
          
            if (low <= _exitAt)
                return true;

            return false;
            
        }
    }
}