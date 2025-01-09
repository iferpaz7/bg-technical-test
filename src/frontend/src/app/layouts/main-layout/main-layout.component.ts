import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatSidenav, MatSidenavContainer, MatSidenavModule} from '@angular/material/sidenav';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import {
  faFacebook,
  faGooglePlay,
  faWhatsapp,
  faYoutube,
} from '@fortawesome/free-brands-svg-icons';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import {NavbarComponent} from "@core/components/navbar/navbar.component";
import {SidebarComponent} from "@core/components/sidebar/sidebar.component";
import {SocialLinksComponent} from "@shared/components/social-links/social-links.component";

@Component({
  selector: 'app-main-layout',
  imports: [
    RouterOutlet,
    MatSidenavContainer,
    MatToolbar,
    MatSidenavModule,
    MatIconButton,
    MatIcon,
    FaIconComponent,
    NavbarComponent,
    SidebarComponent,
    SocialLinksComponent,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
})
export class MainLayoutComponent {
  faFacebook: IconProp = faFacebook;
  faYoutube: IconProp = faYoutube;
  faGooglePlay: IconProp = faGooglePlay;
  faWhatsapp: IconProp = faWhatsapp;

  private readonly _breakpointObserver = inject(BreakpointObserver);
  isMobile = toSignal(
    this._breakpointObserver.observe('(max-width: 768px)').pipe(
      map((result) => result.matches),
      shareReplay(),
    ),
  );
}
