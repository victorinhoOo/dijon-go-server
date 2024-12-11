import { Component } from '@angular/core';
import { GridComponent } from '../grid/grid.component';
import { UserCookieService } from '../Model/UserCookieService';
import { environment } from '../environment';
import { GameDAO } from '../Model/DAO/GameDAO';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { GameStateDTO } from '../Model/DTO/GameStateDTO';
import { firstValueFrom } from 'rxjs';
import { MatIcon } from '@angular/material/icon';
import { IGameBoardDrawer } from '../IGameBoardDrawer';
import { GameBoardDrawer } from '../GameBoardDrawer';

const PROFILE_PIC_URL = environment.apiUrl + '/profile-pics/';

@Component({
  selector: 'app-replay-screen',
  standalone: true,
  imports: [GridComponent, MatIcon],
  templateUrl: './replay-screen.component.html',
  styleUrl: './replay-screen.component.css',
})
export class ReplayScreenComponent {
  private playerPseudo: string;
  private playerAvatar: string;
  private gameDAO: GameDAO;
  private id: number;

  private stateNumber: number;

  private blackCapturedContainer : HTMLElement | null;
  private whiteCapturedContainer : HTMLElement | null;

  private states: GameStateDTO[];

  private boardDrawer: IGameBoardDrawer;

  constructor(
    private userCookieService: UserCookieService,
    private route: ActivatedRoute,
    private httpClient: HttpClient
  ) {
    this.stateNumber = 0;
    this.boardDrawer = new GameBoardDrawer();
    this.gameDAO = new GameDAO(this.httpClient);
    this.playerPseudo = this.userCookieService.getUser()!.Username; // Récupère le nom d'utilisateur et l'avatar pour l'afficher sur la page
    this.playerAvatar = PROFILE_PIC_URL + this.playerPseudo;
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.states = [];
    this.blackCapturedContainer = null;
    this.whiteCapturedContainer = null;
  }

  async ngAfterViewInit(): Promise<void> {
    this.hideGameElements();
    this.displayPlayersInformations();
    
    await this.loadGameStates();
    this.displayState(this.stateNumber);
    document.addEventListener(('keydown'), (event) => {
      switch (event.key) {
        case "ArrowRight": this.nextState();break;
        case "ArrowLeft": this.previousState();break;
        case "Enter": this.moveToState();break;
      }
    });
  }

  private updateCapturedCounters(black:string, white:string):void{
    if(this.blackCapturedContainer != null && this.whiteCapturedContainer != null){
      this.blackCapturedContainer.innerText = "Prises : " + black;  
      this.whiteCapturedContainer.innerText = "Prises : " + white;
    }
  }

  public nextState(): void {
    if (this.stateNumber < this.states.length - 1) {
      this.stateNumber++;
      this.displayState(this.stateNumber);
    }
  }

  public previousState(): void {
    if (this.stateNumber > 0) {
      this.stateNumber--;
      this.displayState(this.stateNumber);
    }
  }

  public moveToState():void{
    let input = document.getElementById('move-number') as HTMLInputElement;
    let moveNumber = Number(input.value)-1;
    this.stateNumber = moveNumber;
    if(moveNumber >= 0 && moveNumber < this.states.length){
      this.stateNumber = moveNumber;
      this.displayState(this.stateNumber);
    }
    input.value = "";
  }

  private displayState(number: number): void {
    let state = this.states[number];
    let blackCaptured = state.CapturedBlack();
    let whiteCaptured = state.CapturedWhite();
    let input = document.getElementById('move-number') as HTMLInputElement;
    input.value = state.MoveNumber().toString();
    this.updateCapturedCounters(blackCaptured.toString(), whiteCaptured.toString());
    let board = state.Board();
    this.boardDrawer.drawBoardState(board);
  }

  private async loadGameStates(): Promise<void> {
    this.states = [];
    console.log(this.states);
    const response = await firstValueFrom(
      this.gameDAO.GetGameStatesById(this.id)
    );
    response.forEach((state) => {
      let gameInfo = new GameStateDTO(
        state['board'],
        state['capturedBlack'],
        state['capturedWhite'],
        state['moveNumber']
      );
      this.states.push(gameInfo);
    });
  }

  private displayPlayersInformations(): void {
    let playerPseudoContainer = document.getElementById('player-pseudo-text');
    let playerAvatarContainer = document.getElementById('player-pic') as HTMLImageElement;
    if (
      playerPseudoContainer != undefined &&
      playerAvatarContainer != undefined
    ) {
      playerPseudoContainer.innerText = this.playerPseudo;
      playerAvatarContainer.src = `${PROFILE_PIC_URL}${this.playerPseudo}`;
      this.displayOpponentInformations();
    }
  }

  private displayOpponentInformations(): void {
    this.gameDAO.GetGameById(this.id).subscribe((gameInfo: any) => {
      let opponentPseudoContainer = document.getElementById('pseudo-text');
      let opponentAvatarContainer = document.getElementById('opponent-pic') as HTMLImageElement;
      if (opponentPseudoContainer != undefined && opponentAvatarContainer != undefined) {
        if (this.playerPseudo == gameInfo.usernamePlayer1) {
          console.log(gameInfo.usernamePlayer1);
          opponentPseudoContainer.innerText = gameInfo.usernamePlayer2;
          opponentAvatarContainer.src = `${PROFILE_PIC_URL}${gameInfo.usernamePlayer2}`;
          this.blackCapturedContainer = document.getElementById('opponent-score-value');
          this.whiteCapturedContainer = document.getElementById('player-score-value');
        } else {
          opponentPseudoContainer.innerText = gameInfo.usernamePlayer1;
          opponentAvatarContainer.src = `${PROFILE_PIC_URL}${gameInfo.usernamePlayer1}`;
          this.blackCapturedContainer = document.getElementById('player-score-value');
          this.whiteCapturedContainer = document.getElementById('opponent-score-value');
        }
      }
    });
  }

  private hideGameElements(): void {
    let passButton = document.getElementById('pass');
    let playerTimer = document.getElementById('player-timer');
    let opponentTimer = document.getElementById('opponent-timer');
    let ruleContainer = document.getElementById('rule-container');
    passButton!.style.display = 'none';
    playerTimer!.style.display = 'none';
    opponentTimer!.style.display = 'none';
    ruleContainer!.style.display = 'none';
  }

  public firstState(): void {
    this.stateNumber = 0;
    this.displayState(this.stateNumber);
  }

  public lastState(): void {
    this.stateNumber = this.states.length - 1;
    this.displayState(this.stateNumber);
  }
}
