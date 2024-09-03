# CulinaryShares Web API

#### This solution consists of two projects, a class library for data access and a web API. My data access library utilizes dapper with stored procedures. The library is referenced in my API. The API is then consumed by my blazor server app. My favorite feature is the server-side paging. When a user searches for a recipe on the blazor app, only the results for the current page are returned and stored in memory. 

#### This project was created to practice data access, SQL Server, Dapper, authentication/authorization, and API developement.

#### I used Microsoft's documentation & code examples, and stack overflow to complete the project. The biggest challenge I faced was learning how to implement Azures auth systems.

## Technologies used: 
* Data Access Class Library
* Dapper w/ Stored Procedures
* SQL Server 2022
* Azure AD B2C

## Tools / Libraries Used:
* xUnit w/ Fluent Assertions
* SSMS
* Dapper
* Postman
* Swagger
* [jwt.io ](https://jwt.io/)

## Features: 
* Endpoints secured with Azure AD B2C
* Versioning
* IP Rate Limiting
* Basic logging with ILogger
* Server-side paging for pagination component
## Postman Documentation: 
https://documenter.getpostman.com/view/27883820/2s9YCARW1m
  
## Database Schema
![](ReadMeImages/culinaryshares-entity-relationship.PNG)

## Want to contribute?
1. Open Git Bash to the location you wish to clone the repository.
2. Run the following command: ```bash git clone https://github.com/barlowtyler96/RecipesApiApp.git ```
