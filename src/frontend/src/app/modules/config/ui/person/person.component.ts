import { Component, inject, signal } from '@angular/core';
import { ColumnDefinition, Pagination } from 'acontplus-utils';
import { Person } from '@config/domain/models/person.model';
import { PersonUseCase } from '@config/domain/use-cases/person.use-case';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatDynamicTableComponent } from '@shared/components/mat-dynamic-table/mat-dynamic-table.component';
import { MatPagination } from '@shared/models/pagination';
import { DialogService } from '@shared/services/dialog.service';
import { PersonAddEditComponent } from '@config/ui/person/components/person-add-edit/person-add-edit.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-person',
  imports: [
    MatMenu,
    MatIcon,
    MatMenuItem,
    MatMenuTrigger,
    MatIconButton,
    MatCardContent,
    MatCard,
    MatPaginator,
    MatDynamicTableComponent,
    MatButton,
  ],
  templateUrl: './person.component.html',
  styleUrl: './person.component.scss',
})
export class PersonComponent {
  columnDefs: ColumnDefinition[] = [
    { key: 'op', label: 'Op.', type: 'string' },
    { key: 'firstName', label: 'Nombre', type: 'string' },
    { key: 'lastName', label: 'Apellido', type: 'string' },
    { key: 'fullName', label: 'Nombre Completo', type: 'string' },
    { key: 'identificationType', label: 'T. Ident.', type: 'string' },
    { key: 'idCard', label: 'Nro. Ident.', type: 'string' },
    { key: 'email', label: 'Correo', type: 'string' },
  ];
  columns: string[] = this.columnDefs.map((colDef) => colDef.key);
  persons: Person[] = [];
  pagination = signal(new MatPagination());

  private readonly _dialogService = inject(DialogService);

  private readonly _personUseCase = inject(PersonUseCase);

  loadData() {
    this.pagination().pageIndex = this.pagination().pageIndex + 1;
    const params = this.pagination();
    this._personUseCase.get(params).subscribe((response) => {
      this.persons = response.persons;
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
    const dialogRef = this._dialogService.open(PersonAddEditComponent, {
      size: 'lg',
    });
    dialogRef.subscribe((response) => {
      if (typeof response === 'object' && response.code === '1') {
        this.loadData();
      }
    });
  }

  onEdit(item: Person) {
    const dialogRef = this._dialogService.open(PersonAddEditComponent, {
      size: 'lg',
      data: item,
    });
    dialogRef.subscribe((response) => {
      if (typeof response === 'object' && response.code === '1') {
        this.loadData();
      }
    });
  }

  onDelete(item: Person) {
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
        this._personUseCase.delete(item.id).subscribe((result) => {
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
