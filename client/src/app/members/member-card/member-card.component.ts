import { Component, computed, inject, input } from "@angular/core";
import { Member } from "../../_models/member";
import { RouterLink } from "@angular/router";
import { LikesService } from "../../_services/likes.service";

@Component({
	selector: "app-member-card",
	standalone: true,
	imports: [RouterLink],
	templateUrl: "./member-card.component.html",
	styleUrl: "./member-card.component.css",
})
export class MemberCardComponent {
	private likeService = inject(LikesService);
	memberChildComponent = input.required<Member>();
	hasLiked = computed(() =>
		this.likeService.likeIDs().includes(this.memberChildComponent().id)
	);

	toggleLike() {
		this.likeService.toggleLike(this.memberChildComponent().id).subscribe({
			next: () => {
				if (this.hasLiked()) {
					this.likeService.likeIDs.update(userids =>
						userids.filter(x => x !== this.memberChildComponent().id)
					);
				} else {
					this.likeService.likeIDs.update(userids => [
						...userids,
						this.memberChildComponent().id,
					]);
				}
			},
		});
	}
}
