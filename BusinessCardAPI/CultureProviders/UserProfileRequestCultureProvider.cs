using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Primitives;

namespace BusinessCardAPI.CultureProviders
{
    public class UserProfileRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {

            var supportedLanguage = new Dictionary<string, string>()
            {
                {"ar", "ar-JO" },
                {"en", "en-US" },
            };


            StringValues value;
            if (httpContext.Request.Headers.TryGetValue("Language", out value))
            {
                if (value.Count == 0)
                {
                    return Task.FromResult(new ProviderCultureResult("en-US"));
                }
                if (supportedLanguage.ContainsKey(value[0]))
                {
                    return Task.FromResult(new ProviderCultureResult(supportedLanguage[value[0]]));
                }

                return Task.FromResult(new ProviderCultureResult("en-US"));
            }
            else
            {

                return Task.FromResult(new ProviderCultureResult("en-US"));
            }
        }
    }
}
