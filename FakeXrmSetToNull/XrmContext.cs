using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace FakeXrmSetToNull
{
    public interface IXrmContext
    {
        IOrganizationService XrmService { get; }
        List<ExecuteMultipleResponse> ExecuteMultipleRequests(List<OrganizationRequest> requests);
    }

    public class XrmContext : IXrmContext
    {
        private const int ExecuteMultipleLimit = 200;
        private readonly Lazy<IOrganizationService> _lazyContext;


        public IOrganizationService XrmService => _lazyContext.Value;

        public List<ExecuteMultipleResponse> ExecuteMultipleRequests(List<OrganizationRequest> requests)
        {
            if (!requests.Any()) return null;

            var responses = new List<ExecuteMultipleResponse>();
            var executeMultipleRequest = new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            var counter = 0;
            foreach (var request in requests)
            {
                counter++;
                executeMultipleRequest.Requests.Add(request);
                if (executeMultipleRequest.Requests.Count != ExecuteMultipleLimit &&
                    counter != requests.Count) continue;

                responses.Add((ExecuteMultipleResponse) _lazyContext.Value.Execute(executeMultipleRequest));
                executeMultipleRequest.Requests.Clear();
            }

            return responses;
        }
    }
}