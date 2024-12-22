import { NgFor } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';

declare var CapitalizeFirstWord: any; // Declare the function from external.js

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor, NavComponent, HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  //#region Private Variables
  private accountService = inject(AccountService);
  //#endregion

  //#region Constructors
  constructor() {}
  //#endregion

  //#region Methods

  setCurrentUser() {
    const userString = localStorage.getItem('userKey');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
    this.accountService.username = user.username;
  }
  //#endregion

  //#endregion Events
  ngOnInit(): void {
    this.setCurrentUser();
  }
  //#endregion
}
