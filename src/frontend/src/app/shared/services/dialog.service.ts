import { Overlay } from '@angular/cdk/overlay';
import { inject, Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';

type DialogOptions = {
  size?: 'sm' | 'md' | 'lg' | 'xl' | 'xxl';
  data?: any;
  disableClose?: boolean;
  isFullScreen?: boolean;
  isScrollable?: boolean;
};
@Injectable({
  providedIn: 'root',
})
export class DialogService {
  private readonly _dialog = inject(MatDialog);
  private readonly _overlay = inject(Overlay);

  open(component: any, options: DialogOptions): Observable<any> {
    const dialogConfig = new MatDialogConfig();

    if (options.isFullScreen) {
      dialogConfig.width = '100%';
      dialogConfig.height = '100%';
      dialogConfig.maxWidth = '100vw';
      dialogConfig.maxHeight = '100vh';
      dialogConfig.panelClass = ['full-screen-dialog'];
      dialogConfig.position = {
        top: '0',
        left: '0',
      };
    } else {
      dialogConfig.width = this.getDialogWidth(options.size);
      dialogConfig.maxWidth = '90vw';
      dialogConfig.height = 'auto';
    }

    dialogConfig.data = options.data;
    dialogConfig.hasBackdrop = true;
    dialogConfig.autoFocus = false;
    dialogConfig.scrollStrategy = this._overlay.scrollStrategies.noop();
    dialogConfig.disableClose = options.disableClose ?? true;

    const dialogRef = this._dialog.open(component, dialogConfig);

    return dialogRef.afterClosed();
  }

  private getDialogWidth(
    size?: 'default' | 'sm' | 'md' | 'lg' | 'xl' | 'xxl',
  ): string {
    switch (size) {
      case 'sm':
        return '300px';
      case 'md':
        return '600px';
      case 'lg':
        return '800px';
      case 'xl':
        return '1200px';

      case 'xxl':
        return '1400px';
      default:
        return '600px'; // Default size
    }
  }
}
