using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationUtils.Utils;
using NUnit.Framework;

namespace AutomationUtils.Tests
{
    [TestFixture]
    public class FeatureFilesTests
    {
        [Test]
        public void Check_AllFeatureFilesAndTheirContent_CountOfFiles()
        {
            var featureFilesContent = TestsUtils.AllFeatureFilesAndTheirContent();
            Verify.AreEqual(2, featureFilesContent.Count,
                "AllFeatureFilesAndTheirContent method returns incorrect count of files");
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_FileNames()
        {
            var featureFilesContent = TestsUtils.AllFeatureFilesAndTheirContent();
            Verify.AreEqual("TestFeatureFile1.feature", Path.GetFileName(featureFilesContent.ElementAt(0).Key),
                "AllFeatureFilesAndTheirContent method returns incorrect files names");
            Verify.AreEqual("TestFeatureFile2.feature", Path.GetFileName(featureFilesContent.ElementAt(1).Key),
                "AllFeatureFilesAndTheirContent method returns incorrect files names");
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_ContentLength()
        {
            var featureFilesContent = TestsUtils.AllFeatureFilesAndTheirContent();
            Verify.AreEqual(21, featureFilesContent.ElementAt(0).Value.Count,
                "AllFeatureFilesAndTheirContent method returns incorrect content length for 'TestFeatureFile1.feature' file");
            Verify.AreEqual(21, featureFilesContent.ElementAt(1).Value.Count,
                "AllFeatureFilesAndTheirContent method returns incorrect content length for 'TestFeatureFile2.feature' file");
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_ScenariosContent()
        {
            var featureFilesContent = TestsUtils.AllFeatureFilesAndTheirContent();
            foreach (var featureFile in featureFilesContent)
            {
                Verify.IsTrue(featureFile.Value.Contains("Scenario: TestScenario"), "AllFeatureFilesAndTheirContent method returns incorrect content, 'TestScenario' is not presenting");
                Verify.IsTrue(featureFile.Value.Contains("Scenario: SecondTestScenario"), "AllFeatureFilesAndTheirContent method returns incorrect content, 'SecondTestScenario' is not presenting");
                Verify.IsTrue(featureFile.Value.Contains("Scenario: ThirdTestScenario"), "AllFeatureFilesAndTheirContent method returns incorrect content, 'ThirdTestScenario' is not presenting");
            }
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_Tags()
        {
            var featureFilesContent = TestsUtils.AllFeatureFilesAndTheirContent();
            foreach (var featureFile in featureFilesContent)
            {
                var tags = featureFile.Value.Where(x => x.Contains("@Regression @LinebreaksTest")).ToList();
                Verify.AreEqual(3, tags.Count, "AllFeatureFilesAndTheirContent method return incorrect content (tags)");
            }
        }

        [Test]
        public void Check_TestsAndTags_CountOfTests()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            Verify.AreEqual(6, testsAndTags.Count, "GetTestsNamesAndTags method returns incorrect count of tests");
        }

        [Test]
        public void Check_TestsAndTags_CountOfTags()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            foreach (var test in testsAndTags)
            {
                Verify.AreEqual(2, test.Value.Count, "GetTestsNamesAndTags method returns incorrect count of tags");
            }
        }

        [Test]
        public void Check_TestsAndTags_TestNames()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            Verify.AreEqual("TestScenario", testsAndTags[0].Key, "GetTestsNamesAndTags method returns incorrect test name");
            Verify.AreEqual("SecondTestScenario", testsAndTags[1].Key, "GetTestsNamesAndTags method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario", testsAndTags[2].Key, "GetTestsNamesAndTags method returns incorrect test name");
            Verify.AreEqual("TestScenario", testsAndTags[3].Key, "GetTestsNamesAndTags method returns incorrect test name");
            Verify.AreEqual("SecondTestScenario", testsAndTags[4].Key, "GetTestsNamesAndTags method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario", testsAndTags[5].Key, "GetTestsNamesAndTags method returns incorrect test name");
        }

        [Test]
        public void Check_TestsAndTags_Tags()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            var expectedTags = new List<string>() { "Regression", "LinebreaksTest" };
            foreach (var test in testsAndTags)
            {
                Verify.AreEqual(expectedTags, test.Value, "GetTestsNamesAndTags method returns incorrect tags");
            }
        }
    }
}