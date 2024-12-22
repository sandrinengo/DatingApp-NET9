import {
  Component,
  inject,
  output,
  AfterViewInit,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../_models/user.model';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements AfterViewInit {
  @ViewChild('txtUserName') inputElement!: ElementRef<HTMLInputElement>;

  private accountService = inject(AccountService);
  model: User = new User();
  cancelRegisteOutput = output<boolean>();

  userRegister() {
    this.accountService.userRegister(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancelRegister();
      },
    });
  }

  cancelRegister() {
    this.cancelRegisteOutput.emit(false);
  }

  ngAfterViewInit(): void {
    this.inputElement.nativeElement.focus(); // Automatically focus the input after view initialization
  }
}
