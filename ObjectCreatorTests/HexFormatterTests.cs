using Xunit;
using Serilog;
using System.Numerics;
using Xunit.Abstractions;

using ObjectCreator;
using ObjectCreatorTests.Models;

namespace ObjectCreatorTests
{
    public class HexFormatterTests
    {
        public HexFormatterTests(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
            // add the xunit test output sink to the serilog logger
            // https://github.com/trbenning/serilog-sinks-xunit#serilog-sinks-xunit
            .WriteTo.TestOutput(output)
            .CreateLogger();
        }

        [Fact]
        public static void FormatHexToJson()
        {
            string hex = "0x000000000000000000000000000000000000000000000000000000000000442200000000000000000000000077018282fd033daf370337a5367e62d8811bc8850000000000000000000000000000000000000000000000000000000061586f9e00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300";
            NewPoolCreated pool = new();
            string expected = "{\"PoolId\":\"17442\",\"Token\":\"0x77018282fd033daf370337a5367e62d8811bc885\",\"FinishTime\":\"1633185694\",\"StartAmount\":\"7592187844964004334566\",\"Owner\":\"0xa81236c2afe21c0165349b267d5754b6ddcd8300\"}";

            var result = HexFormatter.FormatHexToJson<NewPoolCreated>(hex, pool);

            Assert.Equal(expected, result);

            hex = "0x00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd830000000000000000000000000077018282fd033daf370337a5367e62d8811bc885";
            TransferIn transferIn = new();
            expected = "{\"Amount\":\"7592187844964004334566\",\"From\":\"0xa81236c2afe21c0165349b267d5754b6ddcd8300\",\"Token\":\"0x77018282fd033daf370337a5367e62d8811bc885\"}";

            result = HexFormatter.FormatHexToJson<TransferIn>(hex, transferIn);

            Assert.Equal(expected, result);

            hex = "0x0000000000000000000000000000000000000000000000000000000000000000";
            Approval approval = new();
            expected = "{\"Value\":\"0\"}";

            result = HexFormatter.FormatHexToJson<Approval>(hex, approval);

            Assert.Equal(expected, result);
        }

        [Fact]
        public static void Split()
        {
            string hex = "000000000000000000000000000000000000000000000000000000000000442200000000000000000000000077018282fd033daf370337a5367e62d8811bc8850000000000000000000000000000000000000000000000000000000061586f9e00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300";

            var expected = new List<string>
            {
                "0000000000000000000000000000000000000000000000000000000000004422", // PoolId : 17442
                "00000000000000000000000077018282fd033daf370337a5367e62d8811bc885", // Token : 0x77018282fd033daf370337a5367e62d8811bc885
                "0000000000000000000000000000000000000000000000000000000061586f9e", // FinishTime : 1633185694
                "00000000000000000000000000000000000000000000019b92c59bdc0df30be6", // StartAmount : 7592187844964004334566
                "000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300"  // Owner : 0xa81236c2afe21c0165349b267d5754b6ddcd8300
            };

            var result = HexFormatter.Split(hex);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected, result);

            hex = "00000000000000000000000000000000000000000000019b92c59bdc0df30be6000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd830000000000000000000000000077018282fd033daf370337a5367e62d8811bc885";

            expected = new List<string>
            {
                "00000000000000000000000000000000000000000000019b92c59bdc0df30be6", // Amount : 7592187844964004334566
                "000000000000000000000000a81236c2afe21c0165349b267d5754b6ddcd8300", // From : 0xa81236c2afe21c0165349b267d5754b6ddcd8300
                "00000000000000000000000077018282fd033daf370337a5367e62d8811bc885"  // Token : 0x77018282fd033daf370337a5367e62d8811bc885
            };

            result = HexFormatter.Split(hex);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected, result);

            hex = "0000000000000000000000000000000000000000000000000000000000000000";

            expected = new List<string>
            {
                "0000000000000000000000000000000000000000000000000000000000000000", // Value : 0
            };

            result = HexFormatter.Split(hex);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public static void TryFormatInteger()
        {
            string hexInt = "0000000000000000000000000000000000000000000000000000000000004422";

            HexFormatter.TryFormatInteger(hexInt, out BigInteger result);

            Assert.Equal("17442", result.ToString());
        }

        [Fact]
        public static void TryFormatAddress()
        {
            string hexAddress = "00000000000000000000000077018282fd033daf370337a5367e62d8811bc885";

            HexFormatter.TryFormatAddress(hexAddress, out string result);

            Assert.Equal("0x77018282fd033daf370337a5367e62d8811bc885", result);
        }
    }
}