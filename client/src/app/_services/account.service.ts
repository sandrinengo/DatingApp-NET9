import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_models/user.model';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  //#region Properties
  // Inject HTTP service
  private http = inject(HttpClient);
  baseURL = 'https://localhost:5001/api/';
  currentUser = signal<User | null>(null); //(null) is an initial value
  username: string = '';
  //#endregion

  //#region Methods
  userLogin(model: User) {
    return this.http.post<User>(this.baseURL + 'account/login', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('userKey', JSON.stringify(user));
          this.currentUser.set(user);
          this.username = model.Username;
        }
      })
    );
  }

  userLogout() {
    localStorage.removeItem('userKey');
    this.currentUser.set(null);
    this.username = '';
  }

  userRegister(model: User) {
    return this.http.post<User>(this.baseURL + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('userKey', JSON.stringify(user));
          this.currentUser.set(user);
          this.username = model.Username;
        }
        return user;
      })
    );
  }
  //#endregion
}
