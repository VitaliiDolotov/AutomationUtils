using System.Linq;
using AutomationUtils.Extensions;
using AutomationUtils.Utils;
using NUnit.Framework;

namespace AutomationUtils.Tests
{
    [TestFixture]
    class StringExtensionsTests
    {
        [Test]
        public void Check_GetTextBetween_LastOccurrence()
        {
            var str = "some AA text BB some BB";
            var expectedString = " text BB some ";
            Verify.AreEqual(expectedString, str.GetTextBetween("AA", "BB").First(),
                "GetTextBetween method works incorrectly for Last Occurrence");
        }

        [Test]
        public void Check_GetTextBetween_FirstOccurrence()
        {
            var str = "some AA text BB some BB";
            var expectedString = " text ";
            Verify.AreEqual(expectedString, str.GetTextBetween("AA", "BB", false).First(),
                "GetTextBetween method works incorrectly for First Occurrence");
        }
    }
}