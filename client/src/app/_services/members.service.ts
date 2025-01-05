import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseURL = environment.apiURL;

  getMembers() {
    return this.http.get<Member[]>(this.baseURL + 'user');
  }

  getMember(userName: string) {
    return this.http.get<Member>(this.baseURL + 'user/' + userName);
  }
}
