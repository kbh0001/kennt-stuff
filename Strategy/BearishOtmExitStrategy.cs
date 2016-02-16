using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BearishOtmExitStrategy : MoveExitStrategyBase
    {


        private readonly double _exitAt;
        private readonly double _settlesAt;

        public BearishOtmExitStrategy(double exitAmount, double settleAmount)
        {
            _exitAt = exitAmount;
            _settlesAt = settleAmount;
        }

        public override string ExitStategyDescr
        {

            get { return "Exit at: " + _exitAt.ToString(); }
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


        public override bool SettlesSuccessFull(MoveGenericActiveOrder order, double close)
        {
            return close < _settlesAt;
        }

        public override double CashOnSuccesfulExit
        {
            get { return 19; }
        }

        public override double CashOnSuccessfulSettle
        {
            get { return 64; }
        }

        public override double CashOnFailedSettle
        {
            get { return -35; }
        }

        public override double CashOnFailedExit
        {
            get { throw new NotImplementedException(); }
        }
    }
}