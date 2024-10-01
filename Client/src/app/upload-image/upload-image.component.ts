import { Component } from '@angular/core';

@Component({
  selector: 'app-upload-image',
  standalone: true,
  imports: [],
  templateUrl: './upload-image.component.html',
  styleUrl: './upload-image.component.css'
})
export class UploadImageComponent {

  uploadedImage:any;

  fileChange(e:any){
  debugger
    if(e.target.files[0]!=null){
      var reader = new FileReader();
      reader.onload=(e:any)=>{
        this.uploadedImage = e.target.result;
      }
      reader.readAsDataURL(e.target.files[0]);
    }
  }  

}
