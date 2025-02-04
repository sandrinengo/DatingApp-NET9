import { NgFor } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';
import { NgxSpinnerComponent } from 'ngx-spinner';

declare var CapitalizeFirstWord: any; // Declare the function from external.js

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NgFor,
    NavComponent,
    HomeComponent,
    NgxSpinnerComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  //#region Private Variables
  private accountService = inject(AccountService);
  //#endregion

  //#region Methods

  setCurrentUser() {
    const userString = localStorage.getItem('userKey');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
    this.accountService.username = user.username;
  }
  //#endregion

  //#endregion Events
  ngOnInit(): void {
    this.setCurrentUser();
  }
  //#endregion
}
