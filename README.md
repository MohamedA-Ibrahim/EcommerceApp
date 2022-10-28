# EcommerceApp

## Project Structure
* backend folder contains the Api projects
* frontend folder contains the mobile and web projects 

# Backend
This folder contain the projects for the backend of the application. 
Currently it has four projects:
* Domain
* Application
* Infrastructure
* WebApi

## How to run
To run the API or the WebUI:
- First make sure you have sql server installed, then configure the connectionString in the appsettings.json in WebApi project
- Apply Migrations
- Run "Web" project

## Permissions
There are two roles in the app, user and admin. only admin can create categories and their attributes.
You can find the login details for the admin [here](backend/src/Infrastructure/Persistence/ApplicationDbContextSeed.cs)

## Demo
* [Api](http://129.151.247.108//swagger/index.html)
* [WebUI](http://129.151.247.108/)
