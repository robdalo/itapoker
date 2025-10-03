import { ApiConsumer } from '../api-consumer';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GameEngine } from '../game-engine';
import { Router } from '@angular/router';
import { Validator } from '../validator';

@Component({
  selector: 'app-card-table',
  imports: [CommonModule],
  templateUrl: './card-table.html',
  styleUrl: './card-table.css'
})
export class CardTable {

  renderAlertSettings = {
    message: "",
    interval: 0,
    visible: false
  };

  renderAnteUpSettings = {
    interval: 0
  };

  renderDealSettings = {
    interval: 0
  };

  constructor(
    private apiConsumer: ApiConsumer,
    protected gameEngine: GameEngine,
    private router: Router,
    private validator: Validator) {
  }

  addChipClick(value: number) {

    var game = this.gameEngine.getGame();

    if (!this.validator.validateAddChip(game, this.gameEngine.getPlayerBet(game.player), value))
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
    this.gameEngine.saveGame(response);
  }

  anteUpSuccess(response: any) {
    
    this.renderAnteUp();
    
    var wait = setInterval(() => {
      if (this.renderAnteUpSettings.interval == 0) {
        clearInterval(wait);
        this.gameEngine.saveGame(response);
        this.renderAlert(response.alert);
      }    
    }, 500);
  }

  apiCallError(error: any) {
    this.renderAlert(error);
  }

  btnAnteClick() {
    this.apiConsumer.anteUp(
      this.gameEngine.getGame().gameId,
      this.anteUpSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCallClick() {
    this.apiConsumer.call(
      this.gameEngine.getGame().gameId,
      this.callSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCheckClick() {
    this.apiConsumer.check(
      this.gameEngine.getGame().gameId,
      this.checkSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDealClick() {
    this.apiConsumer.deal(
      this.gameEngine.getGame().gameId,
      this.dealSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDrawClick() {
    var game = this.gameEngine.getGame();
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
      this.gameEngine.getGame().gameId,
      this.foldSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnNextClick() {
    this.apiConsumer.next(
      this.gameEngine.getGame().gameId,
      this.nextSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRaiseClick() {
    this.apiConsumer.raise(
      this.gameEngine.getGame().gameId,
      this.gameEngine.getPlayerBet(this.gameEngine.getGame().player),
      this.raiseSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRetireClick() {
    this.router.navigate(["/"]);
  }

  btnShowdownClick() {
    this.apiConsumer.showdown(
      this.gameEngine.getGame().gameId,
      this.showdownSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  callSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);
  }

  checkSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);    
  }

  cardClick(rank: number, suit: number) {

    var game = this.gameEngine.getGame();

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
    this.gameEngine.saveGame(response);
  }

  clearAlert() {
      clearInterval(this.renderAlertSettings.interval);
      this.renderAlertSettings.interval = 0;
      this.renderAlertSettings.message = "";
      this.renderAlertSettings.visible = false;      
  }

  dealSuccess(response: any) {
    this.gameEngine.saveGame(response);
    this.renderDeal();

    var wait = setInterval(() => {
      if (this.renderDealSettings.interval == 0) {
        clearInterval(wait);
        this.renderAlert(response.alert);
      }    
    }, 500);
  }

  drawSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);
  }

  foldSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);      
  }

  getCardUrl(index: number) {

    var card = this.gameEngine.getGame().player.cards[index];
    
    if (!card.reveal)
      return "images/cards/back.png";

    return card.url;
  }

  nextSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);      
  }

  playerBetEnabled() {
    return this.gameEngine.playerBetEnabled(this.gameEngine.getGame()) || 
           this.renderAnteUpVisible();
  }

  raiseSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);      
  }

  removeChipClick(value: number) {

    var game = this.gameEngine.getGame();

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
    this.gameEngine.saveGame(response);
  }

  renderAlert(message: any) {

    if (!message)
      return;

    this.clearAlert();

    this.renderAlertSettings.message = message;

    this.renderAlertSettings.interval = setInterval(() => {
      this.renderAlertSettings.visible = !this.renderAlertSettings.visible;
    }, 1000);

    setTimeout(() => {
      this.clearAlert();
    }, 10000);
  }

  renderAlertVisible() {
    return this.renderAlertSettings.interval > 0 && this.renderAlertSettings.visible;
  }  

  renderAnteUp() {

    var game = this.gameEngine.getGame();

    var pot = game.pot;

    game.pot = 0;

    this.gameEngine.saveGame(game);

    game.player.chips = JSON.parse(JSON.stringify(game.anteChips));
    game.aiPlayer.chips = JSON.parse(JSON.stringify(game.anteChips));

    var index = 0;
    var renderAIPlayer = false;

    this.renderAnteUpSettings.interval = setInterval(() => {
      if (!renderAIPlayer) {
        if (index >= game.anteChips.length) {        
          index = 0;
          renderAIPlayer = true;
        }
        else {
          game.player.chips[index++].visible = true;
          this.gameEngine.saveGame(game);
        }
      }
      else {
        if (index >= game.anteChips.length) {
          clearInterval(this.renderAnteUpSettings.interval);
          this.renderAnteUpSettings.interval = 0;
          game.player.chips = [];
          game.aiPlayer.chips = [];
          game.pot = pot;
          this.gameEngine.saveGame(game);
        }
        else {
          game.aiPlayer.chips[index++].visible = true;
          this.gameEngine.saveGame(game);
        }       
      }
    }, 1000);
  }

  renderAnteUpVisible() {
    return this.renderAnteUpSettings.interval > 0;
  }

  renderDeal() {

    var game = this.gameEngine.getGame();

    var index = 0;
    var reveal = false;

    this.renderDealSettings.interval = setInterval(() => {

      if (!reveal) {
        if (index >= game.player.cards.length) {
          reveal = true;
          index--;
        }
        else {
          game.player.cards[index++].visible = true;
          this.gameEngine.saveGame(game);
        }
      }
      else {
        if (index < 0) {
          clearInterval(this.renderDealSettings.interval);
          this.renderDealSettings.interval = 0;
        }
        else {
          game.player.cards[index--].reveal = true;
          this.gameEngine.saveGame(game);
        }
      }
    }, 500);
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
    return this.renderAlertSettings.interval == 0;
  }

  showdownSuccess(response: any) {
    this.showAllCards(response);
    this.gameEngine.saveGame(response);
    this.renderAlert(response.alert);      
  }
}