import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Validator {

  validateAddChip(game: any, playerBet: number, value: number) {

    // chips can only be added during pre draw and post draw betting

    if (game.stage != 4 && game.stage != 6)
      return false;

    // chips can only be added up to the game limit

    if (playerBet + value > game.limit)
      return false;

    // chips can only be added up to available cash

    if (value > game.player.cash)
      return false;

    return true;
  }  
  
  validateHold(game: any) {
    // cards can only be held during draw
    return game.stage == 5;
  }

  validateRemoveChip(game: any) {

    // chips can only be removed during pre draw and post draw betting
    
    if (game.stage != 4 && game.stage != 6)
      return false;

    return true;
  }  
}