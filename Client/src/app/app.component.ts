import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GridComponent } from './grid/grid.component';
import { RegisterComponent } from './register/register.component';  
import { UploadImageComponent } from './upload-image/upload-image.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ProfileComponent }  from './profile/profile.component'


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GridComponent, RegisterComponent,UploadImageComponent,NavbarComponent,ProfileComponent ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'

})
export class AppComponent {
  title = 'Client';
}
