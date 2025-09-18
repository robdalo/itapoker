import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-high-scores',
  imports: [CommonModule],
  templateUrl: './high-scores.html',
  styleUrl: './high-scores.css'
})
export class HighScores {
  
  highScores: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http
      .get<any[]>('http://localhost:5174/game/highscores')
      .subscribe(data => {
        this.highScores = data;
    });
  }
}
