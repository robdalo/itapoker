import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

declare var window: any;

const { ipcRenderer } = window.require ? window.require('electron') : {};

@Component({
  selector: 'app-main-menu',
  imports: [CommonModule, RouterModule],
  templateUrl: './main-menu.html',
  styleUrl: './main-menu.css'
})
export class MainMenu {

  btnQuitClick() {
    
    if (ipcRenderer) {
      ipcRenderer.send('quit-app');
    }
  }
}
