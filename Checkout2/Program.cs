using System;
using TwoCheckout;

namespace Checkout2
{
    class Program
    {
        static void Main(string[] args)
        {
            TwoCheckoutConfig.SellerID = "901421388";
            TwoCheckoutConfig.PrivateKey = "53424946-7352-4966-9245-38A7A1BF4ED9";

            //var ServiceObject = new SaleService();
            //ServiceObject.List();
            //TwoCheckoutConfig.SellerID = "901288242";
            //TwoCheckoutConfig.PrivateKey = "80D02EA2-4847-4AC0-9E11-77B50CDFBB97";
            TwoCheckoutConfig.Sandbox = true;   //<-- Set Mode to use your 2Checkout sandbox account

            try
            {
                var Billing = new AuthBillingAddress();
                Billing.addrLine1 = "123 Main Street";
                Billing.city = "Townsville";
                Billing.zipCode = "43206";
                Billing.state = "Ohio";
                Billing.country = "USA";
                Billing.name = "Joe Flagster";
                Billing.email = "example@2co.com";
                Billing.phoneNumber = "5555555555";

                var Customer = new ChargeAuthorizeServiceOptions();
                Customer.total = (decimal)1.00;
                Customer.currency = "USD";
                Customer.merchantOrderId = "123";
                Customer.billingAddr = Billing;
                Customer.token = "YjFkYjYxZWItNjQ1MS00NWQ0LTg4NDEtMDk0MmYxYmJlN2Vh";
                //Customer.lineItems = new System.Collections.Generic.List<AuthLineitem>();
                //Customer.lineItems.Add(new AuthLineitem
                //{
                //    price = 2,
                //    name = "tetsPro",
                //    productId = "123456",
                //    type = "product", // TODO: this should be passed in from outside. Possible values: ‘product’, ‘shipping’, ‘tax’ or ‘coupon’
                //    quantity = 1      // TODO: this should be passed in from outside. Possible values: 1-999                    
                //});
                var Charge = new ChargeService();

                String token = "YjFkYjYxZWItNjQ1MS00NWQ0LTg4NDEtMDk0MmYxYmJlN2Vh";

                var Shipping = new AuthShippingAddress();
                Shipping.addrLine1 = "123 test st";
                Shipping.city = "Columbus";
                Shipping.state = "OH";
                Shipping.country = "USA";
                Shipping.name = "Testing Tester";
                var Sale = new ChargeAuthorizeServiceOptions();
                Sale.lineItems = Customer.lineItems;
                Sale.currency = "USD";
                Sale.merchantOrderId = "123";
                Sale.billingAddr = Billing;
                Sale.shippingAddr = Shipping;
                Sale.token = token;
                Sale.returnUrl = "http://www.2checkout.com/documentation";
                var result = Charge.Authorize(Customer);
                Console.Write(result);
            }
            catch (TwoCheckoutException e)
            {
                Console.Write(e);
            }

            TwoCheckoutConfig.ApiUsername = "APIuser1817037";
            TwoCheckoutConfig.ApiPassword = "APIpass1817037";

            try
            {
                //var ServiceObject = new SaleService();
                //ServiceObject.List();
                //var ArgsObject = new SaleRefundServiceOptions();
                //ArgsObject.invoice_id = invoice_id;
                //ArgsObject.comment = "test refund";
                //ArgsObject.category = 5;
                //var result = ServiceObject.Refund(ArgsObject);
            }
            catch (TwoCheckoutException e)
            {
                // read e.message
            }
            Console.WriteLine("Hello World!");
        }
    }
}
