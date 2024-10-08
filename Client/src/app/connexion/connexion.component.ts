import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card'; // Importe MatCardModule


@Component({
  selector: 'app-connexion',
  standalone: true,
  imports: [ReactiveFormsModule,MatCardModule],
  templateUrl: './connexion.component.html',
  styleUrl: './connexion.component.css'
})
export class ConnexionComponent {

   connexionForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.connexionForm = this.fb.group({
      pseudo: ['', Validators.required],
      pwd: ['',Validators.required]})     
  }

  onSubmit(): void{
    console.log(this.connexionForm.value);
  }

}
