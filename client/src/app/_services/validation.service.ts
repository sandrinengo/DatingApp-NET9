import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ValidationService {
  private http = inject(HttpClient);
  private messages: Record<string, Record<string, string>> = {};
  private jsonUrl = '../assets/validation-messages.json';

  /**
   *
   */
  constructor() {
    this.loadMessages();
  }

  private getValidationMessages(): Observable<any> {
    return this.http.get<any>(this.jsonUrl); // Fetch the JSON file
  }

  // Load the JSON file and store the nested messages
  private loadMessages(): void {
    this.http
      .get<Record<string, Record<string, string>>>(this.jsonUrl)
      .subscribe((data) => {
        this.messages = data;
      });
  }

  // Retrieve a specific validation message by field and validator type
  getValidationMessage(messsageFor: string): { name: string; message: string } {
    let result: { name: string; message: string } = { name: '', message: '' };
    let arrMessage = messsageFor.split('-');
    const message = this.messages[arrMessage[0]]?.[arrMessage[1]];

    if (message !== undefined)
      result = {
        name: [arrMessage[1]].toString(),
        message: message,
      };

    return result;
  }
}

// (data) => {
//   //console.log(data[arrMessage[0]][arrMessage[1]]);
//   message = data[arrMessage[0]][arrMessage[1]];
//   index++;
// });
