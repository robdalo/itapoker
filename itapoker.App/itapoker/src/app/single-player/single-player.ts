import { ApiConsumer } from '../api-consumer';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GameEngine } from '../game-engine';
import { Router, RouterModule } from '@angular/router';
import { Validator } from '../validator';

@Component({
  selector: 'app-single-player',
  imports: [FormsModule, RouterModule],
  templateUrl: './single-player.html',
  styleUrl: './single-player.css'
})
export class SinglePlayer {

  ui = {
    playerName: ""
  };

  constructor(
    private apiConsumer: ApiConsumer,
    private gameEngine: GameEngine,
    private router: Router,
    private validator: Validator) {}

  startGameClick() {

    if (!this.validator.singlePlayerOK(this.ui.playerName))
      return;

    this.apiConsumer.singlePlayer(
      this.ui.playerName,
      this.singlePlayerSuccess.bind(this),
      this.singlePlayerError.bind(this));
  }

  singlePlayerError(error: any) {
  }

  singlePlayerSuccess(response: any) {
    this.gameEngine.saveGame(response);
    this.router.navigate(['/card-table']);
  }
}