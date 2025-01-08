import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseURL = environment.apiURL;
  members = signal<Member[]>([]);

  getMembers() {
    return this.http.get<Member[]>(this.baseURL + 'user').subscribe({
      next: (members) => this.members.set(members),
    });
  }

  getMember(userName: string) {
    const member = this.members().find((x) => x.userName === userName);
    if (member !== undefined) return of(member); //return member will get an error, we need to return an observable

    return this.http.get<Member>(this.baseURL + 'user/' + userName);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseURL + 'user', member).pipe(
      tap({
        next: () =>
          this.members.update((members) =>
            members.map((m) => (m.userName === member.userName ? member : m))
          ),
      })
    );
  }
}
