import { Component } from '@angular/core';
import {MatAnchor} from "@angular/material/button";
import {MatTooltip} from "@angular/material/tooltip";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: 'app-social-links',
  imports: [
    MatAnchor,
    MatTooltip,
    FaIconComponent
  ],
  templateUrl: './social-links.component.html',
  styleUrl: './social-links.component.scss'
})
export class SocialLinksComponent {

}
