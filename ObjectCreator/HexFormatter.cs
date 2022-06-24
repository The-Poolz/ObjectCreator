using Nethereum.Util;
using System.Numerics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Text.Json;

namespace ObjectCreator
{
    public static class HexFormatter
    {
        public static readonly string HexStart = "0x";

        public static string? FormatHexToJson<T>(string hex, T obj) where T : class
        {
            if (string.IsNullOrWhiteSpace(hex))
                return null;

            hex = hex.Trim();
            if (hex.StartsWith(HexStart))
                hex = hex.Substring(HexStart.Length);

            Type type = obj.GetType();
            var prop = type.GetProperties();
            List<string> propsNames = new();
            foreach (var property in prop)
            {
                propsNames.Add(property.Name);
            }

            List<string> hexList = Split(hex);
            ExpandoObject expando = new ExpandoObject();
            for (int i = 0; i < hexList.Count; i++)
            {
                bool isAddress = false;
                bool isNumber = false;
                string addressResult = string.Empty;
                BigInteger numberResult = BigInteger.Zero;

                if (TryFormatAddress(hexList[i], out addressResult))
                    isAddress = true;
                else if (TryFormatInteger(hexList[i], out numberResult))
                    isNumber = true;
                else
                    return null;

                if (isAddress)
                    AddProperty(expando, propsNames[i], addressResult);
                else if (isNumber)
                    AddProperty(expando, propsNames[i], numberResult.ToString());
            }

            var result = JsonSerializer.Serialize(expando);

            return result;
        }

        private static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static bool TryFormatAddress(string hex, out string result)
        {
            result = string.Empty;
            if (!hex.StartsWith("0000000000000000000000000"))
            {
                result = "0x" + hex.Remove(0, 24);
                if (AddressUtil.Current.IsValidEthereumAddressHexFormat(result))
                    return true;
            }
            return false;
        }

        public static bool TryFormatInteger(string hex, out BigInteger result) =>
            BigInteger.TryParse(hex, NumberStyles.AllowHexSpecifier, null, out result);

        public static List<string> Split(string hex, int chunkSize = 64) =>
            (from Match m
             in Regex.Matches(hex, @".{1," + chunkSize + "}")
             select m.Value).ToList();
    }
}