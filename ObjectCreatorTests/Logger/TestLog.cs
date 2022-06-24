using System;
using Serilog;

namespace ObjectCreatorTests.Logger
{
    public class TestLog
    {
        private readonly DateTime StartTime;

        public TestLog() { StartTime = DateTime.UtcNow; }

        public void StartTest()
        {
            Log.Information("Test has been run...");
        }

        public void EndTest()
        {
            Log.Information("Test completed.");
            Log.Information($"Test execution time {DateTime.UtcNow - StartTime}.");
        }

        public void WriteResult(string expected, string result)
        {
            if (result.Equals(expected))
            {
                Log.Information($"Expected value: {expected}");
                Log.Information($"Result value: {result}");
            }
            else
            {
                Log.Fatal($"Expected value: {expected}");
                Log.Fatal($"Result value: {result}");
            }
        }
    }
}