using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();
            if(request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Kirill";
                output.LastName = "Plamin";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Marie";
                output.LastName = "Milits";
            }
            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(
            NewCustomerRequest request,
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Alek",
                    LastName = "Bolduin",
                    Email = "alekbol@mail.ru",
                    Age = new Random().Next(10,90),
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName = "Marie",
                    LastName = "Milits",
                    Email = "Mary@mail.ru",
                    Age = new Random().Next(10,90),
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName="Kirill",
                    LastName = "Pavlov",
                    Email = "kir@gmail.com",
                    Age=new Random().Next(10,90),
                    IsActive=true
                }
            };
            foreach(var cust in customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(cust);
            }
        }
    }
}
