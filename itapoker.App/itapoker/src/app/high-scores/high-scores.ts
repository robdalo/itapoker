import { ApiConsumer } from '../api-consumer';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-high-scores',
  imports: [CommonModule, RouterModule],
  templateUrl: './high-scores.html',
  styleUrl: './high-scores.css'
})
export class HighScores {
  
  highScores: any = [];

  constructor(private apiConsumer: ApiConsumer) {}

  ngOnInit() {
    this.apiConsumer.getHighScores(
      this.getHighScoresSuccess.bind(this), 
      this.getHighScoresError.bind(this));
  }

  getHighScoresError(error: any) {
  }

  getHighScoresSuccess(response: any) {
    this.highScores = response;
  }
}