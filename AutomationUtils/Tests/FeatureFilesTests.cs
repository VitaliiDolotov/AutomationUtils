using AutomationUtils.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationUtils.Tests
{
    [TestFixture]
    public class FeatureFilesTests
    {
        [Test]
        public void Check_AllFeatureFilesAndTheirContent_CountOfFiles()
        {
            var featureFilesContent = TestsUtils.FeatureFilesAndTheirContent;
            Verify.AreEqual(2, featureFilesContent.Count,
                "Getting all feature files and their content method returns incorrect count of files");
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_FileNames()
        {
            var featureFilesContent = TestsUtils.FeatureFilesAndTheirContent;
            Verify.IsTrue(featureFilesContent.Keys.Any(x => x.Contains("TestFeatureFile1.feature")),
                "Getting all feature files and their content method returns incorrect files names");
            Verify.IsTrue(featureFilesContent.Keys.Any(x => x.Contains("TestFeatureFile2.feature")),
                "Getting all feature files and their content method returns incorrect files names");
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_ContentLength()
        {
            var featureFilesContent = TestsUtils.FeatureFilesAndTheirContent;
            foreach (var featureFile in featureFilesContent)
            {
                switch (featureFile)
                {
                    case KeyValuePair<string, List<string>> a when a.Key.Contains("TestFeatureFile1.feature"):
                        Verify.AreEqual(24, featureFile.Value.Count, $"Getting all feature files and their content method returns incorrect content length for {featureFile.Key} feature file");
                        break;
                    case KeyValuePair<string, List<string>> a when a.Key.Contains("TestFeatureFile2.feature"):
                        Verify.AreEqual(28, featureFile.Value.Count, $"Getting all feature files and their content method returns incorrect content length for {featureFile.Key} feature file");
                        break;
                    default:
                        throw new Exception($"Length of file is not defined for the following feature: {featureFile.Key}");
                }
            }
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_ScenariosContent()
        {
            var featureFilesContent = TestsUtils.FeatureFilesAndTheirContent;
            foreach (var featureFile in featureFilesContent)
            {
                Verify.IsTrue(featureFile.Value.Contains("Scenario: TestScenario"),
                    "Getting all feature files and their content method returns incorrect content, 'TestScenario' is not presenting");
                Verify.IsTrue(featureFile.Value.Contains("Scenario: SecondTestScenario"),
                    "Getting all feature files and their content method returns incorrect content, 'SecondTestScenario' is not presenting");
                Verify.IsTrue(featureFile.Value.Contains("Scenario Outline: ThirdTestScenario"),
                    "Getting all feature files and their content method returns incorrect content, 'ThirdTestScenario' is not presenting");
            }
        }

        [Test]
        public void Check_AllFeatureFilesAndTheirContent_Tags()
        {
            var featureFilesContent = TestsUtils.FeatureFilesAndTheirContent;
            foreach (var featureFile in featureFilesContent)
            {
                var tags = featureFile.Value.Where(x => x.Contains("@Regression @LinebreaksTest")).ToList();
                Verify.AreEqual(3, tags.Count,
                    "Getting all feature files and their content method return incorrect content (tags)");
            }
        }

        [Test]
        public void Check_TestsAndTags_CountOfTests()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            Verify.AreEqual(8, testsAndTags.Count,
                "Getting all feature files and their content method returns incorrect count of tests");
        }

        [Test]
        public void Check_TestsAndTags_CountOfTags()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            foreach (var test in testsAndTags)
            {
                Verify.AreEqual(2, test.Value.Count,
                    "Getting all feature files and their content method returns incorrect count of tags");
            }
        }

        [Test]
        public void Check_TestsAndTags_TestNames()
        {
            var testsAndTags = TestsUtils.TestsAndTags;
            Verify.AreEqual("TestScenario", testsAndTags[0].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("SecondTestScenario", testsAndTags[1].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario, TestExampleData1", testsAndTags[2].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario, TestExampleData2", testsAndTags[3].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("TestScenario", testsAndTags[4].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("SecondTestScenario", testsAndTags[5].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario, TestExampleData1", testsAndTags[6].Key,
                "Getting all feature files and their content method returns incorrect test name");
            Verify.AreEqual("ThirdTestScenario, TestExampleData2", testsAndTags[7].Key,
    "Getting all feature files and their content method returns incorrect test name");
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