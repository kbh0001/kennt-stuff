#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion


// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// LearningHowToDoStuff
    /// </summary>
    [Description("LearningHowToDoStuff3")]
    public class LearnStrategy3 : Strategy
    {
       
		private bool isLong = false;
		private bool isShort = false;
		private IOrder Order;

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            CalculateOnBarClose = true;
			SetStopLoss(CalculationMode.Ticks, 8);
			SetProfitTarget(CalculationMode.Ticks, 8);
			 ExitOnClose = true;
  			  ExitOnCloseSeconds = 6000;
			EntriesPerDirection = 1;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        { 
			

			
			
			
            // Rising
            if (Close[0] > Close[1] && Close[1] > Close[2])
            {

                	Order = EnterLong(DefaultQuantity, "");
					isLong = true; 
			}
			
			if (Close[0] < Close[1] && Close[1] < Close[2] && Close[2] < Close[3])
            {

                	Order = EnterShort(DefaultQuantity, "");
					isLong = true; 
			}
        }
    }
}
