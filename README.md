AgeRanger is a world leading application designed to identify person's age group!
The only problem with it is... It is not implemented - except a SQLite DB called AgeRanger.db.

To help AgeRanger to conquer the world please implement a web application that communicates with the DB mentioned above, and does the following:

 - Allows user to add a new person - every person has the first name, last name, and age;
 - Displays a list of people in the DB with their First and Last names, age and their age group. The age group should be determened based on the AgeGroup DB table - a person belongs to the age group where person's age >= 
 than group's MinAge and < than group's MaxAge. Please note that MinAge and MaxAge can be null;
 - Allows user to search for a person by his/her first or last name and displays all relevant information for the person - first and last names, age, age group.

In our fantasies AgeRanger is a single page application, and our DBA has already implied that he wants us to migrate it to SQL Server some time in the future.
And unit tests! We love unit tests!

Last, but not the least - our sales manager suggests you'll get bonus points if the application will also allow user to edit existing person records and expose a WEB API.

Please fork the project.

You are free to use any technology and frameworks you need. However if you decide to go with third party package manager or dev tool - don't forget to mention them in the README.md of your fork.


Good luck!



## Backend
- Entity Framework 6 Code First and SQLite extensions
- Autofac
- AutoMapper
- Microsoft.Extensions.Logging and NLog
- NUnit
- WebAPI 2
- .Net Framework 4.5.2
- Visual Studio 2015 Community
## Frontend
- [Yeoman (AngularJS, Requirejs)](https://github.com/aaronallport/generator-angular-require)
- Bootstrap 3.X
- Grunt
## Design 
- Domain Driven Design(DDD)
  - Presentation (WebAPI & WebAPP)
  - Application (Application services & DTOs)
  - Domain (Commnads, Command Handlers, Events, Event Handlers, Entities & EventBus)
  - Infrasturcture (Repositories, ErrorHandler, Logger, DIManager & Database providers)
- Contract & Interface
  - Contract in AgeRanger is the only interface with on statement of any properties or methods and used by Autofac by giving a readable configuration. Contract should implement interface otherwise it dosen't make any sense
  - Interface in AgeRanger is the typical interface....
- CQRS (Command/Event/EventBus)
  - Each of database providers should have 2 Dbcontext refering to write side and read side, for instance, AgeRanger.SQLite in AgeRanger, AgeRangerDbContext is for read side and AgeRangerWriterDbContext is for write side. Write side and read side are using same database in current circumstance. It can be configured in Web.config/connectionString :
  ```
  <connectionStrings>
    <add name="AgeRangerDB" connectionString="data source=|DataDirectory|\sqlite\AgeRanger.db;foreign keys=true" providerName="System.Data.SQLite" />
    <add name="AgeRangerDBWriter" connectionString="data source=|DataDirectory|\sqlite\AgeRanger.db;foreign keys=true" providerName="System.Data.SQLite" />
  </connectionStrings>
  ```
- Dependence Injection
  - Autofac
  - DI is used in every laryer, so each layer is independent even WebAPI
  - DI is also used to inject instance of DbContext into instance of Repository. It means it is very easy to migrant to any database providers if the database provider was included in Infrasturcture
  - DI configurations consists of two parts. One part is in json file resgistering database providers and event handlers. The other part is in Global.asax/Application_Start resgistering others.
- ORM
  - EntityFramework 6 Code First with SQLite extensions
- Logger
  - Logger is only a controller to be used to register log providers on the basis of Microsoft.Extensions.Logging and NLog provider. Changing provider only need change code in LoggerController.cs or create inheritance from LoggerController and register new controller in Global.asax/Application_Start using Autofac
- ErrorHandler
  - ErrorHandler provides a strategy transforming uncatchced exceptions from server to event and trigger the event, so the details of excption will not expose to client only in log files.
  - ErrorHandler provides global error handling for WebAPI using ActionExceptionFilter of WebAPI and return related HttpStautsCode to client
  - WebApp also has global error hander making efforts by being injected into AngularJS httpProvider
- API Help
  - WebAPI provides API help pages. "http://{Domain}/Help"
## Deploy
### WebApi (automatic way)
1. Locate to {Path}\AgeRanger\Tools\Deploy\Service\ and run "nuget restore {Path}AgeRanger\AgeRanger.sln" to restore packages
2. Install [MSDeploy](https://www.iis.net/downloads/microsoft/web-deploy)
3. Create a new website "AgeRanger" and a empty physical folder mapping to "AgeRanger" website in IIS
4. Open cmd under Administrator and locate to "C:\Program Files (x86)\MSBuild\14.0\Bin"
5. Run "MSBuild {Path}\AgeRanger\Tools\Deploy\Service\Project.csproj"
6. If successful, a new folder "AgeRanger" would be created including a folder for backup of the existing website and a folder for the undeployed package of AgeRanger
7. Check the "AgeRanger" website. All of files would be there
### WebApp
1. Install Nodejs
2. Intall npm, bower and grunt
3. Locate to "{Path}\AgeRange\Presentation\App"
4. Run "grunt build"
5. Check the folder dist
6. Don't forget to find file "config.js" in folder scripts and change the api base url
## TODO
- Authentication & Authorization is not there.
