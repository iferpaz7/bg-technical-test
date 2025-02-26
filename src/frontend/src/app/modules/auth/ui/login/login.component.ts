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
import { MatButton } from '@angular/material/button';
import { AuthUseCase } from '@modules/auth/domain/use-cases/auth.use-case';
import { User } from '@modules/auth/domain/models/user.model';
import { MatIconModule } from '@angular/material/icon';
import { MatOption, MatSelect } from '@angular/material/select';
import { IdentificationType } from '@shared/models/identification-type';
import { IdentificationTypeService } from '@shared/services/identification-type.service';
import { ToasterService } from 'acontplus-utils';
import { SocialLinksComponent } from '@shared/components/social-links/social-links.component';
import { ThemeToggleComponent } from '@core/theme/theme-toggle.component';

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
    MatIconModule,
    MatSelect,
    MatOption,
    SocialLinksComponent,
    ThemeToggleComponent,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', [
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
    identificationTypeId: new FormControl(0, [Validators.required]),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', Validators.required),
  });
  identificationTypes: IdentificationType[] = [];
  private readonly _authUseCase = inject(AuthUseCase);
  private readonly _identificationTypeService = inject(
    IdentificationTypeService,
  );
  private readonly _tS = inject(ToasterService);
  loadData() {
    this._identificationTypeService.get().subscribe((response) => {
      this.identificationTypes = response.identificationTypes;
    });
  }

  login() {
    const user = {
      ...this.loginForm.value,
    } as User;

    this._authUseCase.login(user).subscribe((response) => {
      this._tS.toastr({
        type: response.code === '1' ? 'success' : 'warning',
        message: response.message,
      });
    });
  }

  ngOnInit() {
    this.loadData();
  }

  signup() {
    let identificationTypeCode = this.identificationTypes.find(
      (x) => x.id === this.signupForm.get('identificationTypeId')?.value,
    )?.code;

    let idCardLength = this.signupForm.get('idCard')?.value?.length;

    console.log(this.signupForm.value);
    console.log(identificationTypeCode);
    console.log(idCardLength);

    if (idCardLength !== 13 && identificationTypeCode === '04') {
      return this._tS.toastr({
        type: 'warning',
        message: 'El ruc debe tener 13 dígitos',
      });
    }
    if (idCardLength !== 10 && identificationTypeCode === '05') {
      return this._tS.toastr({
        type: 'warning',
        message: 'La cédula debe tener 10 dígitos',
      });
    }
    const data = {
      ...this.signupForm.value,
    };

    this._authUseCase.signup(data).subscribe((response) => {
      this._tS.toastr({
        type: response.code === '1' ? 'success' : 'warning',
        message: response.message,
      });
    });
  }
}
