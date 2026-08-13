Feature: Global search

As a visitor
I want to search the EPAM website
So that I can find content related to a keyword

Scenario Outline: All global search results contain the search keyword
    Given the user is on the EPAM home page
    When the user performs a global search for "<SearchKeyword>"
    Then at least one global search result is displayed
    And all global search result titles contain "<SearchKeyword>"

Examples:
    | SearchKeyword |
    | BLOCKCHAIN    |
    | Cloud         |
    | Automation    |
