import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Validator } from '../validator';
import { ApiConsumer } from '../api-consumer';

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
    private apiConsumer: ApiConsumer,
    private router: Router,
    private validator: Validator) {
  }

  addChipClick(value: number) {

    var game = this.getGame();

    if (!this.validator.validateAddChip(game, this.getPlayerBet(game.player), value))
      return;

    this.apiConsumer.addChip(
      game.gameId, 
      value, 
      this.addChipSuccess.bind(this), 
      this.apiCallError.bind(this));
  }

  addChipSuccess(response: any) {
    this.showAllCards(response);
    this.showAllChips(response);
    this.saveGame(response);
  }

  alertMessageVisible() {
    return this.alertInterval > 0 && this.alertVisible;
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
    this.apiConsumer.anteUp(
      this.getGame().gameId,
      this.anteUpSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCallClick() {
    this.apiConsumer.call(
      this.getGame().gameId,
      this.callSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCheckClick() {
    this.apiConsumer.check(
      this.getGame().gameId,
      this.checkSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDealClick() {
    this.apiConsumer.deal(
      this.getGame().gameId,
      this.dealSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDrawClick() {
    var game = this.getGame();
    var cards = game.player.cards as any[];

    this.apiConsumer.draw(
      game.gameId,
      cards.filter(item => !item.hold),
      this.drawSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnFoldClick() {
    this.apiConsumer.fold(
      this.getGame().gameId,
      this.foldSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnNextClick() {
    this.apiConsumer.next(
      this.getGame().gameId,
      this.nextSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRaiseClick() {
    this.apiConsumer.raise(
      this.getGame().gameId,
      this.getPlayerBet(this.getGame().player),
      this.raiseSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRetireClick() {
    this.router.navigate(["/"]);
  }

  btnShowdownClick() {
    this.apiConsumer.showdown(
      this.getGame().gameId,
      this.showdownSuccess.bind(this),
      this.apiCallError.bind(this)
    );
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

    var game = this.getGame();

    if (!this.validator.validateHold(game))
      return;

    this.apiConsumer.hold(
      game.gameId,
      rank,
      suit,
      this.cardClickSuccess.bind(this),
      this.apiCallError.bind(this)
    );
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

  getHand() {
    return this.getGame().hand.toString().padStart(3, '0');
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

  playerBetVisible() {
    return this.isAnte() || this.isBet() || this.renderAnteUpVisible();
  }

  playerWinningsVisible(player: any) {
    return player.winnings >= 0;
  }

  raiseSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }

  removeChipClick(value: number) {

    var game = this.getGame();

    if (!this.validator.validateRemoveChip(game))
      return;

    this.apiConsumer.removeChip(
      game.gameId,
      value,
      this.removeChipSuccess.bind(this),
      this.apiCallError.bind(this)
    );
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

    game.player.chips = JSON.parse(JSON.stringify(game.anteChips));
    game.aiPlayer.chips = JSON.parse(JSON.stringify(game.anteChips));

    var index = 0;
    var renderAIPlayer = false;

    this.renderAnteUpInterval = setInterval(() => {
      if (!renderAIPlayer) {
        if (index >= game.anteChips.length) {        
          index = 0;
          renderAIPlayer = true;
        }
        else {
          game.player.chips[index++].visible = true;
          this.saveGame(game);
        }
      }
      else {
        if (index >= game.anteChips.length) {
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

  renderAnteUpVisible() {
    return this.renderAnteUpInterval > 0;
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

  subTitleVisible() {
    return this.alertInterval == 0;
  }

  showdownSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }
}