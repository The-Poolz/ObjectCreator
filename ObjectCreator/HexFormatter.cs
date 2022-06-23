using System.Text.RegularExpressions;

namespace ObjectCreator
{
    public static class HexFormatter
    {
        public static T FormatHexToJson<T>(string hex) where T : class
        {
            throw new NotImplementedException();
        }
        public static List<string> Split(string hex, int chunkSize = 64) =>
            (from Match m
             in Regex.Matches(hex, @".{1," + chunkSize + "}")
             select m.Value).ToList();
    }
}
