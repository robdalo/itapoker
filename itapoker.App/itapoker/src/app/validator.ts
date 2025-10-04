import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Validator {

  addChipOK(
    game: any, 
    ui: any,
    playerBet: number, 
    value: number) {

    // chips can only be added when the game is unlocked

    if (ui.locked)
      return false;

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
  
  holdOK(game: any, ui: any) {

    // cards can only be held when the game is unlocked

    if (ui.locked)
      return false;

    // cards can only be held during draw
    return game.stage == 5;
  }

  removeChipOK(game: any, ui: any) {

    // chips can only be removed when the game is unlocked

    if (ui.locked)
      return false;

    // chips can only be removed during pre draw and post draw betting
    
    if (game.stage != 4 && game.stage != 6)
      return false;

    return true;
  }

  singlePlayerOK(playerName: string) {
    
    // player name cannot be null or empty

    if (!playerName)
      return false;

    // player name cannot be white space only

    if (playerName.trim().length < 1)
      return false;

    // player name cannot be greater than 10 characters

    if (playerName.length > 10)
      return false;

    return true;
  }
}