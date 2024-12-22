import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterComponent } from '../register/register.component';
import { User } from '../_models/user.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  //#region Public Variables
  registerMode = false;
  http = inject(HttpClient);
  users: User = new User();
  //users: any;
  //#endregion

  //#region Methods
  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get<User>('https://localhost:5001/api/user').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  ngOnInit(): void {
    this.getUsers();
  }
  //#endregion
}
