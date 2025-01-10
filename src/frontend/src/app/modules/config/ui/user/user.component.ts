import { Component, inject, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import {
  MatCard,
  MatCardActions,
  MatCardContent,
  MatCardHeader,
  MatCardTitle,
  MatCardSubtitle,
} from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Person } from '@config/domain/models/person.model';
import { MatPagination } from '@shared/models/pagination';
import { DialogService } from '@shared/services/dialog.service';
import { PersonAddEditComponent } from '@config/ui/person/components/person-add-edit/person-add-edit.component';
import Swal from 'sweetalert2';
import { User } from '@modules/auth/domain/models/user.model';
import { UserUseCase } from '@config/domain/use-cases/user.use-case';
import { UserAddEditComponent } from '@config/ui/user/components/user-add-edit/user-add-edit.component';

@Component({
  selector: 'app-user',
  imports: [
    MatButton,
    MatCard,
    MatCardContent,
    MatIcon,
    MatPaginator,
    MatCardTitle,
    MatCardHeader,
    MatCardActions,
    MatCardSubtitle,
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
})
export class UserComponent {
  users: User[] = [];
  pagination = signal(new MatPagination());

  private readonly _dialogService = inject(DialogService);

  private readonly _userUseCase = inject(UserUseCase);

  loadData() {
    this.pagination().pageIndex = this.pagination().pageIndex + 1;
    const params = this.pagination();
    this._userUseCase.get(params).subscribe((response) => {
      this.users = response.users;
      this.pagination.update((e) => {
        return {
          ...e,
          pageIndex: e.pageIndex - 1,
          totalRecords: response?.totalRecords || 0,
        };
      });
    });
  }

  ngOnInit() {
    this.loadData();
  }

  onPageChanged($event: PageEvent) {
    this.pagination.update((pag) => {
      return {
        ...pag,
        totalRecords: $event.length,
        pageSize: $event.pageSize,
        pageIndex: $event.pageIndex,
      };
    });
    this.loadData();
  }

  onDialogOpen() {
    const dialogRef = this._dialogService.open(UserAddEditComponent, {
      size: 'sm',
    });
    dialogRef.subscribe((response) => {
      if (typeof response === 'object' && response.code === '1') {
        this.loadData();
      }
    });
  }

  onEdit(item: User) {
    const dialogRef = this._dialogService.open(UserAddEditComponent, {
      size: 'sm',
      data: item,
    });
    dialogRef.subscribe((response) => {
      if (typeof response === 'object' && response.code === '1') {
        this.loadData();
      }
    });
  }

  onDelete(item: User) {
    Swal.fire({
      icon: 'warning',
      title: 'Advertencia!',
      text: `Esta seguro de eliminar este registro de manera permanente?`,
      showCancelButton: true,
      cancelButtonText: 'No',
      confirmButtonText: 'Si',
      allowOutsideClick: false,
    }).then((response) => {
      if (response.isConfirmed) {
        this._userUseCase.delete(item.id).subscribe((result) => {
          Swal.fire({
            icon: result.code == '1' ? 'info' : 'warning',
            title: 'Persona',
            text: result.message,
          });
          if (result.code === '1') {
            this.loadData();
          }
        });
      }
    });
  }
}
