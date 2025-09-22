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
  cashPlayer = String();

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
    this.cashPlayer = game.player.cash;
  }

  addChipClick(number: number) {
    this.chipsPlayer[number]++;
  }

  btnAnteClick() {

  }

  cardClick(number: number) {
    this.holdPlayer[number] = !this.holdPlayer[number];
  }

  getGame() {
    var json = localStorage.getItem("game");
    return json ? JSON.parse(json) : null;
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

  removeChipClick(number: number) {
    this.chipsPlayer[number]--;
  }  
}