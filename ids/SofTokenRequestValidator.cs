using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ids
{
    internal class SofTokenRequestValidator : ICustomTokenRequestValidator
    {
        private readonly IContextStore ContextStore;

        public SofTokenRequestValidator(IContextStore contextStore)
        {
            ContextStore = contextStore;
        }

        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var code = context.Result.ValidatedRequest.Raw["code"];
            var launchContext = ContextStore.GetContext(code);

            var launch = launchContext.Split(":");

            context.Result.CustomResponse = new Dictionary<string, object>
            {
                {"patient", launch[0]},
                {"encounter", launch[1]},
                {"resource", launch[2]}
            };

            return Task.CompletedTask;
        }
    }
}