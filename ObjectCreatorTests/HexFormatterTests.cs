using ObjectCreator;

namespace ObjectCreatorTests
{
    public static class HexFormatterTests
    {
        [Fact]
        public static void Split()
        {
            string hex = "0000000000000000000000000000000000000000000000000487fea90b4a5800000000000000000000000000994d202ce5643151b4829434fb75635ba2586019";

            List<string> expected = new List<string>
            {
                "0000000000000000000000000000000000000000000000000487fea90b4a5800",
                "000000000000000000000000994d202ce5643151b4829434fb75635ba2586019"
            };

            var result = HexFormatter.Split(hex);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected, result);
        }
    }
}