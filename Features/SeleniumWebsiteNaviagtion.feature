Feature: Selenium Website Navigation Validation
  As a user
  I want to validate the Selenium website
  So that I can ensure its UI Naviagtion functionality work correctly

  @regression
  Scenario Outline: Verify navigation to multiple pages
    Given I navigate to "https://www.selenium.dev/"
    When I click on "<LinkText>"
    Then the page title should be "<ExpectedTitle>"

    Examples:
      | LinkText      | ExpectedTitle                           |
      | Projects      | Projects                                |
      | Documentation | The Selenium Browser Automation Project |
      | Blog          | Blog                                    |

  @dataTable
  Scenario: Verify presence of navigation links
    Given I navigate to "https://www.selenium.dev/"
    Then the following navigation links should be present:
      | Link Text     |
      | Projects      |
      | Documentation |
      | Support       |
      | Blog          |

  @regression
  Scenario: Validate Documentation Page Navigation
    Given I navigate to "https://www.selenium.dev/"
    When I click on "Documentation"
    Then the page title should be "The Selenium Browser Automation Project"
