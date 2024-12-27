import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { User } from '../_models/user.model';
import { AccountService } from '../_services/account.service';
import { GlobalService } from '../_services/global.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    FormsModule,
    BsDropdownModule,
    RouterLink,
    RouterLinkActive,
    TitleCasePipe,
  ],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  //#region Variables
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  //isAuthenticated = false; we dont need to use this any more, we will be using our signal
  //model: any = {};
  model: User = new User();
  username: string | null = '';
  //#endregion

  //#region Constructors
  constructor(public globalService: GlobalService) {
    //this.username = this.accountService.username;
  }
  //#endregion

  userLogin() {
    this.accountService.userLogin(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');
      },
      error: (error) => this.toastr.error(error.error),
    });
  }

  userLogout() {
    this.accountService.userLogout();
    this.router.navigateByUrl('/');
  }
}
