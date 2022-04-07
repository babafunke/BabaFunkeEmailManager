# BabaFunkeEmailManager
Custom Bulk Email Managing Project

[![Blog Inspiration (BabaFunke.DataAccess)](https://img.shields.io/badge/Blog-Inspiration-yellowgreen.svg?style=flat-square)](https://daddycreates.com/creating-a-bulk-email-management-system-overview/)
# Introduction 
BabaFunkeEmailManager is a complete project for managing subscribers, newsletters/emails and requests. It's been developed as a cheaper alternative to popular Email marketing tools in the market. It focuses on the core tasks of adding/updating/deleting/unsubscribing contacts (subscribers), creating and sending emails (newsletter) using a combination of Azure Table Storage, Azure Functions and Azure Queue Storage. Lastly, it provides a simple means of creating requests to send out emails to the list of subscribers. It includes the core API project, a client-facing website for managing the operations and finally, an alternative WebJob project for lengthy processes. It was inspired by the need for a self-sustainable system for engaging my growing audience via email.

# Solution
There is a single solution file which contains the projects below
* BabaFunkeEmailManager.Data - an Asp.Net Core Class library project that holds the data models and entities.
* BabaFunkeEmailManager.Service - an Asp.Net Core Class library project that holds the business logic.
* BabaFunkeEmailManager.Functions - an Azure Functions project that consumes the services above and exposes the endpoints for consumption.
* BabaFunkeEmailManager.Webjob - a standalone project for long-running background tasks. It’s an alternative to using a Queue Trigger Azure Function. It’s ideal if you’ll be running lengthy processes frequently and round the clock.
* BabaFunkeEmailManager.Client - a client-facing project that provides a user-friendly UI for managing the entire process.
* BabaFunkeEmailManager.Tests - an Xunit test for the services and endpoints.

# Build and Test
Clone this git repo and open the solution in Visual Studio. Update all Startup.cs, appsettings.json and local.settings.json files with your details where applicable. This includes SendGrid API keys, email addresses etc. 


# Background
For more on the project's background and walkthrough, read my 5-series post in the following order
* [Creating a Bulk-Email Management System – Overview](https://daddycreates.com/creating-a-bulk-email-management-system-overview/)
* [Creating a Bulk-Email Management System II – Models & Entities](https://daddycreates.com/creating-a-bulk-email-management-system-ii-models-entities/)
* [Creating a Bulk-Email Management System III – Services](https://daddycreates.com/creating-a-bulk-email-management-system-iii-services/)
* [Creating a Bulk-Email Management System IV – APIs](https://daddycreates.com/creating-a-bulk-email-management-system-iv-apis/)
* [Creating a Bulk-Email Management System V – WebJob](https://daddycreates.com/creating-a-bulk-email-management-system-v-webjob/)
