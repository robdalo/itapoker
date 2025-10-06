import { CardTable } from './card-table/card-table';
import { HighScores } from './high-scores/high-scores';
import { MainMenu } from './main-menu/main-menu';
import { Routes } from '@angular/router';
import { Settings } from './settings/settings';
import { SinglePlayer } from './single-player/single-player';

export const routes: Routes = [
    { path: '', component: MainMenu },    
    { path: 'card-table', component: CardTable },
    { path: 'high-scores', component: HighScores },
    { path: 'settings', component: Settings },
    { path: 'single-player', component: SinglePlayer },
];