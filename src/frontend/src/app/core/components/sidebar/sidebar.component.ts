import { Component, inject } from '@angular/core';
import { MatListItem, MatNavList } from '@angular/material/list';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { MatIcon } from '@angular/material/icon';
import { MatSidenav } from '@angular/material/sidenav';
import { AuthUseCase } from '@modules/auth/domain/use-cases/auth.use-case';
import { TokenUseCase } from '@modules/auth/domain/use-cases/token.use-case';
import { UserData } from '@modules/auth/domain/models/user-data.model';
import { SIDEBAR_LINKS } from '@core/components/sidebar/sidebar-options';

@Component({
  selector: 'app-sidebar',
  imports: [
    MatNavList,
    MatListItem,
    RouterLink,
    FaIconComponent,
    RouterLinkActive,
    MatIcon,
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  private readonly _sidenav = inject(MatSidenav);
  private readonly _authUseCase = inject(AuthUseCase);
  private readonly _tokenUseCase = inject(TokenUseCase);

  menus = SIDEBAR_LINKS;
  user: UserData = {
    username: '',
  };

  logout() {
    this._authUseCase.logout();
  }

  ngOnInit(): void {
    const decodedToken = this._tokenUseCase.getDecodedToken();
    if (decodedToken) {
      this.user = decodedToken;
    }
  }

  closeSidenav() {
    if (this._sidenav.mode === 'over') {
      this._sidenav.close();
    }
  }
}
