using System.Linq;
using AutomationUtils.Utils;
using NUnit.Framework;

namespace AutomationUtils.Tests
{
    [TestFixture]
    public class FeatureFilesTests
    {
        [Test]
        public void Check_GetAllFeatureFilesContent_FeatureFirstLine()
        {
            var featureFilesContent = TestsUtils.GetAllFeatureFilesAndItContent();
            foreach (var featureFile in featureFilesContent)
            {
                Verify.IsTrue(featureFile.Value.First().Contains("Feature:"), $"'{featureFile.Key}' feature file has incorrect first line");
            }
        }

        [Test]
        public void Check_GetAllFeatureFilesContent_FeatureLastLine()
        {
            var featureFilesContent = TestsUtils.GetAllFeatureFilesAndItContent();
            foreach (var featureFile in featureFilesContent)
            {
                Verify.IsFalse(featureFile.Value.Last().Equals(string.Empty), $"'{featureFile.Key}' feature file has incorrect last line");
            }
        }
    }
}