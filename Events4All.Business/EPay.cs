using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.Business
{
    public class EPay
    {
        //Api request to Stripe Credit Card Processing Service to make a test transaction
        public void MakeStripeApiRequest(double pmtAmt, string pmtRcptEmail)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey("sk_test_4eC39HqLyjWDarjtT1zdp7dc");

            var options = new ChargeCreateOptions
            {
                Amount = (long)pmtAmt,
                Currency = "usd",
                Source = "tok_visa",
                ReceiptEmail = pmtRcptEmail,
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
        }
    }
}
