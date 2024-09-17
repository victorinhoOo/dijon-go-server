import { Component, NgModule } from '@angular/core';
import  {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ProfileSettingsComponent } from '../profile-settings/profile-settings.component';
import { MatDialog } from '@angular/material/dialog';




@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [MatIconModule,MatButtonModule,],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})



export class ProfileComponent {
  constructor(public dialog: MatDialog) {}

  openDialog(): void {
    const dialogRef = this.dialog.open(ProfileSettingsComponent, {
      width: '80%',
      height: '80%',
      panelClass: 'custom-dialog-container'
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log('popup close'); //indiquer plus tard les changements enregistrés
    });
    
  }
  
}



