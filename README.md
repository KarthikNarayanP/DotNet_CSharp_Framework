# **📌 Selenium C# BDD Framework**

**Automated Testing Framework** using:
- **Selenium WebDriver**
- **ReqNroll (BDD Framework)**
- **NUnit (Test Runner)**
- **Extent Reports (HTML Reporting)**
- **GitHub Actions (CI/CD Automation)**

---

## **📁 Project Structure**
```
📂 DotNet_CSharp_Framework
│── 📂 Features                     # BDD Feature Files (Gherkin syntax)
│   │── SeleniumWebsite.feature                # Example Feature for SeleniumWebsite
│   │── SeleniumWebsiteNaviagtion.feature      # Example Feature for SeleniumWebsiteNaviagtion
│
│── 📂 StepDefinitions               # Step Definitions for BDD Scenarios
│   │── LoginSteps.cs                 # Step Definitions for Login
│   │── SearchSteps.cs                # Step Definitions for Search
│
│── 📂 Pages                         # Page Object Model (POM) Classes
│   │── SeleniumPage.cs               
│            
│
│── 📂 Utilities                     # Utility Methods
│   │── WebdriverFactory.cs           # WebDriver Handling (Reusable)
│   │── ExtentReportManager.cs        # HTML Reporting
│
│── 📂 Hooks                         # Test Hooks (Before/After Scenarios)
│   │── TestHooks.cs                  # Setup and Teardown
│
│── 📂 Reports                       # Extent Reports (Generated)
│
│── 📂 .github/workflows             # CI/CD GitHub Actions Workflow
│   │── main.yml            # GitHub Actions Configuration
│
│── 📜 README.md                      # Project Documentation
│── 📜 DotNet_CSharp_Framework.csproj  # .NET Project Configuration
│── 📜 .gitignore                      # Ignore files
```

---

## **📌 Features of the Framework**
 **Selenium WebDriver** → Automates Browser Interactions  
 **ReqNroll** → BDD Testing with Feature Files  
 **NUnit** → Test Execution & Assertions  
 **Extent Reports** → HTML Reports with Screenshots  
 **Parallel Execution** → Faster Test Runs  
 **GitHub Actions** → Automated Testing in CI/CD  

---

## ** Setup & Installation**
### **1️⃣ Prerequisites**
Ensure you have:
- **.NET SDK 9.0+**

### **2️⃣ Clone the Repository**
```sh
git clone https://github.com/your-repo/DotNet_CSharp_Framework.git
cd DotNet_CSharp_Framework
```

### **3️⃣ Install Dependencies**
```sh
dotnet restore
```

---

## **🚀 Running Tests**
### **1️⃣ Run All Tests Locally**
```sh
dotnet test
```

### **2️⃣ Run Tests by Tag**
```sh
dotnet test --filter TestCategory=Regression
```

### **3️⃣ Run Tests in Parallel**
```sh
dotnet test --settings parallel.runsettings
```

### **4️⃣ Generate & View Reports**
Reports are generated in `/Reports/ExtentReport.html`.  
To open in browser:
```sh
start Reports/ExtentReport.html
```

---
