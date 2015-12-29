// 
// Copyright (C) 2008, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//


using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.Design;
using NinjaTrader.Indicator;

// This namespace holds all indicators and is required. Do not change it.

namespace NinjaTrader.Indicator


{
    /// <summary>
    /// Detects common candlestick patterns and marks them on the chart.
    /// </summary>
    [Description("Ken Detects common candlestick patterns and marks them on the chart.")]
    public class KenCandleStickPattern : Indicator
    {
        private readonly Font textFont = new Font("Arial", 8, FontStyle.Regular);
        private Color downColor;
        private bool downTrend;
     
        private int patternsFound;
        private int trendStrength = 4;
        private Color txtColor;
        private Color upColor;
        private bool upTrend;

        /// <summary>
        /// Gets a value indicating if a pattern was found
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public DataSeries PatternFound
        {
            get { return Values[0]; }
        }

        [Description(
            "Number of bars required to define a trend when a pattern requires a prevailing trend. A value of zero will disable trend requirement."
            )]
        [GridCategory("Parameters")]
        [Gui.Design.DisplayName("Trend strength")]
        public int TrendStrength

        {
            get { return trendStrength; }
            set { trendStrength = Math.Max(0, value); }
        }



        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
            Add(new Plot(Color.Transparent, "Pattern Found"));
            Overlay = true;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
            var candleStickDeterminer = new KenCandleStickDeterminer(this);

            if (CurrentBar == 0 && ChartControl != null)
            {
                downColor = ChartControl.GetAxisBrush(ChartControl.BackColor).Color;
                txtColor = downColor;
                if (downColor == Color.Black)
                    upColor = Color.Transparent;
                else
                    upColor = Color.Black;
            }

            // Calculates trend lines and prevailing trend for patterns that require a trend
            if (TrendStrength > 0 && CurrentBar >= TrendStrength)
                CalculateTrendLines();

            Value.Set(0);


            if (candleStickDeterminer.IsBearishBeltHold(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColor = downColor;
                }

                DrawText("Bearish Belt Hold" + CurrentBar, false, "Bearish Belt Hold", 0, High[0], 10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(-1);
            }


            if (candleStickDeterminer.IsBearishEngulfing(TrendStrength, upTrend))
            {
                BarColor = downColor;
                DrawText("Bearish Engulfing" + CurrentBar, false, "Bearish Engulfing", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                patternsFound++;
                Value.Set(-1);
            }


            if (candleStickDeterminer.IsBearishHarami(TrendStrength, upTrend))
            {
                BarColor = downColor;
                DrawText("Bearish Harami" + CurrentBar, false, "Bearish Harami", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                patternsFound++;
                Value.Set(-1);
            }


            if (candleStickDeterminer.IsBearishHaramiCross(TrendStrength, upTrend))
            {
                BarColor = downColor;
                DrawText("Bearish Harami Cross" + CurrentBar, false, "Bearish Harami Cross", 0, Low[0], -10,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                patternsFound++;
                Value.Set(-1);
            }


            if (candleStickDeterminer.IsBullishBeltHold(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    BarColorSeries.Set(CurrentBar - 1, downColor);
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                }

                DrawText("Bullish Belt Hold" + CurrentBar, false, "Bullish Belt Hold", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsBullishEngulfing(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                }

                DrawText("Bullish Engulfing" + CurrentBar, false, "Bullish Engulfing", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsBullishHarami(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                }

                DrawText("Bullish Harami" + CurrentBar, false, "Bullish Harami", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsBullishHaramiCross(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                }

                DrawText("Bullish Harami Cross" + CurrentBar, false, "Bullish Harami Cross", 0, Low[0], -10,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsDarkCloudCover(trendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    BarColor = downColor;
                }

                DrawText("Dark Cloud Cover" + CurrentBar, false, "Dark Cloud Cover", 1, High[0], 10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsBullTrend(trendStrength, downTrend))
                return;


            if (candleStickDeterminer.IsBasicDoji())
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                }

                var yOffset = Close[0] > Close[Math.Min(1, CurrentBar)] ? 10 : -10;
                DrawText("Doji Text" + CurrentBar, false, "Doji", 0, (yOffset > 0 ? High[0] : Low[0]), yOffset,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsDownsideTasukiGap())
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    BarColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Downside Tasuki Gap" + CurrentBar, false, "Downside Tasuki Gap", 1, High[2], 10,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsEveningStar())
            {
                if (ChartControl != null)
                {
                    if (Close[0] > Open[0])
                    {
                        BarColor = upColor;
                        CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    }
                    else
                        BarColor = downColor;

                    if (Close[1] > Open[1])
                    {
                        BarColorSeries.Set(CurrentBar - 1, upColor);
                        CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    }
                    else
                        BarColorSeries.Set(CurrentBar - 1, downColor);

                    if (Close[2] > Open[2])
                    {
                        BarColorSeries.Set(CurrentBar - 2, upColor);
                        CandleOutlineColorSeries.Set(CurrentBar - 2, downColor);
                    }
                    else
                        BarColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Evening Star Text" + CurrentBar, false, "Evening Star", 1, High[1], 10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsFallingThree())
            {
                if (ChartControl != null)
                {
                    BarColor = downColor;
                    BarColorSeries.Set(CurrentBar - 4, downColor);

                    var x = 1;
                    while (x < 4)
                    {
                        if (Close[x] > Open[x])
                        {
                            BarColorSeries.Set(CurrentBar - x, upColor);
                            CandleOutlineColorSeries.Set(CurrentBar - x, downColor);
                        }
                        else
                            BarColorSeries.Set(CurrentBar - x, downColor);
                        x++;
                    }
                }

                DrawText("Falling Three Methods" + CurrentBar, false, "Falling Three Methods", 2,
                    Math.Max(High[0], High[4]), 10, txtColor, textFont, StringAlignment.Center,
                    Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsHammer(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    if (Close[0] > Open[0])
                    {
                        BarColor = upColor;
                        CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    }
                    else
                        BarColor = downColor;
                }

                DrawText("Hammer" + CurrentBar, false, "Hammer", 0, Low[0], -10, txtColor, textFont,
                    StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsHangingMan(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    if (Close[0] > Open[0])
                    {
                        BarColor = upColor;
                        CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    }
                    else
                        BarColor = downColor;
                }

                DrawText("Hanging Man" + CurrentBar, false, "Hanging Man", 0, Low[0], -10, txtColor, textFont,
                    StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsInvertedHammer(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    if (Close[0] > Open[0])
                    {
                        BarColor = upColor;
                        CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    }
                    else
                        BarColor = downColor;
                }

                DrawText("Inverted Hammer" + CurrentBar, false, "InvertedHammer", 0, High[0] + 5*TickSize, 0,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsMorningStar())
            {
                if (ChartControl != null)
                {
                    if (Close[0] > Open[0])
                    {
                        BarColor = upColor;
                        CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    }
                    else
                        BarColor = downColor;

                    if (Close[1] > Open[1])
                    {
                        BarColorSeries.Set(CurrentBar - 1, upColor);
                        CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    }
                    else
                        BarColorSeries.Set(CurrentBar - 1, downColor);

                    if (Close[2] > Open[2])
                    {
                        BarColorSeries.Set(CurrentBar - 2, upColor);
                        CandleOutlineColorSeries.Set(CurrentBar - 2, downColor);
                    }
                    else
                        BarColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Morning Star Text" + CurrentBar, false, "Morning Star", 1, Low[1], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsPiercingLine(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    BarColor = downColor;
                }

                DrawText("Piercing Line" + CurrentBar, false, "Piercing Line", 1, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsRisingThree())
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    BarColorSeries.Set(CurrentBar - 4, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 4, downColor);

                    var x = 1;
                    while (x < 4)
                    {
                        if (Close[x] > Open[x])
                        {
                            BarColorSeries.Set(CurrentBar - x, upColor);
                            CandleOutlineColorSeries.Set(CurrentBar - x, downColor);
                        }
                        else
                            BarColorSeries.Set(CurrentBar - x, downColor);
                        x++;
                    }
                }

                DrawText("Rising Three Methods" + CurrentBar, false, "Rising Three Methods", 2,
                    Math.Min(Low[0], Low[4]), -10, txtColor, textFont, StringAlignment.Center, Color.Transparent,
                    Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsShootingStar(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                    BarColor = downColor;

                DrawText("Shooting Star" + CurrentBar, false, "Shooting Star", 0, Low[0], -10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsStickSandwich())
            {
                if (ChartControl != null)
                {
                    BarColor = downColor;
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Stick Sandwich" + CurrentBar, false, "Stick Sandwich", 1,
                    Math.Min(Low[0], Math.Min(Low[1], Low[2])), -10, txtColor, textFont, StringAlignment.Center,
                    Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsThreeBlackCrows(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = downColor;
                    BarColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Three Black Crows" + CurrentBar, false, "Three Black Crows", 1, High[2], 10, txtColor,
                    textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsThreeWhiteSoldiers(TrendStrength, downTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = upColor;
                    CandleOutlineColorSeries.Set(CurrentBar, downColor);
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Three White Soldiers" + CurrentBar, false, "Three White Soldiers", 1, Low[2], -10,
                    txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsUpsideGapTwoCrows(TrendStrength, upTrend))
            {
                if (ChartControl != null)
                {
                    BarColor = downColor;
                    BarColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Upside Gap Two Crows" + CurrentBar, false, "Upside Gap Two Crows", 1,
                    Math.Max(High[0], High[1]), 10, txtColor, textFont, StringAlignment.Center,
                    Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            if (candleStickDeterminer.IsUpsideTasukiGap())
            {
                if (ChartControl != null)
                {
                    BarColor = downColor;
                    BarColorSeries.Set(CurrentBar - 1, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                    BarColorSeries.Set(CurrentBar - 2, upColor);
                    CandleOutlineColorSeries.Set(CurrentBar - 2, downColor);
                }

                DrawText("Upside Tasuki Gap" + CurrentBar, false, "Upside Tasuki Gap", 1,
                    Math.Max(High[0], High[1]), 10, txtColor, textFont, StringAlignment.Center,
                    Color.Transparent, Color.Transparent, 0);

                patternsFound++;
                Value.Set(1);
            }


            DrawTextFixed("Count", patternsFound + " patterns found", TextPosition.BottomRight);
        }


        public override string ToString()
        {
            return Name + "(" + this.GetType().Name + ")";
        }

        // Calculate trend lines and prevailing trend
        private void CalculateTrendLines()
        {
            // Calculate up trend line
            var upTrendStartBarsAgo = 0;
            var upTrendEndBarsAgo = 0;
            var upTrendOccurence = 1;

            while (Low[upTrendEndBarsAgo] <= Low[upTrendStartBarsAgo])
            {
                upTrendStartBarsAgo = Swing(TrendStrength).SwingLowBar(0, upTrendOccurence + 1, CurrentBar);
                upTrendEndBarsAgo = Swing(TrendStrength).SwingLowBar(0, upTrendOccurence, CurrentBar);

                if (upTrendStartBarsAgo < 0 || upTrendEndBarsAgo < 0)
                    break;

                upTrendOccurence++;
            }


            // Calculate down trend line	
            var downTrendStartBarsAgo = 0;
            var downTrendEndBarsAgo = 0;
            var downTrendOccurence = 1;

            while (High[downTrendEndBarsAgo] >= High[downTrendStartBarsAgo])
            {
                downTrendStartBarsAgo = Swing(TrendStrength).SwingHighBar(0, downTrendOccurence + 1, CurrentBar);
                downTrendEndBarsAgo = Swing(TrendStrength).SwingHighBar(0, downTrendOccurence, CurrentBar);

                if (downTrendStartBarsAgo < 0 || downTrendEndBarsAgo < 0)
                    break;

                downTrendOccurence++;
            }

            if (upTrendStartBarsAgo > 0 && upTrendEndBarsAgo > 0 && upTrendStartBarsAgo < downTrendStartBarsAgo)
            {
                upTrend = true;
                downTrend = false;
            }
            else if (downTrendStartBarsAgo > 0 && downTrendEndBarsAgo > 0 && upTrendStartBarsAgo > downTrendStartBarsAgo)
            {
                upTrend = false;
                downTrend = true;
            }
            else
            {
                upTrend = false;
                downTrend = false;
            }
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private KenCandleStickPattern[] cacheKenCandleStickPattern = null;

        private static KenCandleStickPattern checkKenCandleStickPattern = new KenCandleStickPattern();

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public KenCandleStickPattern KenCandleStickPattern(int trendStrength)
        {
            return KenCandleStickPattern(Input, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, int trendStrength)
        {
            if (cacheKenCandleStickPattern != null)
                for (int idx = 0; idx < cacheKenCandleStickPattern.Length; idx++)
                    if (cacheKenCandleStickPattern[idx].TrendStrength == trendStrength && cacheKenCandleStickPattern[idx].EqualsInput(input))
                        return cacheKenCandleStickPattern[idx];

            lock (checkKenCandleStickPattern)
            {
                checkKenCandleStickPattern.TrendStrength = trendStrength;
                trendStrength = checkKenCandleStickPattern.TrendStrength;

                if (cacheKenCandleStickPattern != null)
                    for (int idx = 0; idx < cacheKenCandleStickPattern.Length; idx++)
                        if (cacheKenCandleStickPattern[idx].TrendStrength == trendStrength && cacheKenCandleStickPattern[idx].EqualsInput(input))
                            return cacheKenCandleStickPattern[idx];

                KenCandleStickPattern indicator = new KenCandleStickPattern();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.TrendStrength = trendStrength;
                Indicators.Add(indicator);
                indicator.SetUp();

                KenCandleStickPattern[] tmp = new KenCandleStickPattern[cacheKenCandleStickPattern == null ? 1 : cacheKenCandleStickPattern.Length + 1];
                if (cacheKenCandleStickPattern != null)
                    cacheKenCandleStickPattern.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheKenCandleStickPattern = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.KenCandleStickPattern KenCandleStickPattern(int trendStrength)
        {
            return _indicator.KenCandleStickPattern(Input, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, int trendStrength)
        {
            return _indicator.KenCandleStickPattern(input, trendStrength);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.KenCandleStickPattern KenCandleStickPattern(int trendStrength)
        {
            return _indicator.KenCandleStickPattern(Input, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, int trendStrength)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.KenCandleStickPattern(input, trendStrength);
        }
    }
}
#endregion
