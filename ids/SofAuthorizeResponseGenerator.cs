using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ids
{
    public class SofAuthorizeResponseGenerator : AuthorizeResponseGenerator
    {
        private readonly IContextStore ContextStore;

        public SofAuthorizeResponseGenerator(ISystemClock clock, ITokenService tokenService, IKeyMaterialService keyMaterialService, IAuthorizationCodeStore authorizationCodeStore, ILogger<AuthorizeResponseGenerator> logger, IEventService events, IContextStore contextStore) : base(clock, tokenService, keyMaterialService, authorizationCodeStore, logger, events)
        {
            ContextStore = contextStore;
        }

        protected async override Task<AuthorizeResponse> CreateCodeFlowResponseAsync(ValidatedAuthorizeRequest request)
        {
            string context = null;

            var state = request.Raw["state"];
            if (state != null)
            {
                context = ContextStore.GetContext(request.Raw["state"]);
            }

            var response = await base.CreateCodeFlowResponseAsync(request);
            
            if (context != null)
            {
                ContextStore.AddContext(response.Code, context);
            }

            return response;
        }
    }
}
