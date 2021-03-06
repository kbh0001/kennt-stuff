// This namespace holds all strategies and is required. Do not change it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using KenNinja;
using NinjaTrader.Cbi;
using NinjaTrader.Indicator;
using System.Linq;

namespace NinjaTrader.Custom.Strategy
{
	/// <summary>
	/// Get Some Data
	/// </summary>
	/// 
	/// 
	/// 
	//
	[Description("Get Some Data")]
	public class StocCross : NinjaTrader.Strategy.Strategy
	{
		// User defined variables (add any user defined variables below)

		private const int TrendStrength = 4;
		private static readonly List<Kp> KpsToUse;
		private static int tradeId = 0;
		private MoveActiveOrderTracker _activeOrderTracker;
		private double _strikeWidth;


		//Configure the allowed patterns that are significant order by performance.
		static StocCross()
		{
		}


		protected override void Initialize()
		{
			var instrument = Instrument.ToString().ToUpper().Replace("DEFAULT", "").Replace(" ", "");
			_strikeWidth = NadexTwoHourBinaryStrikeWidthProvider.GetBinaryStrikeWidthFor(instrument);
			_activeOrderTracker = new MoveActiveOrderTracker();
			_activeOrderTracker.PrintFunction = Print;
			Print(string.Format("Starting for KenCandleStickStrategy {0}", Instrument));
			CalculateOnBarClose = true; //only on bar close
		}


		protected override void OnBarUpdate()
		{
			try
			{
				var now = DateTime.Parse(Time.ToString());
				_activeOrderTracker.HandleCurrentOrders(now, Open[0], Close[0], High[0], Low[0]);

				
				Print(_activeOrderTracker.GetStatusReport());

			
				



				var isBull = IsBull();
				var isBear = IsBear();
				var volScore = GetVoltilityScore();


				if (isBull && volScore == Volatility.High && (new int[] { 10, 20, 30 }).Contains(now.Minute) && IsBullOtmReady())
				{
			 
					var expiryTime = now.AddHours(1);

					var
						order = new MoveGenericActiveOrder
						{
							Id = Guid.NewGuid(),
							Time = now,
							ExpiryHour = expiryTime.Hour,
							ExpiryDay = expiryTime.Day,
							EnteredAt = Close[0],
							StrikeWidth = _strikeWidth
						};

					order.IsLong = true;
					order.SuccessFullySettlesAt = Close[0] + (Math.Abs(_strikeWidth*.25));
					order.ExitStrategy = new BullishOtmExitStrategy(Close[0] + (Math.Abs(_strikeWidth) * 1.5), order.SuccessFullySettlesAt);
					SendNotification(order);
					_activeOrderTracker.AddOrder(order);
				}


				else if (isBear && volScore == Volatility.High && (new int[] { 10, 20, 30 }).Contains(now.Minute))
				{
					var expiryTime = now.AddHours(1);

					var
						order = new MoveGenericActiveOrder
						{
							Id = Guid.NewGuid(),
							Time = now,
							ExpiryHour = expiryTime.Hour,
							ExpiryDay = expiryTime.Day,
							EnteredAt = Close[0],
							StrikeWidth = _strikeWidth
						};


					order.IsLong = false;
					order.SuccessFullySettlesAt = Close[0] - (Math.Abs(_strikeWidth*.25));
					order.ExitStrategy = new BearishOtmExitStrategy(Close[0] - (Math.Abs(_strikeWidth) * 1.5), order.SuccessFullySettlesAt);
					SendNotification(order);
					_activeOrderTracker.AddOrder(order);
				}

				
				else if  (false && isBull && volScore == Volatility.Low && (new int[]{40,50}).Contains(now.Minute))
				{
					var expiryTime = now;

					var
						order = new MoveGenericActiveOrder
						{
							Id = Guid.NewGuid(),
							Time = now,
							ExpiryHour = expiryTime.Hour,
							ExpiryDay = expiryTime.Day,
							EnteredAt = Close[0],
							StrikeWidth = _strikeWidth,
							SuccessFullySettlesAt = Close[0] - (Math.Abs(_strikeWidth * .25)),
							IsLong = true,
							ExitStrategy = new BullishItmExitStrategy(Close[0] - (Math.Abs(_strikeWidth * 5)))
						};

					SendNotification(order);


					_activeOrderTracker.AddOrder(order);
				}
				else if (false && isBear && volScore == Volatility.Low && (new int[] { 50 }).Contains(now.Minute))
				{
					var expiryTime = now;

					var
						order = new MoveGenericActiveOrder
						{
							Id = Guid.NewGuid(),
							Time = now,
							ExpiryHour = expiryTime.Hour,
							ExpiryDay = expiryTime.Day,
							EnteredAt = Close[0],
							StrikeWidth = _strikeWidth,
							SuccessFullySettlesAt = Close[0] + (Math.Abs(_strikeWidth * .25)),
							IsLong = false,
							ExitStrategy = new BearishItmExitStrategy(Close[0] + (Math.Abs(_strikeWidth * 5)))
						};

					SendNotification(order);


					_activeOrderTracker.AddOrder(order);
				}
				 
			}
			catch (Exception e)
			{
				Print("error found:" + e.Message + " " + e.Source + " " + e.StackTrace);
				//Log("error found:" + e.Message + " " + e.Source + " " + e.StackTrace, LogLevel.Error);
			}
		}

		private bool IsBear()


		{
			return IsBearCrossOver(0) && Slope(StochasticsFunc().K, 1, 0) < 0 && (Close[0] < Open[0]);
		}

		private bool IsBearCrossOver(int barsAgo)
		{
			return StochasticsFunc().D[barsAgo] > StochasticsFunc().K[barsAgo] &&
				   StochasticsFunc().D[barsAgo + 1] < StochasticsFunc().K[barsAgo + 1]
				   && (Close[barsAgo] < Open[barsAgo]);
		}

		private bool IsBull()
		{
			var baseRules = IsBullCrossOver(0) && Slope(StochasticsFunc().K, 1, 0) > 0 && (Close[0] > Open[0]);
			return baseRules;

			if (!baseRules)
				return false;
				 
		

			for (int i = 1; i < 6; i++)
			{
				if (IsBearCrossOver(i))
					return Close[i] - Open[0] > _strikeWidth;
			}
			return false;
		}

		private bool IsBullOtmReady()
		{
			return true;
			var map = new Dictionary<int, int>() {{10, 0}, {20, 1}, {30, 2}};
			var now = DateTime.Parse(Time.ToString()).Minute;

			if (map.ContainsKey(now))
			{
				return Open[0] <= (Open[map[now]] - _strikeWidth);
			}


			

			return false;


		}

		private bool IsBullCrossOver(int barsAgo)
		{
			return StochasticsFunc().D[barsAgo] < StochasticsFunc().K[barsAgo] &&
				   StochasticsFunc().D[barsAgo + 1] > StochasticsFunc().K[barsAgo + 1];
		}


		private Stochastics StochasticsFunc()
		{
			return Stochastics(3, 7, 3);
		}


		private Volatility GetVoltilityScore()
		{
			var bolRange = Bollinger(2, 12).Upper[0] - Bollinger(2, 12).Lower[0];
		


			if (bolRange > _strikeWidth*4)
				return Volatility.High;

	   
		 
			if (bolRange < _strikeWidth*2)
				return Volatility.Low;


			return Volatility.Medium;
			
		}


		private void SendNotification(MoveGenericActiveOrder order)
		{
			var instrumentName = Instrument.ToString().Replace("Default", "").Replace(" ", "").Replace("$", "");

			var mailSubject = string.Format("KC-SIGNAL-{0}:  {1} @  {2}", instrumentName,
				(order.IsLong) ? "BULL" : "BEAR",
				Close[0]);
			var mailContentTemplate = @"A {0} signal was observed in '{1}' at {2} at a closing price of {3}.
{4}
Strike Width: {5}
Strategy: {6}
";
			var mailContent = string.Format(mailContentTemplate, (order.IsLong) ? "BULL" : "BEAR", Instrument, Time,
				Close[0], order.ExitStrategy.ExitStategyDescr, order.StrikeWidth, order.ExitStrategy.GetType().Name);

			if (Historical)
				return;
			SendMail("hoskinsken@gmail.com", "hoskinsken@gmail.com", mailSubject, mailContent);
		}
	}
}