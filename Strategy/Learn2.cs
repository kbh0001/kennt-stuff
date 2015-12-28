#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
    /// An example crossover strategy
    /// </summary>
    [Description("An example crossover strategy")]
    public class Learn2 : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
        #endregion
 
        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
        	CalculateOnBarClose = true;
        
		// we'll set the stop loss for our positions globally
		SetStopLoss(CalculationMode.Percent, .1);
  		SetProfitTarget(CalculationMode.Percent, .2);
	}
 
        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
		var sma1 = SMA(10);
		var sma2 = SMA(20);
		Print("OnBarUpdate");	
		if (CrossAbove(sma1, sma2, 1)) {
			EnterLong();
		}
	}
 

    }
}