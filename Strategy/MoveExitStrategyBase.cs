using System;

namespace NinjaTrader.Custom.Strategy
{
    public abstract class MoveExitStrategyBase
    {
        public abstract string ExitStategyDescr { get; }

        public abstract bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low);

        public abstract bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low);

        public abstract bool SettlesSuccessFull(MoveGenericActiveOrder order, double close);

        public abstract Double CashOnSuccesfulExit { get; }
        public abstract Double CashOnSuccessfulSettle { get; }
        public abstract Double CashOnFailedSettle { get; }
        public abstract Double CashOnFailedExit { get; }
       
       
    }
}