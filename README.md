.NET Test Automation Framework

A C# project for automating browser scenarios and REST API checks. It separates shared configuration and browser helpers from reusable page objects, API models and test scenarios.

UI tests use Selenium WebDriver, Reqnroll and xUnit. Scenarios cover site navigation, search and file downloads.
API tests use NUnit and RestSharp to send HTTP requests and check responses.
Run locally
Install the .NET 9 SDK and Chrome, Edge or Firefox. From the repository root, run:

cd Task_LocatorsForWebElements_1
dotnet test Task_LocatorsForWebElements_1.sln
The UI suite defaults to Chrome. Set UI_BROWSER to edge or firefox to use a different installed browser. Browser and API settings are in the appsettings*.json files in UiTests and ApiTests.

The GitHub Actions workflow defines scheduled and manual runs. Its UI step currently references an outdated project path. The current project is Task_LocatorsForWebElements_1/UiTests/TestsUI.csproj.
