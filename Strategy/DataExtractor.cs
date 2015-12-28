using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Dapper;

// This namespace holds all strategies and is required. Do not change it.

namespace NinjaTrader.Custom.Strategy
{
    /// <summary>
    /// Get Some Data
    /// </summary>
    [Description("Get Some Data")]
    public class DataExtractor : NinjaTrader.Strategy.Strategy
    {
		public class DataItem {
			public string Ver {get; set;}
			public double High {get; set;}
			public double Low {get; set;}
			public double Open {get; set;}
			public double Close {get; set;}
			public double Volume {get; set;}
			public DateTime DtTm {get;set;}
			public DateTime Dt {get;set;}
			public int Hr {get; set;}
			public double VolumeSMA3 {get; set;}
				
		}
			
			
		
        
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
		
		protected IDbConnection  DbConn;
       

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            CalculateOnBarClose = false;
			DbConn = OpenConnection();
        }
		
		protected IDbConnection OpenConnection(){
			try {
				
				IDbConnection conn = new SqlConnection("server=THEBEAST\\SQLEXPRESS;trusted_connection=yes;database=ninjaData;connection timeout=10;");
				conn.Open();
				return conn;
				
			}catch(System.Exception ex){
				Print("DB Open Error" + ex.Message);
				return null;
			}
		}
			

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
          

			var myRow = new DataItem{
				High = High[0],
				Low = Low[0],
				Open = Open[0],
				Close = Close[0],
				Volume = Volume[0],
				DtTm = Time[0],
				Dt = Time[0].Date,
				Hr = Time[0].Hour,
				VolumeSMA3 = SMA(Volume, 3)[0]
			};
			
			try{
			DbConn.Execute("insert into dbo.extractedData([High], [Low], [Open], [Close], [Volume], Dt, DtTm, Hr,  VolumeSMA3) select @High, @Low, @Open, @Close, @Volume, @Dt, @DtTm, @Hr, @VolumeSMA3", myRow);
			}catch (Exception ex){
				Print("Fail " + ex.Message);
			}
        }
	}
}
