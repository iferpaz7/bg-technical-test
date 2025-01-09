import { Component, inject, signal } from '@angular/core';
import { PersonUseCase } from '@config/domain/use-cases/person.use-case';
import { IdentificationTypeService } from '@shared/services/identification-type.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { IdentificationType } from '@shared/models/identification-type';
import { ToasterService } from 'acontplus-utils';
import { MatCustomDialogComponent } from '@shared/components/mat-custom-dialog/mat-custom-dialog.component';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-person-add-edit',
  imports: [
    MatCustomDialogComponent,
    ReactiveFormsModule,
    MatIcon,
    MatButton,
    MatFormField,
    MatInput,
    MatLabel,
    MatOption,
    MatSelect,
  ],
  templateUrl: './person-add-edit.component.html',
  styleUrl: './person-add-edit.component.scss',
})
export class PersonAddEditComponent {
  dialogIcon = signal('person_add"');
  dialogTitle = signal('Añadir Nueva Persona');

  personForm = new FormGroup({
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
  private readonly _dialogRef = inject(MatDialogRef<PersonAddEditComponent>);
  private readonly _identificationTypeService = inject(
    IdentificationTypeService,
  );
  private readonly _personUseCase = inject(PersonUseCase);

  private readonly _tS = inject(ToasterService);
  loadData() {
    this._identificationTypeService.get().subscribe((response) => {
      this.identificationTypes = response.identificationTypes;
    });
  }

  ngOnInit() {
    this.loadData();
  }
  onClose() {
    this._dialogRef.close();
  }
  onSave() {
    let person = {
      ...this.personForm.value,
    };

    this._personUseCase.create(person).subscribe((response) => {
      this._tS.toastr({
        type: response.code === '1' ? 'success' : 'warning',
        message: response.message,
      });
    });
  }
}
