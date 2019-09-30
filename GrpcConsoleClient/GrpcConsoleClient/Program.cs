using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var input = new HelloRequest { Name = "Kirill" };
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(input);

            //Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customer.CustomerClient(channel);

            var clientRequester = new CustomerLookupModel { UserId = 2 };
            
            var customer = await customerClient.GetCustomerInfoAsync(clientRequester);

            Console.WriteLine($"Customer: {customer.FirstName} - {customer.LastName}");
            Console.WriteLine("==========new customer==============");

            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var currenCustomer = call.ResponseStream.Current;

                    Console.WriteLine($"Customer: {currenCustomer.FirstName} - {currenCustomer.LastName}\n" +
                        $" Email: {currenCustomer.Email} Age: {currenCustomer.Age} Active:{currenCustomer.IsActive}");
                }
            }

            Console.ReadLine();
        }
    }
}
