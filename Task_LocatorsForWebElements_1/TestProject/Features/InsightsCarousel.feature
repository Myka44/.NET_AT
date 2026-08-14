Feature: Insights carousel

As a visitor
I want to open an article from the Insights carousel
So that I can verify that the selected article is opened

Scenario Outline: Article title matches the selected carousel title
    Given the user is on the EPAM home page
    When the user opens Insights and swipes the carousel <SwipeCount> times
    And notes the active article title and opens the article
    Then the article page title matches the selected carousel title

Examples:
    | SwipeCount |
    | 2          |
    | 3          |
