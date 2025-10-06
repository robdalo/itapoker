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
    hand: {
      aiPlayer: {
        visible: false
      },
      player: {
        visible: true
      }
    },
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
      bet: {
        interval: 0,
        enabled() { return this.interval > 0; }
      },
      deal: {
        interval: 0
      },
      draw: {
        interval: 0
      }
    }
  };

  constructor(
    private apiConsumer: ApiConsumer,
    private gameEngine: GameEngine,
    private router: Router,
    private validator: Validator) {}

  ngOnInit() {
    this.game = this.gameEngine.getGame();
  }

  addChipClick(value: number) {

    if (!this.validator.addChipOK(
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
    this.showAllCards(response.player.cards);
    this.showAllChips(response);
    this.saveGame(response);
  }

  aiPlayerBetEnabled() {
    return this.ui.render.anteUp.enabled();
  }

  aiPlayerHandClick() {

    if (!this.validator.playerHandClick(this.game))
      return;

    this.ui.hand.aiPlayer.visible = true;
    this.ui.hand.player.visible = false;
  }

  aiPlayerHandEnabled() {
    return this.ui.hand.aiPlayer.visible &&
           this.gameEngine.aiPlayerHandEnabled(this.game);
  }

  aiPlayerLastBetEnabled() {
    return (
      this.gameEngine.drawEnabled(this.game) ||
      this.gameEngine.showdownEnabled(this.game) || (
        this.gameEngine.playerBetEnabled(this.game) && 
       !this.ui.render.anteUp.enabled()
      ) || (
        this.gameEngine.gameOverEnabled(this.game) &&
        this.game.aiPlayer.lastBetType == 3
      ));
  }

  aiPlayerWinningsEnabled() {
    return this.gameEngine.playerWinningsEnabled(this.game.aiPlayer);
  }

  anteUpEnabled() {
    return this.gameEngine.anteUpEnabled(this.game);
  }

  anteUpSuccess(response: any) {
    this.renderAnteUp(response);
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
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.anteUp(
      this.game.gameId,
      this.anteUpSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCallClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.call(
      this.game.gameId,
      this.callSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnCheckClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.check(
      this.game.gameId,
      this.checkSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDealClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.deal(
      this.game.gameId,
      this.dealSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnDrawClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.draw(
      this.game.gameId,
      (this.game.player.cards as any[]).filter(item => !item.hold),
      this.drawSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnFoldClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.fold(
      this.game.gameId,
      this.foldSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnNextClick() {
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.next(
      this.game.gameId,
      this.nextSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  btnRaiseClick() {
    this.clearAlert();
    this.lockUI();
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
    this.clearAlert();
    this.lockUI();
    this.apiConsumer.showdown(
      this.game.gameId,
      this.showdownSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  callEnabled() {
    return this.gameEngine.callEnabled(this.game);
  }

  callSuccess(response: any) {
    this.showAllCards(response.player.cards);
    this.saveGame(response);
    this.renderAlert(response.alert);
    this.unlockUI();
  }

  cardClick(rank: number, suit: number) {

    if (!this.validator.holdOK(this.game, this.ui))
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
    this.showAllCards(response.player.cards);
    this.saveGame(response);
  }

  checkEnabled() {
    return this.gameEngine.checkEnabled(this.game);
  }

  checkSuccess(response: any) {
    this.showAllCards(response.player.cards);
    this.renderBet(response);
    var wait = setInterval(() => {
      if (this.ui.render.bet.interval == 0) {
        clearInterval(wait);
        this.saveGame(response);
        this.renderAlert(response.alert);
        this.unlockUI();
      }    
    }, 500);
  }  

  clearAlert() {
      clearInterval(this.ui.render.alert.interval);
      this.ui.render.alert.interval = 0;
      this.ui.render.alert.message = "";
      this.ui.render.alert.visible = false;      
  }

  dealEnabled() {
    return this.gameEngine.dealEnabled(this.game);
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

  drawEnabled() {
    return this.gameEngine.drawEnabled(this.game);
  }

  drawSuccess(response: any) {
    this.showAllCards(response.player.cards);
    this.renderDraw(response);
    var wait = setInterval(() => {
      if (this.ui.render.draw.interval == 0) {
        clearInterval(wait);
        this.saveGame(response);
        this.renderAlert(response.alert);
        this.unlockUI();
      }
    }, 500);
  }

  foldEnabled() {
    return this.gameEngine.foldEnabled(this.game);
  }

  foldSuccess(response: any) {
    this.saveGame(response);
    this.renderAlert(response.alert);
    this.unlockUI();
  }

  gameOverEnabled() {
    return this.gameEngine.gameOverEnabled(this.game);
  }

  getAIPlayerBet() {
    return this.gameEngine.getPlayerBet(this.game.aiPlayer);
  }

  getCardUrl(card: any) {
    return this.gameEngine.getCardUrl(card);
  }

  getHand() {
    return this.gameEngine.getHand(this.game);
  }

  getPlayerBet() {
    return this.gameEngine.getPlayerBet(this.game.player);
  }

  lockUI() {
    this.ui.locked = true;
  }

  nextSuccess(response: any) {
    this.saveGame(response);
    this.resetHandVisibility();
    this.renderAlert(response.alert);
    this.unlockUI();
  }

  playerBetEnabled() {
    return this.gameEngine.playerBetEnabled(this.game);
  }

  playerHandClick() {
    
    if (!this.validator.playerHandClick(this.game))
      return;

    this.ui.hand.aiPlayer.visible = false;
    this.ui.hand.player.visible = true;
  }

  playerHandEnabled() {
    return this.ui.hand.player.visible &&
           this.gameEngine.playerHandEnabled(this.game);
  }

  playerLastBetEnabled() {
    return this.gameEngine.playerLastBetEnabled(this.game);
  }

  playerWinningsEnabled() {
    return this.gameEngine.playerWinningsEnabled(this.game.player);
  }

  raiseEnabled() {
    return this.gameEngine.raiseEnabled(this.game);
  }

  raiseSuccess(response: any) {
    this.showAllCards(response.player.cards);
    this.renderBet(response);
    var wait = setInterval(() => {
      if (this.ui.render.bet.interval == 0) {
        clearInterval(wait);
        this.saveGame(response);
        this.renderAlert(response.alert);
        this.unlockUI();
      }    
    }, 500);
  }

  removeChipClick(value: number) {

    if (!this.validator.removeChipOK(this.game, this.ui))
      return;

    this.apiConsumer.removeChip(
      this.game.gameId,
      value,
      this.removeChipSuccess.bind(this),
      this.apiCallError.bind(this)
    );
  }

  removeChipSuccess(response: any) {
    this.showAllCards(response.player.cards);
    this.showAllChips(response);
    this.saveGame(response);
  }

  renderAlert(message: any) {

    if (!message)
      return;

    this.ui.render.alert.message = message;

    this.ui.render.alert.interval = setInterval(() => {
      this.ui.render.alert.visible = !this.ui.render.alert.visible;
    }, 1000);

    setTimeout(() => {
      this.clearAlert();
    }, 10000);
  }

  renderAnteUp(response: any) {
    
    this.game.player.chips = JSON.parse(JSON.stringify(response.player.lastBetChips));
    this.game.aiPlayer.chips = JSON.parse(JSON.stringify(response.aiPlayer.lastBetChips));

    var playerChips = this.game.player.chips as any[];
    var aiPlayerChips = this.game.aiPlayer.chips as any[];

    var renderAIPlayer = false;

    this.ui.render.anteUp.interval = setInterval(() => {
      if (!renderAIPlayer) {
        if (playerChips.filter(x => !x.visible).length == 0) {
          renderAIPlayer = true;
        }
        else {
          playerChips[0].visible = true;
        }
      }
      else {
        if (aiPlayerChips.filter(x => !x.visible).length == 0) {
          clearInterval(this.ui.render.anteUp.interval);
          this.ui.render.anteUp.interval = 0;
          this.game.player.chips = [];
          this.game.aiPlayer.chips = [];
        }
        else {
          aiPlayerChips[0].visible = true;
        }
      }
    }, 1000);
  }

  renderBet(response: any) {

    this.game.aiPlayer.chips = JSON.parse(JSON.stringify(response.aiPlayer.lastBetChips));
  
    var index = 0;
    
    this.ui.render.bet.interval = setInterval(() => {
      if (index >= this.game.aiPlayer.chips.length) {
        clearInterval(this.ui.render.bet.interval);
        this.ui.render.bet.interval = 0;
      }
      else {
        this.game.aiPlayer.chips[index++].visible = true;
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
        }
      }
      else {
        if (index < 0) {
          clearInterval(this.ui.render.deal.interval);
          this.ui.render.deal.interval = 0;
        }
        else {
          this.game.player.cards[index--].reveal = true;
        }
      }
    }, 500);
  }

  renderDraw(response: any) {

    var discarded: any[] = [];
    var replaced: any[] = [];
    var cards = this.game.player.cards as any[];
    var cardsPostDraw = response.player.cards as any[];

    cards.forEach(x => {
      if (cardsPostDraw.filter(y => y.rank == x.rank && y.suit == x.suit).length == 0)
        discarded.push(x);
    });

    cardsPostDraw.forEach(x => {
      if (cards.filter(y => y.rank == x.rank && y.suit == x.suit).length == 0)
        replaced.push(x);
    });

    this.ui.render.draw.interval = setInterval(() => {
      if (discarded.length > 0) {
        var discard = discarded.pop();
        var replace = replaced.pop(); 
        var existing = cards.filter(x => x.rank == discard.rank && x.suit == discard.suit)[0];
        
        existing.visible = false;
        existing.reveal = false;
        existing.rank = replace.rank;
        existing.suit = replace.suit;
        existing.title = replace.title;
        existing.hold = replace.hold;
        existing.url = replace.url;
      }
      else if (cards.filter(x => !x.visible).length > 0) {
        cards.filter(x => !x.visible)[0].visible = true;
      }
      else if (cards.filter(x => !x.reveal).length > 0) {
        cards.filter(x => !x.reveal)[0].reveal = true;
      }
      else {
        clearInterval(this.ui.render.draw.interval);
        this.ui.render.draw.interval = 0;
      }
    }, 500);
  }

  resetHandVisibility() {
    this.ui.hand.aiPlayer.visible = false;
    this.ui.hand.player.visible = true;    
  }

  saveGame(response: any) {
    this.game = this.gameEngine.saveGame(response);
  }

  showAllCards(cards: any[]) {
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

  showdownEnabled() {
    return this.gameEngine.showdownEnabled(this.game);
  }

  showdownSuccess(response: any) {
    this.showAllCards(response.aiPlayer.cards);
    this.showAllCards(response.player.cards);
    this.saveGame(response);
    this.aiPlayerHandClick();
    this.renderAlert(response.alert);
    this.unlockUI();
  }

  subTitleEnabled() {
    return this.ui.render.alert.interval == 0;
  }

  unlockUI() {
    this.ui.locked = false;
  }
}