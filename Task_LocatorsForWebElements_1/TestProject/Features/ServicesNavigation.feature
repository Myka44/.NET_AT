Feature: Navigate to a service category

As a visitor
I want to be able to navigate to a specific service
So that I can view information about it

Scenario Outline: Navigation to a service category from the main navigation bar
	Given the user is on the EPAM home page
	When the user hovers 'Services' button on the main navigation bar
		And the user clicks "<ServiceCategory>" service category
	Then the new page contains "<ServiceCategory>" in the title
		And the Our Related Expertise section is displayed on the page


Examples: 
| ServiceCategory |
| Generative AI   |
| Responsible AI  |


