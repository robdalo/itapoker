import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-card-table',
  imports: [CommonModule],
  templateUrl: './card-table.html',
  styleUrl: './card-table.css'
})
export class CardTable {

  errorMessage = "";
  errorMessageTimerRunning = false;
  errorMessageVisible = false;
  handTitleTimerRunning = false;
  handTitleVisible = false;

  constructor(
    private http: HttpClient) {
  }

  addChipClick(value: number) {

    var game = this.getGame();

    // chips can only be added during pre draw and post draw betting

    if (game.stage != 4 && game.stage != 6)
      return;

    // chips can only be added up to the game limit

    if (this.getPlayerBet() + value > this.getGame().limit)
      return;

    var request = {
      GameId: game.gameId,
      Value: value
    };

    this.http.post("http://localhost:5174/game/chip/add", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  apiCallSuccess(response: any) {
    this.saveGame(response);
  }

  apiCallError(error: any) {
    this.renderErrorMessage(error);
  }

  btnAnteClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/anteup", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnCallClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId,
      BetType: 1 // call
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnCheckClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId,
      BetType: 2 // check
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnDealClick() {
    
    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/deal", request).subscribe({
      next: value => { 
        this.apiCallSuccess(value);
        this.renderFlashingHandTitle();
      },
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnDrawClick() {

    var game = this.getGame();
    var cards = game.player.cards as any[];

    var request = {
      GameId: game.gameId,
      Cards: cards.filter(item => !item.hold)
    };

    this.http.post("http://localhost:5174/game/draw", request).subscribe({
      next: value => {
        this.apiCallSuccess(value);
        this.renderFlashingHandTitle();
      },
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnFoldClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId,
      BetType: 3 // fold
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnNextClick() {
  
    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/next", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnRaiseClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId,
      BetType: 4, // raise
      Amount: this.getPlayerBet()
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnShowdownClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/showdown", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  cardClick(rank: number, suit: number) {

    var game = this.getGame();

    // cards can only be held during draw
    
    if (game.stage != 5)
      return;

    var request = {
      GameId: game.gameId,
      Rank: rank,
      Suit: suit
    };

    this.http.post("http://localhost:5174/game/hold", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  getGame() {
    var json = localStorage.getItem("game");
    return json ? JSON.parse(json) : null;
  }

  getGameStage(stage: number) {

    switch (stage) {

      case 1: return "NewGame";
      case 2: return "Ante";
      case 3: return "Deal";
      case 4: return "BetPreDraw";
      case 5: return "Draw";
      case 6: return "BetPostDraw";
      case 7: return "Showdown";
      case 8: return "GameOver";

      default: return "";
    }
  }

  getHandTitle(handType: number) {

    switch (handType)
    {
      case 2: return "Two High";
      case 3: return "Three High";
      case 4: return "Four High";
      case 5: return "Five High";
      case 6: return "Six High";
      case 7: return "Seven High";
      case 8: return "Eight High";
      case 9: return "Nine High";
      case 10: return "Ten High";
      case 11: return "Jack High";
      case 12: return "Queen High";
      case 13: return "King High";
      case 14: return "Ace High";
      case 20: return "One Pair";
      case 30: return "Two Pair";
      case 40: return "Three Of A Kind";
      case 50: return "Straight";
      case 60: return "Flush";
      case 70: return "Full House";
      case 80: return "Four Of A Kind";
      case 90: return "Straight Flush";
      case 100: return "Royal Flush";

      default: return "";
    }
  }

  getPlayerBet() {

    var bet = 0;
    var chips = this.getGame().player.chips as any[];

    chips.forEach(x => {
      bet += x.total;
    });

    return bet;
  }

  isAnte() {
    var game = this.getGame();
    return game.stage == 2; // ante up
  }

  isBet() {
    var game = this.getGame();
    return game.stage == 4 || game.stage == 6; // bet pre draw / bet post draw
  }

  isCallAvailable() {

    var game = this.getGame();
    return game.aiPlayer.lastBetType == 4; // raise
  }

  isCardsDealt() {
    var game = this.getGame();
    return game.stage > 3; // deal
  }

  isDeal() {
    var game = this.getGame();
    return game.stage == 3; // deal
  }

  isDraw() {
    var game = this.getGame();
    return game.stage == 5; // draw
  }

  isGameOver() {
    var game = this.getGame();
    return game.stage == 8; // gameover
  }

  isShowdown() {
    var game = this.getGame();
    return game.stage == 7; // showdown
  }

  removeChipClick(value: number) {

    var game = this.getGame();

    // chips can only be removed during pre draw and post draw betting
    
    if (game.stage != 4 && game.stage != 6)
      return;

    var request = {
      GameId: game.gameId,
      Value: value
    };

    this.http.post("http://localhost:5174/game/chip/remove", request).subscribe({
      next: value => this.apiCallSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  renderErrorMessage(error: any) {

    this.errorMessage = error.error;
    this.errorMessageTimerRunning = true;

    var interval = setInterval(() => {
      this.errorMessageVisible = !this.errorMessageVisible;
    }, 1000);

    setTimeout(() => {
      clearInterval(interval);
      this.errorMessageTimerRunning = false;
      this.errorMessageVisible = false;
    }, 10000);
  }

  renderFlashingHandTitle() {

    this.handTitleTimerRunning = true;

    var interval = setInterval(() => {
      this.handTitleVisible = !this.handTitleVisible;
    }, 1000);

    setTimeout(() => {
      clearInterval(interval);
      this.handTitleTimerRunning = false;
      this.handTitleVisible = false;
    }, 10000);
  }

  saveGame(game: any) {
    localStorage.setItem("game", JSON.stringify(game));
  }
}