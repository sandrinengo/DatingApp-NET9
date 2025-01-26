import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Member } from "../_models/member";
import { PaginatedResult } from "../_models/pagination";
import { setPaginatedResponse, setPaginationHeader } from "./paginationHelper";

@Injectable({
	providedIn: "root",
})
export class LikesService {
	private http = inject(HttpClient);
	baseURL = environment.apiURL;

	likeIDs = signal<number[]>([]);
	paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

	toggleLike(targetUserID: number) {
		return this.http.post(`${this.baseURL}likes/${targetUserID}`, {});
	}

	getLikes(predicate: string, pageNumber: number, pageSize: number) {
		let params = setPaginationHeader(pageNumber, pageSize);
		params = params.append("predicate", predicate);
		return this.http.get<Member[]>(
			`${this.baseURL}likes`, {observe:"response", params}).subscribe({
				next:response=>setPaginatedResponse(response, this.paginatedResult)
			});
	}

	getLikeIDs() {
		return this.http.get<number[]>(`${this.baseURL}likes/list`, {}).subscribe({
			next: userids => this.likeIDs.set(userids), // We get the IDs back from the server, and then set likeIDs signal to the ids
		});
	}
}
