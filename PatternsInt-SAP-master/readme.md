It is a project for students

# Technologies
### This project uses .NET 6 ([sdk](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)) platform and ASP.NET Core MVC Framework ([documentation](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-6.0)).

### This project uses [Entity Framework](https://learn.microsoft.com/en-us/ef/) for ORM.

### This project has been configured to use [MS SQL Server](https://learn.microsoft.com/en-us/sql/sql-server/?view=sql-server-ver16). If you are not windows user, then you can use docker container with this DBMS ([dockerhub mssql](https://hub.docker.com/_/microsoft-mssql-server)).

### You can also change DBMS to any relational DBMS supported by Entity Framework if you don't want (can't) to use MS SQL Server. But, first of all you will need to install provider for this DBMS using Nuget Package manager, remove existing migrations and generate a new one for selected DBMS using the newly installed provider.

# Some information you need to know
### There is a seed of data in project. See `ApplicationDbInitializer` class to find admin password. Or if you want to add data to seed method.

### Projects EmailSdk and SmsSdk are imitation sdk for developing notification functionality. You will need them in one of the tasks.
