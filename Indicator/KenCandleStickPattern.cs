// 
// Copyright (C) 2008, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using KenNinja;
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
        private Kp pattern = Kp.All;
        private int patternsFound;
        private int trendStrength = 4;
        private Color txtColor;
        private Color upColor;

        /// <summary>
        /// Gets a value indicating if a pattern was found
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public DataSeries PatternFound
        {
            get { return Values[0]; }
        }

        [Description("Choose a candlestick pattern to restrict chart.")]
        [GridCategory("Parameters")]
        [Gui.Design.DisplayName("Chart Pattern")]
        public Kp Pattern
        {
            get { return pattern; }
            set { pattern = value; }
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
            try
            {
                var candleStickDeterminer = new KenCandleStickDeterminer(this, TrendStrength, Print);

                if (CurrentBar == 0 && ChartControl != null)
                {
                    downColor = ChartControl.GetAxisBrush(ChartControl.BackColor).Color;
                    txtColor = downColor;
                    if (downColor == Color.Black)
                        upColor = Color.Transparent;
                    else
                        upColor = Color.Black;
                }
                Value.Set(0);

                var allowedPatterns = new List<Kp>();

                if (pattern == Kp.All)
                {
                    foreach (var dood in Enum.GetValues(typeof (Kp)))
                    {
                        allowedPatterns.Add((Kp) dood);
                    }
                }
                else
                {
                    allowedPatterns.Add(pattern);
                }


                //Start identifiying patterns


                if (allowedPatterns.Contains(Kp.BearishBeltHold))
                {
                    if (candleStickDeterminer.IsBearishBeltHold)
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
                        Value.Set(Kp.BearishBeltHold.ToInt());
                    }
                }


                if (allowedPatterns.Contains(Kp.BearishEngulfing))
                {
                    if (candleStickDeterminer.IsBearishEngulfing)
                    {
                        BarColor = downColor;
                        DrawText("Bearish Engulfing" + CurrentBar, false, "Bearish Engulfing", 0, Low[0], -10, txtColor,
                            textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                        patternsFound++;
                        Value.Set(Kp.BearishEngulfing.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishHarami))
                {
                    if (candleStickDeterminer.IsBearishHarami)
                    {
                        BarColor = downColor;
                        DrawText("Bearish Harami" + CurrentBar, false, "Bearish Harami", 0, Low[0], -10, txtColor,
                            textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                        patternsFound++;
                        Value.Set(Kp.BearishHarami.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishHaramiCross))
                {
                    if (candleStickDeterminer.IsBearishHaramiCross)
                    {
                        BarColor = downColor;
                        DrawText("Bearish Harami Cross" + CurrentBar, false, "Bearish Harami Cross", 0, Low[0], -10,
                            txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);
                        patternsFound++;
                        Value.Set(Kp.BearishHaramiCross.ToInt());
                    }
                }
                if (allowedPatterns.Contains(Kp.BullishBeltHold))
                {
                    if (candleStickDeterminer.IsBullishBeltHold)
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
                        Value.Set(Kp.BullishBeltHold.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishEngulfing))
                {
                    if (candleStickDeterminer.IsBullishEngulfing)
                    {
                        if (ChartControl != null)
                        {
                            BarColor = upColor;
                            CandleOutlineColorSeries.Set(CurrentBar, downColor);
                        }

                        DrawText("Bullish Engulfing" + CurrentBar, false, "Bullish Engulfing", 0, Low[0], -10, txtColor,
                            textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                        patternsFound++;
                        Value.Set(Kp.BullishEngulfing.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishHarami))
                {
                    if (candleStickDeterminer.IsBullishHarami)
                    {
                        if (ChartControl != null)
                        {
                            BarColor = upColor;
                            CandleOutlineColorSeries.Set(CurrentBar, downColor);
                        }

                        DrawText("Bullish Harami" + CurrentBar, false, "Bullish Harami", 0, Low[0], -10, txtColor,
                            textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                        patternsFound++;
                        Value.Set(Kp.BullishHarami.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishHaramiCross))
                {
                    if (candleStickDeterminer.IsBullishHaramiCross)
                    {
                        if (ChartControl != null)
                        {
                            BarColor = upColor;
                            CandleOutlineColorSeries.Set(CurrentBar, downColor);
                        }

                        DrawText("Bullish Harami Cross" + CurrentBar, false, "Bullish Harami Cross", 0, Low[0], -10,
                            txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                        patternsFound++;
                        Value.Set(Kp.BullishHaramiCross.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishDarkCloudCover))
                {
                    {
                        if (candleStickDeterminer.IsDarkCloudCover)
                        {
                            if (ChartControl != null)
                            {
                                CandleOutlineColorSeries.Set(CurrentBar - 1, downColor);
                                BarColorSeries.Set(CurrentBar - 1, upColor);
                                BarColor = downColor;
                            }

                            DrawText("Dark Cloud Cover" + CurrentBar, false, "Dark Cloud Cover", 1, High[0], 10,
                                txtColor,
                                textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                            patternsFound++;
                            Value.Set(Kp.BearishDarkCloudCover.ToInt());
                        }
                    }


                }

                if (allowedPatterns.Contains(Kp.BullishDoji))
                {

                    if (candleStickDeterminer.IsBullishDoji)
                    {
                        if (ChartControl != null)
                        {
                            BarColor = upColor;
                            CandleOutlineColorSeries.Set(CurrentBar, downColor);
                        }

                        var yOffset = Close[0] > Close[Math.Min(1, CurrentBar)] ? 10 : -10;
                        DrawText("Confirmed Doji Text" + CurrentBar, false, "Bullish Doji", 0,
                            (yOffset > 0 ? High[0] : Low[0]), yOffset,
                            txtColor, textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                        patternsFound++;
                        Value.Set(Kp.BullishDoji.ToInt());
                    }
                }


                if (allowedPatterns.Contains(Kp.BearishDownsideTasukiGap))
                {
                    if (candleStickDeterminer.IsDownsideTasukiGap)
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
                        Value.Set(Kp.BearishDownsideTasukiGap.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishEveningStar))
                {
                    if (candleStickDeterminer.IsEveningStar)
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
                        Value.Set(Kp.BearishEveningStar.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishFallingThreeMethods))
                {
                    if (candleStickDeterminer.IsFallingThreeMethods)
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
                        Value.Set(Kp.BearishFallingThreeMethods.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishHammer))
                {
                    if (candleStickDeterminer.IsHammer)
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
                        Value.Set(Kp.BullishHammer.ToInt());
                    }
                }


                if (allowedPatterns.Contains(Kp.BearishHangingMan))
                {
                    if (candleStickDeterminer.IsHangingMan)
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
                        Value.Set(Kp.BearishHangingMan.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishInvertedHammer))
                {
                    if (candleStickDeterminer.IsInvertedHammer)
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
                        Value.Set(Kp.BullishInvertedHammer.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishMorningStar))
                {
                    if (candleStickDeterminer.IsMorningStar)
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
                        Value.Set(Kp.BullishMorningStar.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishPiercingLine))
                {
                    if (candleStickDeterminer.IsPiercingLine)
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
                        Value.Set(Kp.BullishPiercingLine.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BullishRishingThreeMethods))
                {
                    if (candleStickDeterminer.IsRisingThree)
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
                        Value.Set(Kp.BullishRishingThreeMethods.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishShootingStar))
                {
                    if (candleStickDeterminer.IsShootingStar)
                    {
                        if (ChartControl != null)
                            BarColor = downColor;

                        DrawText("Shooting Star" + CurrentBar, false, "Shooting Star", 0, Low[0], -10, txtColor,
                            textFont, StringAlignment.Center, Color.Transparent, Color.Transparent, 0);

                        patternsFound++;
                        Value.Set(Kp.BearishShootingStar.ToInt());
                    }
                }


                if (candleStickDeterminer.IsStickSandwich)
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

                if (allowedPatterns.Contains(Kp.BearishThreeBlackCrows))
                {
                    if (candleStickDeterminer.IsThreeBlackCrows)
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
                        Value.Set(Kp.BearishThreeBlackCrows.ToInt());
                    }
                }


                if (allowedPatterns.Contains(Kp.BullishThreeWhiteSoldiers))
                {

                    if (candleStickDeterminer.IsThreeWhiteSoldiers)
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
                        Value.Set(Kp.BullishThreeWhiteSoldiers.ToInt());
                    }
                }

                if (allowedPatterns.Contains(Kp.BearishUpsideGapTwoCrows))
                {
                    if (candleStickDeterminer.IsUpsideGapTwoCrows)
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
                        Value.Set(Kp.BearishUpsideGapTwoCrows.ToInt());
                    }
                }


                if (allowedPatterns.Contains(Kp.BullishUpsideTasukiGap))
                {

                    if (candleStickDeterminer.IsUpsideTasukiGap)
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
                        Value.Set(Kp.BullishUpsideTasukiGap.ToInt());
                    }

                }


                DrawTextFixed("Count", patternsFound + " patterns found", TextPosition.BottomRight);
            }
            catch (Exception e)
            {
                Print("error found:" + e.Message + " " + e.Source + " " + e.StackTrace);
            }
        }


        public override string ToString()
        {
            return Name + "(" + GetType().Name + ")";
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
        public KenCandleStickPattern KenCandleStickPattern(Kp pattern, int trendStrength)
        {
            return KenCandleStickPattern(Input, pattern, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, Kp pattern, int trendStrength)
        {
            if (cacheKenCandleStickPattern != null)
                for (int idx = 0; idx < cacheKenCandleStickPattern.Length; idx++)
                    if (cacheKenCandleStickPattern[idx].Pattern == pattern && cacheKenCandleStickPattern[idx].TrendStrength == trendStrength && cacheKenCandleStickPattern[idx].EqualsInput(input))
                        return cacheKenCandleStickPattern[idx];

            lock (checkKenCandleStickPattern)
            {
                checkKenCandleStickPattern.Pattern = pattern;
                pattern = checkKenCandleStickPattern.Pattern;
                checkKenCandleStickPattern.TrendStrength = trendStrength;
                trendStrength = checkKenCandleStickPattern.TrendStrength;

                if (cacheKenCandleStickPattern != null)
                    for (int idx = 0; idx < cacheKenCandleStickPattern.Length; idx++)
                        if (cacheKenCandleStickPattern[idx].Pattern == pattern && cacheKenCandleStickPattern[idx].TrendStrength == trendStrength && cacheKenCandleStickPattern[idx].EqualsInput(input))
                            return cacheKenCandleStickPattern[idx];

                KenCandleStickPattern indicator = new KenCandleStickPattern();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Pattern = pattern;
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
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Kp pattern, int trendStrength)
        {
            return _indicator.KenCandleStickPattern(Input, pattern, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, Kp pattern, int trendStrength)
        {
            return _indicator.KenCandleStickPattern(input, pattern, trendStrength);
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
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Kp pattern, int trendStrength)
        {
            return _indicator.KenCandleStickPattern(Input, pattern, trendStrength);
        }

        /// <summary>
        /// Ken Detects common candlestick patterns and marks them on the chart.
        /// </summary>
        /// <returns></returns>
        public Indicator.KenCandleStickPattern KenCandleStickPattern(Data.IDataSeries input, Kp pattern, int trendStrength)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.KenCandleStickPattern(input, pattern, trendStrength);
        }
    }
}
#endregion
