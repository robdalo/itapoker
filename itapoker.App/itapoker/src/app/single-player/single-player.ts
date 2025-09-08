import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-single-player',
  imports: [FormsModule],
  templateUrl: './single-player.html',
  styleUrl: './single-player.css'
})
export class SinglePlayer {
  playerName = String();
  cash = String();
  ante = String();
  limit = String();
}
