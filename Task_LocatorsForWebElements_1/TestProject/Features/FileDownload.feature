Feature: Code of Ethical Conduct download

As a visitor
I want to download the Code of Ethical Conduct
So that I can read the policy offline

Scenario Outline: Code of Ethical Conduct PDF is downloaded
    Given the user is on the EPAM home page
    When the user downloads the Code of Ethical Conduct PDF from the footer
    Then the file "<ExpectedFileName>" is downloaded

Examples:
    | ExpectedFileName          |
    | Code_of_Ethical_Conduct.pdf |
