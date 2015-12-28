#region Using declarations
using System;
using System.ComponentModel;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Data
{
	/// <summary>
	/// </summary>
	public class VolumeFilterBarsType : BarsType
	{
		private static bool		registered		= Register(new VolumeFilterBarsType());

		/// <summary>
		/// </summary>
		/// <param name="bars"></param>
		/// <param name="open"></param>
		/// <param name="high"></param>
		/// <param name="low"></param>
		/// <param name="close"></param>
		/// <param name="time"></param>
		/// <param name="volume"></param>
		/// <param name="isRealtime"></param>
		public override void Add(Bars bars, double open, double high, double low, double close, DateTime time, long volume, bool isRealtime)
		{
			
			if (volume <= bars.Period.Value2) return;
			
			if (bars.Count == 0)
			{
				while (volume > bars.Period.Value)
				{
					AddBar(bars, open, high, low, close, time, bars.Period.Value, isRealtime);
					volume -= bars.Period.Value;
				}

				if (volume > 0)
					AddBar(bars, open, high, low, close, time, volume, isRealtime);
			}
			else
			{
				long volumeTmp = 0;
				if (!bars.IsNewSession(time, isRealtime))
				{
					volumeTmp = Math.Min(bars.Period.Value - bars.GetVolume(bars.Count - 1), volume);
					if (volumeTmp > 0)
						UpdateBar(bars, open, high, low, close, time, volumeTmp, isRealtime);
				}

				volumeTmp = volume - volumeTmp;
				while (volumeTmp > 0)
				{
					AddBar(bars, open, high, low, close, time, Math.Min(volumeTmp, bars.Period.Value), isRealtime);
					volumeTmp -= bars.Period.Value;
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="barsData"></param>
		public override void ApplyDefaults(Gui.Chart.BarsData barsData)
		{
			barsData.DaysBack		= 3;
			barsData.Period.Value	= 1000;
			barsData.Period.Value2 = 100;
		}

		/// <summary>
		/// </summary>
		public override PeriodType BuiltFrom
		{
			get { return PeriodType.Tick; }
		}

		/// <summary>
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public override string ChartDataBoxDate(DateTime time)
		{
			return time.ToString(Cbi.Globals.CurrentCulture.DateTimeFormat.ShortDatePattern);
		}

		/// <summary>
		/// </summary>
		/// <param name="chartControl"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public override string ChartLabel(Gui.Chart.ChartControl chartControl, DateTime time)
		{
			return time.ToString(chartControl.LabelFormatTick, Cbi.Globals.CurrentCulture);
		}

		/// <summary>
		/// Here is how you restrict the selectable chart styles by bars type
		/// </summary>
		public override Gui.Chart.ChartStyleType[] ChartStyleTypesSupported
		{
			get { return new Gui.Chart.ChartStyleType[] { Gui.Chart.ChartStyleType.Box, Gui.Chart.ChartStyleType.CandleStick, Gui.Chart.ChartStyleType.HiLoBars, Gui.Chart.ChartStyleType.LineOnClose, 
				Gui.Chart.ChartStyleType.OHLC, Gui.Chart.ChartStyleType.Custom0, Gui.Chart.ChartStyleType.Custom1, Gui.Chart.ChartStyleType.Custom2, Gui.Chart.ChartStyleType.Custom3,
				Gui.Chart.ChartStyleType.Custom4, Gui.Chart.ChartStyleType.Custom5, Gui.Chart.ChartStyleType.Custom6, Gui.Chart.ChartStyleType.Custom7, Gui.Chart.ChartStyleType.Custom8,
				Gui.Chart.ChartStyleType.Custom9, Gui.Chart.ChartStyleType.Final0, Gui.Chart.ChartStyleType.Final1, Gui.Chart.ChartStyleType.Final2, Gui.Chart.ChartStyleType.Final3,
				Gui.Chart.ChartStyleType.Final4 }; }
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new VolumeFilterBarsType();
		}

		/// <summary>
		/// </summary>
		public override int DefaultValue
		{ 
			get { return 1000; }
		}

		/// <summary>
		/// </summary>
		public override string DisplayName
		{
			get { return "VolumeFilter"; }
		}

		/// <summary>
		/// </summary>
		/// <param name="period"></param>
		/// <param name="barsBack"></param>
		/// <returns></returns>
		public override int GetInitialLookBackDays(Period period, int barsBack)
		{ 
			return 1;
		}
	
		/// <summary>
		/// </summary>
		public override double GetPercentComplete(Bars bars, DateTime now)
		{
			return (double) bars.Get(bars.CurrentBar).Volume / bars.Period.Value;
		}

		/// <summary>
		/// </summary>
		/// <param name="propertyDescriptor"></param>
		/// <param name="period"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public override PropertyDescriptorCollection GetProperties(PropertyDescriptor propertyDescriptor, Period period, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(propertyDescriptor, period, attributes);

			// here is how you remove properties not needed for that particular bars type
			properties.Remove(properties.Find("BasePeriodType", true));
			properties.Remove(properties.Find("BasePeriodValue", true));
			properties.Remove(properties.Find("PointAndFigurePriceType", true));
			properties.Remove(properties.Find("ReversalType", true));
			//properties.Remove(properties.Find("Value2", true));

			Gui.Design.DisplayNameAttribute.SetDisplayName(properties, "Value", "\r\rValue");
			Gui.Design.DisplayNameAttribute.SetDisplayName(properties, "Value2", "\r\rFilterValue");

			return properties;
		}

		/// <summary>
		/// </summary>
		public override bool IsIntraday
		{
			get { return true; }
		}

		/// <summary>
		/// </summary>
		/// <param name="period"></param>
		/// <returns></returns>
		public override string ToString(Period period)
		{
			return string.Format("{0} VolumeFilter{1}", period.Value, (period.MarketDataType != MarketDataType.Last ? " - " + period.MarketDataType : string.Empty));
		}

		/// <summary>
		/// </summary>
		public VolumeFilterBarsType() : base(PeriodType.Custom8)
		{
		}
	}
}
