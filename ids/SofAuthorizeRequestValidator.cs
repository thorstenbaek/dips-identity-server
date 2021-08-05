using IdentityServer4.Validation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ids
{
    internal class SofCustomAuthorizeRequestValidator : ICustomAuthorizeRequestValidator
    {
        private readonly IContextStore Store;
        public SofCustomAuthorizeRequestValidator(IContextStore store)
        {
            Store = store;
        }

        public Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
        {
            if (!context.Result.IsError)
            {
                var state = context.Result.ValidatedRequest.Raw["state"];
                if (string.IsNullOrEmpty(state))
                {
                    context.Result.IsError = true;
                    context.Result.Error = "Missing state parameter";
                }
                var launchParameter = context.Result.ValidatedRequest.Raw["launch"];
                if (string.IsNullOrEmpty(launchParameter))
                {
                    context.Result.IsError = true;
                    context.Result.Error = "Missing launch parameter";
                }
                else
                {
                    Store.AddContext(state, launchParameter);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}