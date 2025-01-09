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
  }
}
