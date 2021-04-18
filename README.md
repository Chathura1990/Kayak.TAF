# **Kayak.TAF**

---

## **Agenda**

* [Description](#description)
* [Technologies](#technologies)
* [Browser support](#browser-support)
* [Modules](#modules)
* [How to run tests](#how-to-run-tests)
    * [Run from the command line](#run-from-the-command-line)
    * [Running for different environments](#running-for-different-environments)
    * [Run from Visual Studio](#run-from-visual-studio)
    * [Run from JetBrains Rider](#run-from-jetbrains-rider)
* [Parallel tests execution](#parallel-tests-execution)
* [Test Result Report](#test-result-report)
    * [Screenshots](#screenshots)
* [How to add UI test](#how-to-add-ui-test)
* [Configurations](#configurations)
    * [appsettings.json Files*](#appsettingsjson-files)
    * [Configuration section](#configuration-section)
    * [How to get any value from the .json file](#how-to-get-any-value-from-the-json-file)
    * [How to add a new appsettings.{Environment}.json file](#how-to-add-a-new-appsettingsenvironmentjson-file)
* [IoC Container](#ioc-container) 
    * [UnityContainer](#unitycontainer)
    * [Register](#register)
    * [Resolve](#resolve)
* [Page Object Pattern](#page-object-pattern)  
* [Logging](#logging)

---

## **Description**

Test Automation Framework provides the ability to automate UI for Kayak website.

---

## **Technologies**
1. ASP.Net Core 3.1
2. NUnit3
3. Selenium WebDriver
4. BDDfy
5. Log4net
6. Unity Container

---

##Browser support
1. **Chrome** (by default)
2. **Chrome Headless**

---

## **Modules**

Kayak.TAF has several modules, the main modules are:

1. **Core** – project with main classes to work with WebDriver, Web Elements, Logs, Configurations, etc.
2. **PageModels** – project where the implementation of page object models are located (all pages with all its elements, actions and validations for it).
3. **UiSteps** - project where UI Test Steps implementations are located.
4. **UiTests** - project where UI Test Scenarios (Features are located.

---

## **How to run tests**

### **Run from the command line**
1. Open command line
2. Navigate to the directory containing the *.sln
3. Run the following command

```powershell
dotnet build --configuration {DEV/Local/etc} 
dotnet test --filter "TestCategory={Tag}"
```
Where `{Tag}` can be *Smoke*, *API* or any other that you may specify in tests under `[Category("")]` attribute.

The list of existing tags:

1. Smoke
2. Regression

3. UI
4. API

5. Highest
6. High
7. Medium
8. Low
9. Lowest

### **Running for different environments**

While running the test with dotnet test command add --configuration parameter

```
dotnet test --configuration {Environment}
```
For more info about command line parameters **see this [link](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=netcore21)**

### **Run from Visual Studio**

[VS Test Explorer Guide](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019)

1. Open the *.sln in VS
2. Build Solution (select necessary configuration, e.g Local, DEV, AQA, etc.)
3. Open Test -> Windows -> Test Explorer
4. Click on 'Run All' (if you want to run all the tests) or right click on selected tests and 'Run Selected Tests'

### **Run from JetBrains Rider**

[JetBrains Unit Testing Guide](https://www.jetbrains.com/help/rider/Unit_Testing__Index.html)

1. Open the *.sln in JetBrains Rider
2. Build Solution (select necessary configuration, e.g Local, DEV, AQA, etc.)
3. Choose *Feature.cs and click on 'Run All' (if you want to run all the tests) or right click on selected tests and 'Run'

---

## **Parallel tests execution**

All test classes (or its parent) should have ```[Parallelizable]``` attribute.

```ParallelScope``` is set to Fixtures. That means parallelization will be on Features level.

```
[Parallelizable(ParallelScope.Fixtures)]
```
### **How to restrict max number of parallel tests**

There is an attribute in NUnit called **LevelOfParallelism**.

For .Net Framework projects `--workers` argument can be used. But for .Net Core `--workers` argument isn't available now (see this [issue](https://github.com/nunit/nunit-console/issues/475))
So, the only way to restrict number of parallel tests - is custom **LevelOfParallelism** attribute.

* Add the following task in .**csproj** (e.g. in UiTests.csproj)
```xml
<ItemGroup>
    <AssemblyAttribute Include="Core.Attributes.LevelOfParallelismAttribute">
        <_Parameter1>5</_Parameter1>
    </AssemblyAttribute>
</ItemGroup>
```
**NOTE**: NUnit.Framework.LevelOfParallelismAttribute cannot be used, because it takes only int parameter, but this task (with Core.Attributes.LevelOfParallelismAttribute) will convert Parameter1 to string.

* Verify that your own version of LevelOfParallelismAttribute.cs is present in Core. In compare with NUnit.Framework.LevelOfParallelism, it takes string parameters and allows to execute build task successfully.

* Build the solution.

---

## **Test Result Report**

Test results are generated by BDDfy and placed under `/bin/{build}` folder as **'BDDfy.html'**.

### **Screenshots**

Screenshots are captured for failed UI tests and stored in `/bin/{build}/Screenshots` folder.

---

## **How to add UI test**
### **Add a new test to the existing feature**
* Navigate to UiTests/Features and open corresponding *Feature.cs file
* Create a new method in the feature class
* Mark this method with `[Test]` attribute
```csharp
[Test]
public void NewTestCase() {...}
```
* Using BDDfy Fluent API specify test steps with 
```csharp
That.Given()
    .When()
    .Then()
    .BDDfy();
```
* Use `[Category("")]` attribute to filter the UI tests if needed. E.g. `[Category("Smoke")]`

The TAF automatically detects a new test and shows it under Test Explorer window in Visual Studio (to open it follow Test -> Windows -> Test Explorer)

### **Add a new feature**
1. Navigate to UiTests/Features
2. Create a folder for each different feature inside the Features folder to categorize them  
3. Right click the folder with specific features and Add -> New Class
4. Use `[Category("")]` attribute to filter your features if needed
5. Derive your class from `TestHelper` 
```csharp
[TestFixture]
[Category("UI")]
public class NewTestFeature : TestHelper {}
```

---

## **Configurations**

### **appsettings.*.json Files**

All the parameters are stored in *.json files. There are different **appsettings.{Environment}.json** files for different environments.  **appsettings.json** file is used as default configurations file (Local).

### **Configuration section**
**ConfigManager.cs** uses ConfigurationBuilder (see GetConfiguration()). **ConfigurationBuilder** allows to read configurations from different sources (.json, command line, azure key vault, environment variables and so on).
See this [link](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1).
Configuration.cs allows to **automatically map values** from any configurations provider (e.g. from *.json) to instances of classes that implements **IConfiguration** interface (see ConnectionConfiguration for example).
Configuration.cs also allows to read value from *.json with **nested levels** (e.g. Configuration.GetValue("Browser:Type")).

### **How to get any value from the .json file**
To retrieve any value form the config file use the following command `ConfigManager.{Configuration section}.{Parameter}`

**Example**:  `ConfigManager.Browser.StartUrl`,  
              `ConfigManager.Wait.DefaultTimeout`

### **How to add a new appsettings.{Environment}.json file**
1. Create a new .json with all the necessary parameters
2. Update ConfgiManager.**GetConfiguration()** method specifying the build configuration to which the new .json file related:

``` csharp
private static IConfigurationRoot GetConfiguration()
{
    return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
#if LOCAL
            .AddJsonFile("appsettings.json", true, true)
#elif QA
            .AddJsonFile("appsettings.QA.json", true, true)
#endif
            .Build();
}
```
---

## **IoC Container**
### **UnityContainer**
**UnityContainer** creates an object of the **Steps** class and injects all the dependency objects through constructor at run time and disposes it at the appropriate time. **UnityContainer** is placed in **TestHelper.cs** class for **Ui**  project. It is marked with **[OneTimeSetUp]** attribute so that it's called before each test method.

### **Register**
Before Unity resolve dependencies, we first need to register the type-mapping with the container, so that it can create the correct object for the given type. Use **RegisterType()** method to register a type mapping.
``` csharp
[OneTimeSetUp]
public void GlobalSetUp()
{
    //...
    Container.RegisterType<HttpClient>()
         .RegisterType<BaseUiSteps>()
         .RegisterType<FlightSearchUiSteps>()
         .RegisterType<FlightSearchResultUiSteps>();
    //...
}
```

### **Resolve**
Unity creates an object of the specified class and automatically injects dependencies using `Resolve()` method. Use Resolve method when you want to get an object of any *Steps* class in **TestHelper.cs**
``` csharp
protected FlightSearchUiSteps FlightSearchUiSteps => Container.Resolve<FlightSearchUiSteps>();
```

---

## **Page Object Pattern**
The classes and objects participating in this pattern are:

1. **Page** (PageModels/Pages/BasePage.cs)- Holds the actions that can be performed on the page. Exposes an easy access to the Page Validator through the Validate() method. The best implementations of the pattern hide the usage of the Element Map, wrapping it through all action methods.
2. Page **Element Map** (PageModels/Maps/FlightSearchPageElementMap.cs) – Contains all element properties and their location logic.
3. Page **Validator** (PageModels/Validators/FlightSearchPageValidator.cs) – Consists of the validations that can be performed on the page.

To get quick access to the page and its elements use **PageFactory.GetPage()** method
``` csharp
public class PageFactory
{
    public static TPage GetPage<TPage>() where TPage : BasePage, new()
    {
        var page = new TPage {};
        return page
    }
}
```
**Example**: `PageFactory.GetPage<LoginPage>()`

---

## **Logging**

**Log4Net** is used in TAF.

**LogFile.txt** is generated after each test execution and placed under **/bin** directory.

**NOTE**: While TestHelper.cs has `[Parallelizable(ParallelScope.Fixtures)]` attribute, log lines will be mixed up in the log file, but not in Visual Studio. Run it not in parallel if you want to see sequential entries in the log file.

---

