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

  ante = String();
  limit = String();
  cash = String();
  hand = String();
  stage = String();
  pot = String();
  playerCash = String();
  playerWinnings = String();

  chipsAIPlayer: number[] = [ 0, 0, 0, 0 ];
  chipsPlayer: number[] = [ 0, 0, 0, 0 ];
  holdAIPlayer: Boolean[] = [ false, false, false, false, false ];
  holdPlayer: Boolean[] = [ false, false, false, false, false ];

  constructor(
    private http: HttpClient) {
  }

  ngOnInit() {
    this.updateCardTable();
  }

  updateCardTable() {
    
    var game = this.getGame();

    this.ante = game.ante;
    this.limit = game.limit;
    this.cash = game.cash;
    this.hand = game.hand;
    this.stage = this.getGameStage(game.stage);
    this.pot = game.pot;
    this.playerCash = game.player.cash;
    this.playerWinnings = game.player.winnings;
  }

  addChipClick(number: number) {
    this.chipsPlayer[number]++;
  }

  btnAnteClick() {
    
    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/anteup", request).subscribe({
      next: value => this.anteUpSuccess(value),
      error: err => this.anteUpError(err),
      complete: () => {}
    });
  }

  btnDrawClick() {

  }

  btnCallClick() {

    var game = this.getGame();

    var request = {
      GameId: game.gameId,
      BetType: 1 // call
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: value => this.callSuccess(value),
      error: err => this.callError(err),
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
      next: value => this.checkSuccess(value),
      error: err => this.checkError(err),
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
      next: value => this.foldSuccess(value),
      error: err => this.foldError(err),
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
      next: value => this.raiseSuccess(value),
      error: err => this.raiseError(err),
      complete: () => {}
    });
  }

  btnDealClick() {
    
    var game = this.getGame();

    var request = {
      GameId: game.gameId
    };

    this.http.post("http://localhost:5174/game/deal", request).subscribe({
      next: value => this.dealSuccess(value),
      error: err => this.dealError(err),
      complete: () => {}
    });     
  }

  anteUpSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();
  }

  anteUpError(error: any) {

  }

  callSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();
  }

  callError(error: any) {

  }

  checkSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();
  }

  checkError(error: any) {

  }

  foldSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();    
  }

  foldError(error: any) {

  }

  dealSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();
  }

  dealError(error: any) {

  }

  raiseSuccess(response: any) {
    this.saveGame(response);
    this.updateCardTable();
  }

  raiseError(error: any) {

  }  

  cardClick(number: number) {
    this.holdPlayer[number] = !this.holdPlayer[number];
  }

  getGame() {
    var json = localStorage.getItem("game");
    return json ? JSON.parse(json) : null;
  }

  saveGame(game: any) {
    localStorage.setItem("game", JSON.stringify(game));
  }

  getGameStage(number: number) {
    if (number == 1)
      return "NewGame";    
    else if (number == 2)
      return "Ante";
    else if (number == 3)
      return "Deal";
    else if (number == 4)
      return "BetPreDraw";
    else if (number == 5)
      return "Draw";
    else if (number == 6)
      return "BetPostDraw";
    else if (number == 7)
      return "Showdown";
    else if (number == 8)
      return "GameOver";

    return "";
  }

  getPlayerBet() {
    return (this.chipsPlayer[0] * 5) +
           (this.chipsPlayer[1] * 10) +
           (this.chipsPlayer[2] * 25) +
           (this.chipsPlayer[3] * 50);
  }

  getPot() {
    return this.getPlayerBet();
  }

  isAnte() {
    var game = this.getGame();
    return game.stage == 2; // ante up
  }

  isDeal() {
    var game = this.getGame();
    return game.stage == 3; // deal
  }

  isBet() {
    var game = this.getGame();
    return game.stage == 4; // bet pre draw
  }

  isDraw() {
    var game = this.getGame();
    return game.stage == 5; // draw
  }  

  isPostDeal() {
    var game = this.getGame();
    return game.stage > 3; // deal
  }

  removeChipClick(number: number) {
    this.chipsPlayer[number]--;
  }  
}