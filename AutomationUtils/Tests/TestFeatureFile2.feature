

Feature: TestFeatureFile2

@Regression @LinebreaksTest
Scenario: TestScenario
	Given User is login
	When User do something
	Then User sees result

@Regression @LinebreaksTest
Scenario: SecondTestScenario
	Given User is login
	When User do something
	Then User sees result

@Regression @LinebreaksTest
Scenario Outline: ThirdTestScenario
	Given User is login
	When User do something
	Then User sees result

Examples:
	| Header1          | Header2 |
	| TestExampleData1 | Data1   |
	| TestExampleData2 | Data2   |

