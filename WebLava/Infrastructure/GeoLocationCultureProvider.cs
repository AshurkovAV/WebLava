using Microsoft.AspNetCore.Localization;

namespace WebLava.Infrastructure
{
    public class GeoLocationCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // Определяем страну по IP (используем GeoIP2 или API)
            //var countryCode = await GetCountryFromIp(httpContext.Connection.RemoteIpAddress);

            //// Для Колумбии - испанский
            //if (countryCode == "CO")
            //    return new ProviderCultureResult("es");

            // По умолчанию - английский
            return new ProviderCultureResult("en");
        }

        //public async Task<string?> GetCountryFromIp(string? ip)
        //{
        //    if (string.IsNullOrEmpty(ip))
        //        return null;

        //    using var httpClient = new HttpClient();
        //    try
        //    {
        //        // Запрос к API (только countryCode)
        //        var response = await httpClient.GetFromJsonAsync<GeoIpResponse>(
        //            $"http://ip-api.com/json/{ip}?fields=countryCode");

        //        return response?.CountryCode;
        //    }
        //    catch
        //    {
        //        return null; // или fallback-культура
        //    }
        //}
    }
}
