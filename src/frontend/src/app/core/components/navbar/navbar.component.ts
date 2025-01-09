import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatTooltip } from '@angular/material/tooltip';
import { MatButton } from '@angular/material/button';
import { AuthUseCase } from '@modules/auth/domain/use-cases/auth.use-case';
import { UserData } from '@modules/auth/domain/models/user-data.model';
import { TokenUseCase } from '@modules/auth/domain/use-cases/token.use-case';
import {ThemeToggleComponent} from "@core/theme/theme-toggle.component";

@Component({
  selector: 'app-navbar',
  imports: [
    MatIcon,
    MatMenuTrigger,
    MatTooltip,
    MatButton,
    MatMenu,
    MatMenuItem,
    ThemeToggleComponent,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  user: UserData = {
    username: '',
  };
  private readonly _authUseCase = inject(AuthUseCase);
  private readonly _tokenUseCase = inject(TokenUseCase);

  ngOnInit(): void {
    const decodedToken = this._tokenUseCase.getDecodedToken();
    if (decodedToken) {
      this.user = decodedToken;
    }
  }

  goToProfile() {}

  logout() {
    this._authUseCase.logout();
  }
}
