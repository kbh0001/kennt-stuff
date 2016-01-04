namespace KenNinja
{
    public enum Kp
    {
        All = 0,

        //Uncategorized
        Doji = 1,


        //Da bears reversals (require uptrend)
        BearishBeltHold = -101,
        BearishEngulfing = -102,
        BearishHarami = -103,
        BearishHaramiCross = -104,
        BearishDarkCloudCover = -105,
        BearishEveningStar = -106,
        BearishShootingStar = -107,
        BearishHangingMan = -108,
        BearishThreeBlackCrows = -109,
        BearishUpsideGapTwoCrows = -110,
        BearishStickSandwich = -111,
        BearishDoji = -112,

        //Da bears that need confirmation


        //Bearish Continuation - requires down trend 
        BearishDownsideTasukiGap = -201,
        BearishFallingThreeMethods = -202,


        //Da Bulls Reversals (require downtrend)
        BullishBeltHold = 101,
        BullishEngulfing = 102,
        BullishHarami = 103,
        BullishHaramiCross = 104,
        BullishDoji = 105,
        BullishHammer = 106,
        BullishThreeWhiteSoldiers = 107,
        BullishMorningStar = 108,
        BullishPiercingLine = 109,
        BullishStickSandwich = 110,
        BullishBottomBounce = 112,

        //Da bull that need confirmation and downtrend
        BullishInvertedHammer = 111,

        //BullishContiuation - requires uptrend
        BullishUpsideTasukiGap = 201,
        BullishRishingThreeMethods = 202
    }
}