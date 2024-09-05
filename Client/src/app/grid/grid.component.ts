import { Component, AfterViewInit } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { elementAt } from 'rxjs';

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css'
})
export class GridComponent {



  public click():void{
    alert("clicked");
  }







}
