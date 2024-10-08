import { Component, AfterViewInit} from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css'
})
export class GridComponent implements AfterViewInit{

  private count:number;

  public constructor(){
    this.count = 0;
  }


  ngAfterViewInit(): void {
    let pawns = Array.from(document.getElementsByClassName("pawn"));
    pawns.forEach(pawn=>{
      pawn.addEventListener("click", ()=>this.click(pawn))
    })
  }

  public click(pawn: Element):void{
    let index: number = this.count % 2;
    let colors = ["black", "white"];
    pawn.setAttribute("style", "background-color: "+colors[index]+";")
    this.count++;
  }







}
