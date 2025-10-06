import { ApiConsumer } from '../api-consumer';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  imports: [FormsModule],
  templateUrl: './settings.html',
  styleUrl: './settings.css'
})
export class Settings {

  ui = {
    baseUrl: ""
  };

  constructor(
    private apiConsumer: ApiConsumer,
    private router: Router) {}

  ngOnInit() {
    this.ui.baseUrl = this.apiConsumer.getBaseUrl();
  }

  btnSaveClick() {
    this.apiConsumer.saveBaseUrl(this.ui.baseUrl);
    this.router.navigate(['/']);
  }
}
