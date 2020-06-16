using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APIBase.Common.FileFunctions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace APIBase.Common.RsaSecurityKeyReader
{
    public static class RsaSecurityKeyReader
    {
        public static async Task<RsaSecurityKey> GetSignInKeyAsync(string pathTempKey)
        {
            var tempKeyAsJson = await FileReader.ReadFileAsStringAsync(pathTempKey);

            var rsaJson = JsonConvert.DeserializeObject<RsaJson>(tempKeyAsJson);

            var parametersRsa = new RSAParameters()
            {
                D = Convert.FromBase64String(rsaJson.D),
                DP = Convert.FromBase64String(rsaJson.DP),
                DQ = Convert.FromBase64String(rsaJson.DQ),
                Exponent = Convert.FromBase64String(rsaJson.Exponent),
                InverseQ = Convert.FromBase64String(rsaJson.InverseQ),
                Modulus = Convert.FromBase64String(rsaJson.Modulus),
                P = Convert.FromBase64String(rsaJson.P),
                Q = Convert.FromBase64String(rsaJson.Q)
            };

            var publicAndPrivateKey = RSA.Create();
            
            publicAndPrivateKey.ImportParameters(parametersRsa);

            var response = new RsaSecurityKey(publicAndPrivateKey);

            return response;
        }

        public static RsaSecurityKey GetSignInKey(string pathTempKey)
        {
            var tempKeyAsJson = FileReader.ReadFileAsString(pathTempKey);

            var rsaJson = JsonConvert.DeserializeObject<RsaJson>(tempKeyAsJson);

            var parametersRsa = new RSAParameters()
            {
                D = Convert.FromBase64String(rsaJson.D),
                DP = Convert.FromBase64String(rsaJson.DP),
                DQ = Convert.FromBase64String(rsaJson.DQ),
                Exponent = Convert.FromBase64String(rsaJson.Exponent),
                InverseQ = Convert.FromBase64String(rsaJson.InverseQ),
                Modulus = Convert.FromBase64String(rsaJson.Modulus),
                P = Convert.FromBase64String(rsaJson.P),
                Q = Convert.FromBase64String(rsaJson.Q)
            };

            var publicAndPrivateKey = RSA.Create();

            publicAndPrivateKey.ImportParameters(parametersRsa);

            var response = new RsaSecurityKey(publicAndPrivateKey);

            return response;
        }
    }
}
