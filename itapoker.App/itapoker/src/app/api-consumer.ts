import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiConsumer {

  apiEndpoints: { [key: string]: string } = {
    "addChip": "game/chip/add",
    "anteUp": "game/anteup",
    "bet": "game/bet",
    "deal": "game/deal",
    "draw": "game/draw",
    "getHighScores": "game/highscores",
    "hold": "game/hold",
    "next": "game/next",
    "removeChip": "game/chip/remove",
    "showdown": "game/showdown",
    "singlePlayer": "game/singleplayer"
  };
  
  baseUrl = "http://localhost:5174/";

  constructor(
    private http: HttpClient) {
  }

  addChip(
    gameId: string, 
    value: number, 
    nextCallback: (response: any) => void, 
    errorCallback: (error: any) => void) 
  {
    this.http.post(this.getApiEndpoint("addChip"), {
      GameId: gameId,
      Value: value
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });    
  }

  anteUp(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("anteUp"), {
      GameId: gameId
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  call(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("bet"), {
      GameId: gameId,
      BetType: 1 // call
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  check(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("bet"), {
      GameId: gameId,
      BetType: 2 // check      
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });    
  }

  deal(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("deal"), {
      GameId: gameId
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  draw(
    gameId: string,
    cards: any[],
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("draw"), {
      GameId: gameId,
      Cards: cards
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  fold(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void) 
  {
    this.http.post(this.getApiEndpoint("bet"), {
      GameId: gameId,
      BetType: 3 // fold      
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  getApiEndpoint(name: string) {
    return this.baseUrl + this.apiEndpoints[name];
  }

  hold(
    gameId: string,
    rank: number,
    suit: number,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("hold"), {
      GameId: gameId,
      Rank: rank,
      Suit: suit
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });    
  }

  getHighScores(
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void) 
  {
    this.http.get(this.getApiEndpoint("getHighScores")).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  next(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("next"), {
      GameId: gameId      
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  raise(
    gameId: string,
    amount: number,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("bet"), {
      GameId: gameId,
      BetType: 4, // raise
      Amount: amount      
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  removeChip(
    gameId: string,
    value: number,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("removeChip"), {
      GameId: gameId,
      Value: value
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  showdown(
    gameId: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("showdown"), {
      GameId: gameId      
    }).subscribe({
      next: response => nextCallback(response),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }

  singlePlayer(
    playerName: string,
    nextCallback: (response: any) => void,
    errorCallback: (error: any) => void)
  {
    this.http.post(this.getApiEndpoint("singlePlayer"), {
      PlayerName: playerName
    }).subscribe({
      next: value => nextCallback(value),
      error: error => errorCallback(error),
      complete: () => {}
    });
  }
}