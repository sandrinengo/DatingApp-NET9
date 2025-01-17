import {
	Component,
	inject,
	output,
	AfterViewInit,
	OnInit,
} from "@angular/core";
import {
	AbstractControl,
	FormBuilder,
	FormControl,
	FormGroup,
	ReactiveFormsModule,
	ValidatorFn,
	Validators,
} from "@angular/forms";
import { User } from "../_models/user.model";
import { AccountService } from "../_services/account.service";
import { JsonPipe, NgIf } from "@angular/common";
import { ValidationService } from "../_services/validation.service";
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from "@angular/router";

@Component({
	selector: "app-register",
	standalone: true,
	imports: [
		ReactiveFormsModule,
		JsonPipe,
		NgIf,
		TextInputComponent,
		DatePickerComponent,
	],
	templateUrl: "./register.component.html",
	styleUrl: "./register.component.css",
})
export class RegisterComponent implements OnInit, AfterViewInit {
	//  @ViewChild('txtUserName') inputElement!: ElementRef<HTMLInputElement>;
	private accountService = inject(AccountService);
	private validationService = inject(ValidationService);
	private formBuilder = inject(FormBuilder);
	private router = inject(Router);
	model: User = new User();
	cancelRegisteOutput = output<boolean>();
	maxDate = new Date();
	validationErrors: string[] | undefined;

	/** Create form elements */
	registerForm: FormGroup = new FormGroup({});
	/** End Creating */

	userRegister() {
		const dob = this.getDateOnly(this.registerForm.get("dob")?.value);
		this.registerForm.patchValue({ dob: dob });
		this.accountService.userRegister(this.registerForm.value).subscribe({
			next: _ => {
				this.router.navigateByUrl("/members");
			},
			error: error => (this.validationErrors = error),
		});
	}

	cancelRegister() {
		this.cancelRegisteOutput.emit(false);
	}

	private getDateOnly(dob: string | undefined) {
		if (!dob) return;

		return new Date(dob).toISOString().slice(0, 10);
	}

	initializeForm() {
		this.registerForm = this.formBuilder.group({
			gender: ["male"],
			username: ["", Validators.required],
			knownAs: ["", Validators.required],
			dob: ["", Validators.required],
			city: ["", Validators.required],
			country: ["", Validators.required],
			password: [
				"",
				[Validators.required, Validators.minLength(4), Validators.maxLength(8)],
			],
			confirmPassword: ["", [Validators.required, this.matchValue("password")]],
		});
		// Subscribe to the password control to observe if it's changed or not, so we can check the matching
		// between password control and confirm password control.
		this.registerForm.controls["password"].valueChanges.subscribe({
			next: () =>
				this.registerForm.controls["confirmPassword"].updateValueAndValidity(),
		});
	}

	initializeForm11111() {
		this.registerForm = new FormGroup({
			username: new FormControl("", Validators.required),
			password: new FormControl("", [
				Validators.required,
				Validators.minLength(4),
				Validators.maxLength(8),
			]),
			confirmPassword: new FormControl("", [
				Validators.required,
				this.matchValue("password"),
			]),
		});
		// Subscribe to the password control to observe if it's changed or not, so we can check the matching
		// between password control and confirm password control.
		this.registerForm.controls["password"].valueChanges.subscribe({
			next: () =>
				this.registerForm.controls["confirmPassword"].updateValueAndValidity(),
		});
	}

	matchValue(matchTo: string): ValidatorFn {
		return (control: AbstractControl) => {
			return control.value === control.parent?.get(matchTo)?.value
				? null
				: { isMatching: true };
		};
	}

	// Validation method to check for errors
	isFieldInvalid(fieldName: string): boolean {
		const field = this.registerForm.get(fieldName);
		return !!(field?.invalid && (field?.touched || field?.dirty));
	}

	getErrorMessage(fieldName: string, validatorType: string) {}

	// Reusable method to get messages for a field
	test(validators: string[]): { name: string; message: string }[] {
		return validators.map(validator => {
			console.log(this.validationService.getValidationMessage(validator).name);
			return this.validationService.getValidationMessage(validator);
		});
	}

	ngAfterViewInit(): void {
		//this.inputElement.nativeElement.focus(); // Automatically focus the input after view initialization
	}

	ngOnInit(): void {
		this.initializeForm();
		this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
	}
}
