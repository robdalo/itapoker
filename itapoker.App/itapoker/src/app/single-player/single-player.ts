import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-single-player',
  imports: [FormsModule, RouterModule],
  templateUrl: './single-player.html',
  styleUrl: './single-player.css'
})
export class SinglePlayer {
  playerName = String();
  cash = String();
  ante = String();
  limit = String();

  constructor(
    private http: HttpClient,
    private router: Router) {
  }

  startGameClick() {

    var request = {
      PlayerName: this.playerName
    };

    this.http.post("http://localhost:5174/game/singleplayer", request).subscribe({
      next: value => this.startGameSuccess(value),
      error: err => this.startGameError(err),
      complete: () => {}
    });
  }

  startGameSuccess(response: any) {
    localStorage.setItem("game", JSON.stringify(response));
    this.router.navigate(['/card-table']);
  }

  startGameError(error: any) {
  }
}
