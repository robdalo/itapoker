# itapoker

## Introduction

Basic implementation of a five card poker game with a WebApi backend and Angular UI frontend. A single human player can play five card poker against an AI player, with configurable ante, cash pots and betting limits.

## How it works

The solution has been designed such that the Angular UI is as lightweight as possible and the bulk of the computational heavy lifting is the responsibility of the WebApi.

The WebApi processes requests from the Angular UI and stores the game state in a local in memory database, implemented using LiteDB. Each request and response in and out of the API is mapped using AutoMapper.

- itapoker.WebApi
    - C# .NET 9.0
    - LiteDB
    - AutoMapper

- itapoker.App
    - Angular
    - TypeScript

The .NET application features dependency injection and follows SOLID for separation of concerns.

Each request that enters the WebApi is validated using a validation service and each response that leaves the WebApi is redacted using a security service.

Card and Deck management is performed by the card service and betting is handled by the bet service. A decision service handles AI player decision making during the course of a game.

## How to run

To play the game, we must run both the WebApi and the Angular UI. Once the UI is launched, the URL of the WebApi can be configured via the Settings page.

- WebApi
    - cd itapoker.WebApi
    - dotnet run

- UI
    - cd itapoker.App/itapoker
    - ng serve