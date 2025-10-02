import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-card-table',
  imports: [CommonModule],
  templateUrl: './card-table.html',
  styleUrl: './card-table.css'
})
export class CardTable {

  alertInterval = 0;
  alertMessage = "";
  alertVisible = false;

  renderAnteUpInterval = 0;
  renderDealInterval = 0;

  constructor(
    private http: HttpClient,
    private router: Router) {
  }

  addChipClick(value: number) {

    if (!this.validateAddChip(value))
      return;

    var request = {
      GameId: this.getGame().gameId,
      Value: value
    };

    this.http.post("http://localhost:5174/game/chip/add", request).subscribe({
      next: response => this.addChipSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  addChipSuccess(response: any) {
    this.showAllCards(response);
    this.showAllChips(response);
    this.saveGame(response);
  }

  anteUpSuccess(response: any) {
    
    this.renderAnteUp();
    
    var wait = setInterval(() => {
      if (this.renderAnteUpInterval == 0) {
        clearInterval(wait);
        this.saveGame(response);
        this.renderAlert(response.alert);
      }    
    }, 500);
  }

  apiCallError(error: any) {
    this.renderAlert(error);
  }

  btnAnteClick() {
    var request = {
      GameId: this.getGame().gameId
    };

    this.http.post("http://localhost:5174/game/anteup", request).subscribe({
      next: value => this.anteUpSuccess(value),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnCallClick() {
    var request = {
      GameId: this.getGame().gameId,
      BetType: 1 // call
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: response => this.callSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnCheckClick() {
    var request = {
      GameId: this.getGame().gameId,
      BetType: 2 // check
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: response => this.checkSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnDealClick() {
    var request = {
      GameId: this.getGame().gameId
    };

    this.http.post("http://localhost:5174/game/deal", request).subscribe({
      next: response => this.dealSuccess(response),
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
      next: response => this.drawSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnFoldClick() {
    var request = {
      GameId: this.getGame().gameId,
      BetType: 3 // fold
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: response => this.foldSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnNextClick() {
    var request = {
      GameId: this.getGame().gameId
    };

    this.http.post("http://localhost:5174/game/next", request).subscribe({
      next: response => this.nextSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnRaiseClick() {
    var request = {
      GameId: this.getGame().gameId,
      BetType: 4, // raise
      Amount: this.getPlayerBet(this.getGame().player)
    };

    this.http.post("http://localhost:5174/game/bet", request).subscribe({
      next: response => this.raiseSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  btnRetireClick() {
    this.router.navigate(["/"]);
  }

  btnShowdownClick() {
    var request = {
      GameId: this.getGame().gameId
    };

    this.http.post("http://localhost:5174/game/showdown", request).subscribe({
      next: response => this.showdownSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  callSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);
  }

  checkSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);    
  }

  cardClick(rank: number, suit: number) {

    if (!this.validateHold())
      return;

    var request = {
      GameId: this.getGame().gameId,
      Rank: rank,
      Suit: suit
    };

    this.http.post("http://localhost:5174/game/hold", request).subscribe({
      next: response => this.cardClickSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  cardClickSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
  }

  clearAlert() {
      clearInterval(this.alertInterval);
      this.alertInterval = 0;
      this.alertVisible = false;
      this.alertMessage = "";
  }

  dealSuccess(response: any) {
    this.saveGame(response);
    this.renderDeal();

    var wait = setInterval(() => {
      if (this.renderDealInterval == 0) {
        clearInterval(wait);
        this.renderAlert(response.alert);
      }    
    }, 500);
  }

  drawSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);
  }

  foldSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }  

  getBetType(betType: number) {

    switch(betType) {

      case 1: return "Call";
      case 2: return "Check";
      case 3: return "Fold";
      case 4: return "Raise";

      default: return "";
    }
  }

  getCardUrl(index: number) {

    var card = this.getGame().player.cards[index];
    
    if (!card.reveal)
      return "images/cards/back.png";

    return card.url;
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

  getPlayerBet(player: any) {

    var bet = 0;
    var chips = player.chips as any[];

    chips.filter(x => x.visible).forEach(x => {
      bet += x.total;
    });

    return bet;
  }

  isAIPlayerHandVisible() {
    return false;
  }

  isAIPlayerLastBetVisible() {
    return this.getGame().stage != 8; // game over
  }

  isAnte() {
    return this.getGame().stage == 2; // ante up
  }

  isBet() {
    return this.getGame().stage == 4 || 
           this.getGame().stage == 6; // bet pre draw / bet post draw
  }

  isCallAvailable() {
    return this.getGame().aiPlayer.lastBetType == 4 && // raise
           this.getPlayerBet(this.getGame().player) == 0;
  }

  isCardsDealt() {
    return this.getGame().stage > 3; // deal
  }

  isCheckAvailable() {
    return this.getPlayerBet(this.getGame().player) == 0 &&
           this.getGame().aiPlayer.lastBetType == 0; // no previous bet
  }

  isDeal() {
    return this.getGame().stage == 3; // deal
  }

  isDraw() {
    return this.getGame().stage == 5; // draw
  }

  isFoldAvailable() {
    return this.getPlayerBet(this.getGame().player) == 0;
  }

  isGameOver() {
    return this.getGame().stage == 8; // gameover
  }

  isRaiseAvailable() {
    return this.getPlayerBet(this.getGame().player) > 0 && (
           this.getGame().aiPlayer.lastBetType == 0 || // no previous bet
           this.getGame().aiPlayer.lastBetType == 4); // raise
  }

  isShowdown() {
    return this.getGame().stage == 7; // showdown
  }

  nextSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }

  raiseSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
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
      next: response => this.removeChipSuccess(response),
      error: err => this.apiCallError(err),
      complete: () => {}
    });
  }

  removeChipSuccess(response: any) {
    this.showAllCards(response);
    this.showAllChips(response);
    this.saveGame(response);
  }

  renderAlert(message: any) {

    if (!message)
      return;

    this.clearAlert();

    this.alertMessage = message;

    this.alertInterval = setInterval(() => {
      this.alertVisible = !this.alertVisible;
    }, 1000);

    setTimeout(() => {
      this.clearAlert();
    }, 10000);
  }

  renderAnteUp() {

    var game = this.getGame();

    var pot = game.pot;

    game.pot = 0;

    this.saveGame(game);

    var anteUpChips = [
      { url: "images/chips/light-blue.png", title: "Light Blue Chip", value: 5, quantity: 1, total: 5, visible: false }
    ];

    game.player.chips = JSON.parse(JSON.stringify(anteUpChips));
    game.aiPlayer.chips = JSON.parse(JSON.stringify(anteUpChips));

    var index = 0;
    var renderAIPlayer = false;

    this.renderAnteUpInterval = setInterval(() => {
      if (!renderAIPlayer) {
        if (index >= anteUpChips.length) {        
          index = 0;
          renderAIPlayer = true;
        }
        else {
          game.player.chips[index++].visible = true;
          this.saveGame(game);
        }
      }
      else {
        if (index >= anteUpChips.length) {
          clearInterval(this.renderAnteUpInterval);
          this.renderAnteUpInterval = 0;
          game.player.chips = [];
          game.aiPlayer.chips = [];
          game.pot = pot;
          this.saveGame(game);
        }
        else {
          game.aiPlayer.chips[index++].visible = true;
          this.saveGame(game);
        }       
      }
    }, 1000);
  }

  renderDeal() {

    var game = this.getGame();

    var index = 0;
    var reveal = false;

    this.renderDealInterval = setInterval(() => {

      if (!reveal) {
        if (index >= game.player.cards.length) {
          reveal = true;
          index--;
        }
        else {
          game.player.cards[index++].visible = true;
          this.saveGame(game);
        }
      }
      else {
        if (index < 0) {
          clearInterval(this.renderDealInterval);
          this.renderDealInterval = 0;
        }
        else {
          game.player.cards[index--].reveal = true;
          this.saveGame(game);
        }
      }
    }, 500);
  }

  saveGame(game: any) {
    localStorage.setItem("game", JSON.stringify(game));
  }

  showAllCards(game: any) {
    
    var cards = game.player.cards as any[];

    cards.forEach(x => {
      x.reveal = true;
      x.visible = true;
    });
  }

  showAllChips(game: any) {

    var chips = game.player.chips as any[];

    chips.forEach(x => {
      x.visible = true;
    });
  }

  showdownSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }

  validateAddChip(value: number) {

    var game = this.getGame();

    // chips can only be added during pre draw and post draw betting

    if (game.stage != 4 && game.stage != 6)
      return false;

    // chips can only be added up to the game limit

    if (this.getPlayerBet(this.getGame().player) + value > this.getGame().limit)
      return false;

    // chips can only be added up to available cash

    if (value > this.getGame().player.cash)
      return false;

    return true;
  }

  validateHold() {
    // cards can only be held during draw
    return this.getGame().stage == 5;
  }
}