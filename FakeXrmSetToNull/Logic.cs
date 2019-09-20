using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Xrm.CertX.Shared.Entities;

namespace FakeXrmSetToNull
{
    public interface ILogic
    {
        void Process();
    }

    public class Logic : ILogic
    {
        private readonly IXrmContext _xrmContext;

        public Logic(IXrmContext xrmContext)
        {
            _xrmContext = xrmContext;
        }

        public void Process()
        {
            var bonuses = GetBonuses();
            
            foreach (var bonus in bonuses)
            {
                bonus.statecode = null;
                //bonus.Attributes.Remove(cgk_bonus.AttributeLogicalNames.statecode);
            }
            var executeMultipleResponses = _xrmContext.ExecuteMultipleRequests(bonuses.Select(x => new CreateRequest { Target = x }).Cast<OrganizationRequest>().ToList());
        }

        private static List<cgk_bonus> GetBonuses()
        {
            return new List<cgk_bonus>
            {
                new cgk_bonus
                {
                    cgk_name = "Bonus1"
                },

                new cgk_bonus
                {
                    cgk_name = "Bonus2",
                    statuscode = new OptionSetValue(1),
                    statecode = cgk_bonusState.Active
                },
                new cgk_bonus
                {
                    cgk_name = "Bonus3",
                    statecode = cgk_bonusState.Active
                }
            };
        }
    }
}


