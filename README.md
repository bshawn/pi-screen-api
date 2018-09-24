# pi-screen-api

## Introduction

This project is the RESTful API layer intended for use in maintaining display screens in a facility.  The major components of the system include:

- API - written in .NET Core 2.1
- Client - intended to be run on a Raspberry Pi

At this time, the backing datastore for system data is a simple json file.  This will eventually change to something more scalable.

## Dependencies

- [.NET Core 2.1](https://www.microsoft.com/net/download)

## Running the code

```bash
git clone github.com/bshawn/pi-screen-api.git
cd pi-screen-api/src
dotnet run
```

## Testing API calls using Postman

Postman is used to test API calls.  The repo contains a [postman](postman) folder containing the environment and ScreenAPI collection.

Download Postman [here](https://getpostman.com).