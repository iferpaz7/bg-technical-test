import {
  Component,
  effect,
  ElementRef,
  inject,
  Renderer2,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { fab } from '@fortawesome/free-brands-svg-icons';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { ThemeService } from '@core/theme/theme.service';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Bg Users';
  private readonly _themeService = inject(ThemeService);
  private readonly _document = inject(DOCUMENT);
  private readonly _renderer = inject(Renderer2);
  constructor(
    private readonly el: ElementRef,
    private readonly renderer: Renderer2,
    library: FaIconLibrary,
  ) {
    library.addIconPacks(fab, fas, far);

    //loading theme
    effect(() => {
      this._renderer.setAttribute(
        this._document.documentElement,
        'class',
        this._themeService.theme(),
      );
    });
  }
}
