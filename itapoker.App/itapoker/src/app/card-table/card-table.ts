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

  game: any;

  ui = {
    locked: false,
    render: {
      alert: {        
        interval: 0,
        message: "",
        visible: false,
        enabled() { return this.interval > 0 && this.visible; }
      },
      anteUp: {
        interval: 0,
        enabled() { return this.interval > 0; }
      },
      deal: {
        interval: 0
      }
    }
  };

  constructor(
    private apiConsumer: ApiConsumer,
    protected gameEngine: GameEngine,
    private router: Router,
    private validator: Validator) {
      this.game = this.gameEngine.getGame();
  }

  addChipClick(value: number) {

    if (!this.validator.validateAddChip(
      this.game, 
      this.ui,
      this.gameEngine.getPlayerBet(this.game.player), 
      value))
      return;

    this.apiConsumer.addChip(
      this.game.gameId, 
      value, 
      this.addChipSuccess.bind(this), 
      this.apiCallError.bind(this));
  }

  addChipSuccess(response: any) {
    this.showAllCards(response);
    this.showAllChips(response);
    this.saveGame(response);
  }

  anteUpSuccess(response: any) {
    this.renderAnteUp();
    var wait = setInterval(() => {
      if (this.ui.render.anteUp.interval == 0) {
        clearInterval(wait);
        this.saveGame(response);
        this.renderAlert(response.alert);
        this.unlockUI();
      }    
    }, 500);
  }

  apiCallError(error: any) {
    this.renderAlert(error);
    this.unlockUI();
  }

  btnAnteClick() {
    this.lockUI();
    this.apiConsumer.anteUp(
      this.game.gameId,
      this.anteUpSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCallClick() {
    this.apiConsumer.call(
      this.game.gameId,
      this.callSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCheckClick() {
    this.apiConsumer.check(
      this.game.gameId,
      this.checkSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDealClick() {
    this.lockUI();
    this.apiConsumer.deal(
      this.game.gameId,
      this.dealSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDrawClick() {
    var cards = this.game.player.cards as any[];
    this.apiConsumer.draw(
      this.game.gameId,
      cards.filter(item => !item.hold),
      this.drawSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnFoldClick() {
    this.apiConsumer.fold(
      this.game.gameId,
      this.foldSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnNextClick() {
    this.apiConsumer.next(
      this.game.gameId,
      this.nextSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRaiseClick() {
    this.apiConsumer.raise(
      this.game.gameId,
      this.gameEngine.getPlayerBet(this.game.player),
      this.raiseSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRetireClick() {
    this.router.navigate(["/"]);
  }

  btnShowdownClick() {
    this.apiConsumer.showdown(
      this.game.gameId,
      this.showdownSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  callSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);
  }

  cardClick(rank: number, suit: number) {

    if (!this.validator.validateHold(this.game, this.ui))
      return;

    this.apiConsumer.hold(
      this.game.gameId,
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

  checkSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);    
  }  

  clearAlert() {
      clearInterval(this.ui.render.alert.interval);
      this.ui.render.alert.interval = 0;
      this.ui.render.alert.message = "";
      this.ui.render.alert.visible = false;      
  }

  dealSuccess(response: any) {
    this.saveGame(response);
    this.renderDeal();
    var wait = setInterval(() => {
      if (this.ui.render.deal.interval == 0) {
        clearInterval(wait);
        this.renderAlert(response.alert);
        this.unlockUI();
      }    
    }, 500);
  }

  drawSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);
  }

  foldSuccess(response: any) {
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }

  lockUI() {
    this.ui.locked = true;
  }

  nextSuccess(response: any) {
    this.saveGame(response);
    this.renderAlert(response.alert);
  }

  raiseSuccess(response: any) {
    this.showAllCards(response);
    this.saveGame(response);
    this.renderAlert(response.alert);      
  }

  removeChipClick(value: number) {

    if (!this.validator.validateRemoveChip(this.game, this.ui))
      return;

    this.apiConsumer.removeChip(
      this.game.gameId,
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
    this.ui.render.alert.message = message;

    this.ui.render.alert.interval = setInterval(() => {
      this.ui.render.alert.visible = !this.ui.render.alert.visible;
    }, 1000);

    setTimeout(() => {
      this.clearAlert();
    }, 10000);
  }

  renderAnteUp() {
    var pot = this.game.pot;

    this.game.pot = 0;
    this.saveGame(this.game);

    this.game.player.chips = JSON.parse(JSON.stringify(this.game.anteChips));
    this.game.aiPlayer.chips = JSON.parse(JSON.stringify(this.game.anteChips));

    var index = 0;
    var renderAIPlayer = false;

    this.ui.render.anteUp.interval = setInterval(() => {
      if (!renderAIPlayer) {
        if (index >= this.game.anteChips.length) {        
          index = 0;
          renderAIPlayer = true;
        }
        else {
          this.game.player.chips[index++].visible = true;
          this.saveGame(this.game);
        }
      }
      else {
        if (index >= this.game.anteChips.length) {
          clearInterval(this.ui.render.anteUp.interval);
          this.ui.render.anteUp.interval = 0;
          this.game.player.chips = [];
          this.game.aiPlayer.chips = [];
          this.game.pot = pot;
          this.saveGame(this.game);
        }
        else {
          this.game.aiPlayer.chips[index++].visible = true;
          this.saveGame(this.game);
        }       
      }
    }, 1000);
  }

  renderDeal() {
    var index = 0;
    var reveal = false;
    this.ui.render.deal.interval = setInterval(() => {
      if (!reveal) {
        if (index >= this.game.player.cards.length) {
          reveal = true;
          index--;
        }
        else {
          this.game.player.cards[index++].visible = true;
          this.saveGame(this.game);
        }
      }
      else {
        if (index < 0) {
          clearInterval(this.ui.render.deal.interval);
          this.ui.render.deal.interval = 0;
        }
        else {
          this.game.player.cards[index--].reveal = true;
          this.saveGame(this.game);
        }
      }
    }, 500);
  }

  saveGame(response: any) {
    this.gameEngine.saveGame(response);
    this.game = this.gameEngine.getGame();
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

  subTitleEnabled() {
    return this.ui.render.alert.interval == 0;
  }

  unlockUI() {
    this.ui.locked = false;
  }
}