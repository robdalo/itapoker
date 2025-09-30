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

  alertInterval = 0;
  alertMessage = "";
  alertTimerRunning = false;
  alertVisible = false;

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
      next: value => this.apiCallSuccess(value, false),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  apiCallSuccess(response: any, renderAlert: boolean) {
    
    this.saveGame(response);
    
    if (renderAlert)
      this.renderAlert(this.getGame().alert);
  }

  apiCallError(error: any) {
    this.renderAlert(error);
  }

  btnAnteClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/anteup", request).subscribe({
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, true),
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
      next: value => this.apiCallSuccess(value, false),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  clearAlert() {
      clearInterval(this.alertInterval);
      this.alertTimerRunning = false;
      this.alertVisible = false;
      this.alertMessage = "";
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
    return game.aiPlayer.lastBetType == 4 && // raise
           this.getPlayerBet() == 0;
  }

  isCardsDealt() {
    var game = this.getGame();
    return game.stage > 3; // deal
  }

  isCheckAvailable() {
    var game = this.getGame();
    return this.getPlayerBet() == 0 &&
           game.aiPlayer.lastBetType == 0; // no previous bet
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

   isRaiseAvailable() {
    var game = this.getGame();
    return this.getPlayerBet() > 0 && (
           game.aiPlayer.lastBetType == 0 || // no previous bet
           game.aiPlayer.lastBetType == 4); // raise
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
      next: value => this.apiCallSuccess(value, false),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  renderAlert(message: any) {

    if (!message)
      return;

    this.clearAlert();

    this.alertMessage = message;
    this.alertTimerRunning = true;

    this.alertInterval = setInterval(() => {
      this.alertVisible = !this.alertVisible;
    }, 1000);

    setTimeout(() => {
      this.clearAlert();
    }, 10000);
  }

  saveGame(game: any) {
    localStorage.setItem("game", JSON.stringify(game));
  }
}