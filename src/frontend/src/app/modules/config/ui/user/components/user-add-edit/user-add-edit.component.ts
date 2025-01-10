import { Component, inject, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCustomDialogComponent } from '@shared/components/mat-custom-dialog/mat-custom-dialog.component';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Person } from '@config/domain/models/person.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IdentificationType } from '@shared/models/identification-type';
import { ApiResponse, ToasterService } from 'acontplus-utils';
import { UserUseCase } from '@config/domain/use-cases/user.use-case';

@Component({
  selector: 'app-user-add-edit',
  imports: [
    MatButton,
    MatCustomDialogComponent,
    MatFormField,
    MatIcon,
    MatInput,
    MatLabel,
    ReactiveFormsModule,
  ],
  templateUrl: './user-add-edit.component.html',
  styleUrl: './user-add-edit.component.scss',
})
export class UserAddEditComponent {
  private readonly data = inject<Person>(MAT_DIALOG_DATA);

  dialogIcon = signal('person_add"');
  dialogTitle = signal('Añadir Usuario');

  userForm = new FormGroup({
    id: new FormControl(0),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    enabled: new FormControl(false),
  });

  identificationTypes: IdentificationType[] = [];
  private readonly _dialogRef = inject(MatDialogRef<UserAddEditComponent>);

  private readonly _userUseCase = inject(UserUseCase);

  private readonly _tS = inject(ToasterService);

  ngOnInit() {
    if (this.data && this.data.id > 0) {
      this.dialogIcon = signal('edit');
      this.dialogTitle = signal('Editar Usuario');

      this.userForm.patchValue(this.data);
    }
  }
  onClose() {
    this._dialogRef.close();
  }

  commonResponse = (response: ApiResponse) => {
    this._tS.toastr({
      type: response.code === '1' ? 'success' : 'warning',
      message: response.message,
    });
    if (response.code === '1') {
      this._dialogRef.close(response);
    }
  };

  onSave() {
    let person = {
      ...this.userForm.value,
    };

    if (this.data && this.data.id > 0) {
      this._userUseCase
        .update(this.data.id, person)
        .subscribe(this.commonResponse);
    } else {
      this._userUseCase.create(person).subscribe(this.commonResponse);
    }
  }
}
