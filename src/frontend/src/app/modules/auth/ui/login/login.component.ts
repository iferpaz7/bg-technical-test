import { Component, inject } from '@angular/core';
import {
  MatCard,
  MatCardContent,
  MatCardFooter,
  MatCardHeader,
  MatCardTitle,
} from '@angular/material/card';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatAnchor, MatButton } from '@angular/material/button';
import { MatTooltip } from '@angular/material/tooltip';
import { AuthUseCase } from '@modules/auth/domain/use-cases/auth.use-case';
import { User } from '@modules/auth/domain/models/user.model';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { MatIconModule } from '@angular/material/icon';
import { MatOption, MatSelect } from '@angular/material/select';
import { IdentificationType } from '@shared/models/identification-type';
import { IdentificationTypeService } from '@shared/services/identification-type.service';

@Component({
  selector: 'app-login',
  imports: [
    MatCard,
    MatCardHeader,
    MatCardContent,
    MatCardTitle,
    MatTabGroup,
    MatTab,
    MatLabel,
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatButton,
    MatCardFooter,
    MatAnchor,
    MatTooltip,
    FaIconComponent,
    MatIconModule,
    MatSelect,
    MatOption,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(100),
    ]),
    password: new FormControl('', Validators.required),
  });
  signupForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    idCard: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(100),
    ]),
    identificationTypeId: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', Validators.required),
  });
  identificationTypes: IdentificationType[] = [];
  private readonly _authUseCase = inject(AuthUseCase);
  private readonly _identificationTypeService = inject(
    IdentificationTypeService,
  );
  
  loadData() {
    this._identificationTypeService.get().subscribe((response) => {
      this.identificationTypes = response.identificationTypes;
    });
  }

  login() {
    const user = {
      ...this.loginForm.value,
    } as User;

    this._authUseCase.login(user);
  }

  ngOnInit() {
    this.loadData();
  }

  signup() {
    const data = {
      ...this.signupForm.value,
    };

    this._authUseCase.signup(data);
  }
}
