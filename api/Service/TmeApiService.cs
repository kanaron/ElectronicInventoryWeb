using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Domain.TmeJResults;
using Domain.TmeModels;

namespace API.Service;

public class TmeApiService
{
    private readonly IConfiguration _configuration;

    public TmeApiService(IConfiguration config)
    {
        _configuration = config;
    }

    public async Task<ProductWithParameters?> GetProductWithParametersAsync(string token, string symbol)
    {
        var queryParams = new Dictionary<string, string>
    {
        { "Country", "PL" },
        { "Language", "en" },
        { "SymbolList[0]", symbol },
        { "Token", token }
    };

        var response = await ApiCall("Products/GetParameters", queryParams);

        ErrorResult error;

        if (!IsStatusOK(response, out error))
        {
            return null; //TODO add error handling
        }

        var parameterResult = JsonConvert.DeserializeObject<GetParametersJResult>(response);

        return new ProductWithParameters
        {
            Symbol = parameterResult.Data.ProductList.First().Symbol,
            ParameterList = parameterResult.Data.ProductList.First().ParameterList
                .Select(p => new Parameters
                {
                    ParameterId = p.ParameterId,
                    ParameterName = p.ParameterName,
                    ParameterValueId = p.ParameterValueId,
                    ParameterValue = p.ParameterValue
                }).ToList()
        };
    }

    public async Task<ProductWithDescription?> GetProductWithDescriptionAsync(string token, string symbol)
    {
        var queryParams = new Dictionary<string, string>
    {
        { "Country", "PL" },
        { "Language", "en" },
        { "SymbolList[0]", symbol },
        { "Token", token }
    };

        var response = await ApiCall("Products/GetProducts", queryParams);

        ErrorResult error;

        if (!IsStatusOK(response, out error))
        {
            return null; //TODO add error handling
        }

        GetDescriptionJResult product = JsonConvert.DeserializeObject<GetDescriptionJResult>(response);

        return new ProductWithDescription
        {
            Symbol = product.Data.ProductList.First().Symbol,
            CustomerSymbol = product.Data.ProductList.First().CustomerSymbol,
            OriginalSymbol = product.Data.ProductList.First().OriginalSymbol,
            EAN = product.Data.ProductList.First().EAN,
            Producer = product.Data.ProductList.First().Producer,
            Description = product.Data.ProductList.First().Description,
            CategoryId = product.Data.ProductList.First().CategoryId,
            Category = product.Data.ProductList.First().Category,
            Photo = product.Data.ProductList.First().Photo,
            Thumbnail = product.Data.ProductList.First().Thumbnail,
            Weight = product.Data.ProductList.First().Weight,
            WeightUnit = product.Data.ProductList.First().WeightUnit,
            SuppliedAmount = product.Data.ProductList.First().SuppliedAmount,
            MinAmount = product.Data.ProductList.First().MinAmount,
            Multiples = product.Data.ProductList.First().Multiples,
            Packing = product.Data.ProductList.First().Packing,
            ProductStatusList = product.Data.ProductList.First().ProductStatusList,
            Unit = product.Data.ProductList.First().Unit,
            ProductInformationPage = product.Data.ProductList.First().ProductInformationPage,
            Guarantee = product.Data.ProductList.First().Guarantee,
            OfferId = product.Data.ProductList.First().OfferId,
            Certificates = product.Data.ProductList.First().Certificates
        };
    }

    private async Task<string> ApiCall(string action, Dictionary<string, string> apiParams)
    {
        // All code in this function is based on documentation https://developers.tme.eu/en/

        string uri = $@"https://api.tme.eu/{action}.json";

        // Encode and normalize params
        FormUrlEncodedContent urlEncodedContent = new(apiParams);
        string encodedParams = await urlEncodedContent.ReadAsStringAsync();

        // Calculate signature basis according the documentation
        string escapedUri = UrlEncode(uri);
        string escapedParams = UrlEncode(encodedParams);
        string signatureBase = $"POST&{escapedUri}&{escapedParams}";

        // Calculate HMAC-SHA1 from signature and encode by Base64 function
        byte[] hmacSha1 = HashHmac(signatureBase, _configuration["TME:AppSecret"]!);
        string apiSignature = Convert.ToBase64String(hmacSha1);

        // Add ApiSignature to params
        apiParams.Add("ApiSignature", apiSignature);
        // Send POST message and return .json content as result
        var result = await SendMessage(uri, new FormUrlEncodedContent(apiParams));

        return result;
    }

    public bool IsStatusOK(string jsonContent, out ErrorResult errorResult)
    {
        JObject json = JObject.Parse(jsonContent);
        string status = json["Status"].ToString();
        if (status == "OK")
        {
            errorResult = null;
            return true;
        }
        else
        {
            errorResult = new ErrorResult(status, json["Error"].ToString());
            return false;
        }
    }

    private static async Task<string> SendMessage(string uri, FormUrlEncodedContent content)
    {
        HttpClient client = new();

        // API service is available only via the TLSv1.2 protocol. This information can be found on https://developers.tme.eu/en/signin 
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            HttpResponseMessage response = await client.PostAsync(uri, content);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ex.Message;
        }
    }

    private static byte[] HashHmac(string input, string key)
    {
        HMACSHA1 hmac = new(Encoding.ASCII.GetBytes(key));
        byte[] byteArray = Encoding.ASCII.GetBytes(input);
        return hmac.ComputeHash(byteArray);
    }

    private static string UrlEncode(string s)
    {
        // This function uses uppercase in escaped chars to be compatible with the documentation

        // Input: https://api.tme.eu/Products/GetParameters.json
        // https%3a%2f%2fapi.tme.eu%2fProducts%2fGetParameters.json - HttpUtility.UrlEncode
        // https%3A%2F%2Fapi.tme.eu%2FProducts%2FGetParameters.json - result
        // %3a => %3A ...

        char[] temp = System.Web.HttpUtility.UrlEncode(s).ToCharArray();
        for (int i = 0; i < temp.Length - 2; i++)
        {
            if (temp[i] == '%')
            {
                temp[i + 1] = char.ToUpper(temp[i + 1]);
                temp[i + 2] = char.ToUpper(temp[i + 2]);
            }
        }
        return new string(temp);
    }
}
