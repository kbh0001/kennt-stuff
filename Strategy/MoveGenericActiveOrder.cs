using System;

namespace NinjaTrader.Custom.Strategy
{
    public class MoveGenericActiveOrder
    {
        public double StrikeWidth { get; set; }
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public int ExpiryHour { get; set; }
        public int ExpiryDay { get; set; }
        public bool IsLong { get; set; }
        public double EnteredAt { get; set; }
        public double ExitAt { get; set; }
        public double SettleAT { get; set; }
    }
}