using System;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Payments;
using Money = PayPalCheckoutSdk.Payments.Money;
using Refund = PayPalCheckoutSdk.Payments.Refund;


namespace paypal
{
    public class demo
    {
        static String clientId = "AcrQWc6nEGpw2PvnMujSOPmcjvfeXTS9gV2bd-fVHBbab9cnYKZmSeRPtW8WEYU1qGKU5OK-BFF94ARk";
        static String secret = "EKKbA07eQ18SGxVCrhyEB0Ky-tu3kvYZdXXEUAKFPFMcG48Ex3HtBf6k6jZzqBEUvMdCIGJSLmDgar-2";

        public static HttpClient client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }
        /// <summary>
        /// AUTHORIZE,CAPTURE
        /// </summary>
        /// <returns></returns>
        public async static Task<HttpResponse> createOrder()
        {
            HttpResponse response;
            // Construct a request object and set desired parameters
            // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                         AmountWithBreakdown = new AmountWithBreakdown()
                         {
                            CurrencyCode = "USD",
                            Value = "100.00"
                         },
                         Items = new List<Item>(){

 
                         }
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = "https://www.example.com",
                    CancelUrl = "https://www.example.com"
                }
            };


            // Call API with your client and get a response for your call
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(order);
            response = await client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Order Id: {0}", result.Id);
            Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
            Console.WriteLine("Links:");
            foreach (PayPalCheckoutSdk.Orders.LinkDescription link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            return response;
        }


        public async static Task<HttpResponse> captureOrder()
        {

            // Construct a request object and set desired parameters
            // Replace ORDER-ID with the approved order id from create order
            var request = new OrdersCaptureRequest("APPROVED-ORDER-ID");
            request.RequestBody(new OrderActionRequest());
            HttpResponse response = await client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Capture Id: {0}", result.Id);
            return response;
        }

        /// <summary>
        /// 付款id
        /// </summary>
        /// <param name="CaptureId"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public async static Task<HttpResponse> CapturesRefund(string CaptureId, bool debug = true)
        {
            var request = new CapturesRefundRequest(CaptureId);
            request.Prefer("return=representation");
            RefundRequest refundRequest = new RefundRequest()
            {
                Amount = new Money
                {
                    Value = "20.00",
                    CurrencyCode = "USD"
                }
            };
            request.RequestBody(refundRequest);
            var response = await client().Execute(request);

            if (debug)
            {
                var result = response.Result<Refund>();
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("Refund Id: {0}", result.Id);
                Console.WriteLine("Links:");
                foreach (PayPalCheckoutSdk.Payments.LinkDescription link in result.Links)
                {
                    Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
                }
                Console.WriteLine("Response JSON: \n {0}", System.Text.Json.JsonSerializer.Serialize(result));
            }
            return response;
        }


        //This function can be used to perform authorization on the approved order.
        public async static Task<HttpResponse> AuthorizeOrder(string OrderId, bool debug = true)
        {
            var request = new OrdersAuthorizeRequest(OrderId);
            request.Prefer("return=representation");
            request.RequestBody(new AuthorizeRequest());
            var response = await client().Execute(request);

            if (debug)
            {
                var result = response.Result<Order>();
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("Order Id: {0}", result.Id);
                Console.WriteLine("Authorization Id: {0}", result.PurchaseUnits[0].Payments.Authorizations[0].Id);
                Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
                Console.WriteLine("Links:");
                foreach (PayPalCheckoutSdk.Orders.LinkDescription link in result.Links)
                {
                    Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
                }
                AmountWithBreakdown amount = result.PurchaseUnits[0].AmountWithBreakdown;
                Console.WriteLine("Buyer:");
                Console.WriteLine("\tEmail Address: {0}", result.Payer.Email);
                Console.WriteLine("Response JSON: \n {0}", System.Text.Json.JsonSerializer.Serialize(result));
            }

            return response;
        }

    }
}
