import { Routes } from '@angular/router';
import { CardTable } from './card-table/card-table';
import { HighScores } from './high-scores/high-scores';
import { MainMenu } from './main-menu/main-menu';
import { SinglePlayer } from './single-player/single-player';

export const routes: Routes = [
    { path: 'card-table', component: CardTable },
    { path: 'high-scores', component: HighScores },
    { path: 'main-menu', component: MainMenu },
    { path: 'single-player', component: SinglePlayer },
];
