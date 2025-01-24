import {
	HttpClient,
	HttpHeaders,
	HttpParams,
	HttpResponse,
} from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../environments/environment";
import { Member } from "../_models/member";
import { of, tap } from "rxjs";
import { Photo } from "../_models/photo";
import { PaginatedResult } from "../_models/pagination";
import { UserParams } from "../_models/userParams";
import { AccountService } from "./account.service";

@Injectable({
	providedIn: "root",
})
export class MembersService {
	private http = inject(HttpClient);
	private accountService = inject(AccountService);
	baseURL = environment.apiURL;
	paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
	memberCache = new Map();
	user = this.accountService.currentUser();
	userParams = signal<UserParams>(new UserParams(this.user)); // To remember the filter when user selects. We're going to use one type of Signal called model that we get from Angular core.
	// And this is a writable signal that is exposed as an input output pair on the containing directive.
	// What it gives us effectively is two way biding to a signal. So we'll use a model and we'll give it a type of user params, and we''' give it an inital value of UserParams

	resetUserParams() {
		this.userParams.set(new UserParams(this.user));
	}

	getMembers() {
		const response = this.memberCache.get(
			Object.values(this.userParams()).join("-")
		);

		if (response) return this.setPaginatedResponse(response);

		let params = this.setPaginationHeader(
			this.userParams().pageNumber,
			this.userParams().pageSize
		);
		params = params.append("minAge", this.userParams().minAge);
		params = params.append("maxAge", this.userParams().maxAge);
		params = params.append("gender", this.userParams().gender);
		params = params.append("orderBy", this.userParams().orderBy);
		return this.http
			.get<Member[]>(this.baseURL + "user", { observe: "response", params })
			.subscribe({
				next: response => {
					this.setPaginatedResponse(response);
					// So, the idea is that when the client calls the get members method here or our component does, then first
					// of all we check to see if we've already got this inside our member cache.
					// Every time we make a request, then we're effectively storing that inside our member cache.
					// So, if it's in there, it's been requested before and the page has not been refreshed. It should be available from there.
					// If we do have it, we're simply going to set our signal to that response which is inside paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
					// and our component will react to that.
					this.memberCache.set(
						Object.values(this.userParams()).join("-"),
						response
					);
				},
			});
	}

	getMember(userName: string) {
		const member: Member = [...this.memberCache.values()]
			.reduce((arr, element) => arr.concat(element.body), [])
			.find((x: Member) => x.userName === userName);
		if (member) return of(member); //return member will get an error, we need to return an observable

		return this.http.get<Member>(this.baseURL + "user/" + userName);
	}

	updateMember(member: Member) {
		return this.http
			.put(this.baseURL + "user", member)
			.pipe
			// tap({
			// 	next: () =>
			// 		this.members.update(members =>
			// 			members.map(m => (m.userName === member.userName ? member : m))
			// 		),
			// })
			();
	}

	setMainPhoto(photo: Photo) {
		return this.http
			.put(this.baseURL + "user/set-main-photo/" + photo.id, {})
			.pipe
			// tap({
			//   next: () =>
			//     this.members.update((members) =>
			//       members.map((m) => {
			//         if (m.photos.includes(photo)) {
			//           m.photoURL = photo.url;
			//         }
			//         return m;
			//       })
			//     ),
			// })
			(); //{} empty body
	}

	deletePhoto(photo: Photo) {
		return this.http
			.delete(this.baseURL + "user/delete-photo/" + photo.id)
			.pipe
			// tap({
			//   next: () =>
			//     this.members.update((members) =>
			//       members.map((m) => {
			//         if (m.photos.includes(photo)) {
			//           m.photos = m.photos.filter((x) => x.id !== photo.id);
			//         }
			//         return m;
			//       })
			//     ),
			// })
			();
	}

	private setPaginationHeader(pageNumber: number, pageSize: number) {
		let params = new HttpParams();

		if (pageNumber && pageSize) {
			params = params.append("pageNumber", pageNumber);
			params = params.append("pageSize", pageSize);
		}

		return params;
	}

	private setPaginatedResponse(response: HttpResponse<Member[]>) {
		this.paginatedResult.set({
			items: response.body as Member[],
			pagination: JSON.parse(response.headers.get("Pagination")!),
		});
	}
}
