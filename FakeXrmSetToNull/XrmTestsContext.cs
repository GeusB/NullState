using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace FakeXrmSetToNull
{
    public class XrmTestsContext : IXrmContext
    {              
        public IOrganizationService XrmService { get; set; }
        
        public List<ExecuteMultipleResponse> ExecuteMultipleRequests(List<OrganizationRequest> requests)
        {
            if (!requests.Any()) return null;

            var executeMultipleRequest = new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            executeMultipleRequest.Requests.AddRange(requests);

            return new List<ExecuteMultipleResponse> {(ExecuteMultipleResponse) XrmService.Execute(executeMultipleRequest)};
        }
    }
}
