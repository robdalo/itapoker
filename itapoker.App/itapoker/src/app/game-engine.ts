import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GameEngine {

  aiPlayerHandEnabled(game: any) {
    // ai player hand is only enabled when game stage is game over
    return game.stage == 8;
  }  

  anteUpEnabled(game: any) {
    // ante up is only enabled when the game stage is ante up
    return game.stage == 2;
  }

  betEnabled(game: any) {
    // bet is only enabled when game stage is bet pre draw 
    // or bet post draw
    return game.stage == 4 || game.stage == 6;
  }

  callEnabled(game: any) {
    // call is only enabled when betting is enabled,
    // ai player raised in last bet and player bet is zero
    return this.betEnabled(game) &&
           game.aiPlayer.lastBetType == 4 &&
           this.getPlayerBet(game.player) == 0;
  }

  checkEnabled(game: any) {
    // check is only enabled when betting is enabled,
    // ai player has not previously placed a bet and
    // player bet is zero
    return this.betEnabled(game) &&
           this.getPlayerBet(game.player) == 0 &&
           game.aiPlayer.lastBetType == 0;
  }

  dealEnabled(game: any) {
    // deal is only enabled when game stage is deal
    return game.stage == 3;
  }

  drawEnabled(game: any) {
    // draw is only enabled when game stage is draw
    return game.stage == 5;
  }

  foldEnabled(game: any) {
    // fold is only enabled when betting is enabled and
    // player bet is zero
    return this.betEnabled(game) &&
           this.getPlayerBet(game.player) == 0;
  }

  gameOverEnabled(game: any) {
    // game over is only enabled when game stage is game over
    return game.stage == 8;
  }

  getCardUrl(card: any) {
    return card.reveal ? card.url : "images/cards/back.png";
  }

  getGame() {
    var json = localStorage.getItem("game");
    return json ? JSON.parse(json) : null;
  }

  getHand(game: any) {
    return game.hand.toString().padStart(3, '0');
  }

  getPlayerBet(player: any) {
    var bet = 0;
    var chips = player.chips as any[];

    chips.filter(x => x.visible).forEach(x => {
      bet += x.total;
    });

    return bet;
  }

  playerBetEnabled(game: any) {
    // player bet is only enabled when ante up is enabled or
    // betting is enabled
    return this.anteUpEnabled(game) || 
           this.betEnabled(game);
  }

  playerHandEnabled(game: any) {
    // player hand is only enabled when game stage has 
    // progressed past deal, but has not reached game over
    return game.stage > 3;
  }

  playerLastBetEnabled(game: any) {
    // player last bet is only enabled when game stage is game over
    // and player has folded
    return game.stage == 8 &&
           game.player.lastBetType == 3;
  }

  playerWinningsEnabled(player: any) {
    // player winnings is only enabled when player winnings 
    // is greater than zero
    return player.winnings >= 0;
  }

  raiseEnabled(game: any) {
    // raise is only enabled when betting is enabled, player bet
    // is greater than zero, ai player has not previously placed
    // a bet or ai player raised in last bet
    return this.betEnabled(game) &&
           this.getPlayerBet(game.player) > 0 && (
              game.aiPlayer.lastBetType == 0 ||
              game.aiPlayer.lastBetType == 4);
  }

  saveGame(game: any) {
    localStorage.setItem("game", JSON.stringify(game));
    return this.getGame();
  }  

  showdownEnabled(game: any) {
    // showdown is only enabled when game stage is showdown
    return game.stage == 7;
  }
}