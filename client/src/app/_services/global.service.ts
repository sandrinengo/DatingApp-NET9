import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root', // This makes the service available globally (singleton)
})
export class GlobalService {
  capitalizeFirstWord(text: string) {
    if (!text) {
      return ''; // Handle empty string case
    }
    return text.charAt(0).toUpperCase() + text.slice(1);
  }
}
