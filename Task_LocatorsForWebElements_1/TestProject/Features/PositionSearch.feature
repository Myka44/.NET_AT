Feature: Position search

As a job candidate
I want to search for positions using role and location criteria
So that I can find a relevant remote position

Scenario Outline: Latest position contains the searched programming language
    Given the user is on the EPAM home page
    When the user opens Careers and starts a position search
    And enters "<ProgrammingLanguage>" as the role or keyword
    And submits the position search
    And filters positions by "<Country>" and Remote
    And expands the latest position result
    Then the latest position description contains "<ProgrammingLanguage>"

Examples:
    | ProgrammingLanguage | Country               |
    | JavaScript          | Republic of Lithuania |
    | Java                | Poland                |
