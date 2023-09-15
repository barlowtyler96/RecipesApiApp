# Recipes Web API

#### This solution consists of two projects, a class library for data access and a web API. My data access library utilizes dapper with stored procedures. The library is referenced in my API. The API is then consumed by my blazor server app. My favorite feature is the server-side paging. When a user searches for a recipe on the blazor app, only the results for the current page are returned and stored in memory. 

#### This project was created to practice data access, SQL Server, Dapper, authentication/authorization, and API developement.

#### I used Microsoft's documentation & code examples, Stack Overflow, and ChatGPT to complete the project. The biggest challenge I faced was learning how to implement Azures auth systems.

## Technologies used: 
* Custom Data Access Class Library
* Dapper w/ Stored Procedures
* SQL Server 2022
* Azure AD B2C

## Tools used:
* Postman
* Swagger
* [jwt.io ](https://jwt.io/)

## Features: 
* Endpoints secured with Azure AD B2C
* Server-side paging for pagination component
* Basic logging with ILogger
  
## Database Schema
![](ReadMeImages/culinaryshares-entity-relationship.PNG)

## Sample Video: 
[![Sample Video](https://img.youtube.com/vi/CcnFogJZYZ8/0.jpg)](https://www.youtube.com/watch?v=CcnFogJZYZ8)
