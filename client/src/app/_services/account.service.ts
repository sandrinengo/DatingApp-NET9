import { HttpClient } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";
import { User } from "../_models/user.model";
import { map } from "rxjs";
import { environment } from "../../environments/environment";
import { LikesService } from "./likes.service";

@Injectable({
	providedIn: "root",
})
export class AccountService {
	//#region Properties
	// Inject HTTP service
	private http = inject(HttpClient);
	private likeService = inject(LikesService);
	baseURL = environment.apiURL;
	currentUser = signal<User | null>(null); //(null) is an initial value
	username: string = "";
	//#endregion

	//#region Methods
	userLogin(model: User) {
		let user = new User();
		return this.http.post<any>(this.baseURL + "account/login", model).pipe(
			map(response => {
				if (response) {
					user = response;
					this.setCurrentUser(user);
					this.username = user.username;
				}
			})
		);
	}

	setCurrentUser(user: User) {
		localStorage.setItem("userKey", JSON.stringify(user));
		this.currentUser.set(user);
		this.likeService.getLikeIDs(); // getLikeIDs function which updates the Signal which stores UserIDs
		this.username = user.username;
	}

	userLogout() {
		localStorage.removeItem("userKey");
		this.currentUser.set(null);
		this.username = "";
	}

	userRegister(model: User) {
		return this.http.post<User>(this.baseURL + "account/register", model).pipe(
			map(user => {
				if (user) {
					this.setCurrentUser(user);
				}
				return user;
			})
		);
	}
	//#endregion
}
