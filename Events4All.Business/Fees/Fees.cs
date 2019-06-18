using System;
using System.Configuration;

namespace Events4All.Business.Fees
{
    class Fees
    {
        public int CalcCancellationFee(DateTime EventDay, int EventPrice)
        {
            int Fee = 0;
            if (EventPrice > 0)
            {
                if (EventDay.Day - DateTime.Now.Day <= 3)
                {
                    string StringFee = ConfigurationManager.AppSettings["CancellationFee"];
                    Fee = Convert.ToInt32(StringFee);
                    return Fee;
                }
                else
                {
                    Fee = 0;
                    return Fee;
                }
            }
            else
            {
                Fee = 0;
                return Fee;
            }
        }


        public int CalcParkingFee(int ParkingFee)
        {
            string StringFee = ConfigurationManager.AppSettings["ParkingFee"];
            int Fee = Convert.ToInt32(StringFee) + ParkingFee;
            return Fee;
        }

        public double CalcProcessingFee(int Price)
        {
            string PFP = ConfigurationManager.AppSettings["ProcessingFeePercent"];
            double ProcessFeePercent = Convert.ToInt32(PFP) /100;
            double Fee = Price * ProcessFeePercent;
            return Fee;
        }

    }
}
