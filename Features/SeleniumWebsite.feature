Feature: Selenium Website Validation
  As a user
  I want to validate the Selenium website
  So that I can ensure its UI and functionality work correctly

  @smoke @home
  Scenario: Validate Selenium page title
    Given I navigate to "https://www.selenium.dev/"
    Then the page title should be "Selenium"

  @dataDriven @home @regression
  Scenario Outline: Search functionality validation
    Given I navigate to "https://www.selenium.dev/"
    When I click on "Documentation"
    And I enter "<SearchText>" in the search bar
    Then the search results should contain "<ExpectedResult>"

    Examples:
      | SearchText | ExpectedResult |
      | WebDriver  | WebDriver      |
      | Grid       | Grid           |
      | IDE        | IDE            |
