import { Component, AfterViewInit, OnInit} from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { WebsocketService } from '../websocket.service';

@Component({
  selector: 'app-grid',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule],
  templateUrl: './grid.component.html',
  styleUrl: './grid.component.css'
})
export class GridComponent implements AfterViewInit, OnInit{

  private size:number;

  public constructor(private route: ActivatedRoute, private websocketService:WebsocketService){
    this.size = 0
  }

  public getSize():number{
    return this.size-1;
  }

  ngOnInit(): void {
    this.size = 19;
  }

  ngAfterViewInit(): void {
    let stones = document.getElementsByClassName("stone");
    let stonesArray = Array.from(stones);
    stonesArray.forEach((stone)=>{
      stone.addEventListener("click",()=>{
        this.click(stone);
      })
    })
  }

  public click(stone:any):void{
    this.websocketService.placeStone(stone.id);
  }

 







}
