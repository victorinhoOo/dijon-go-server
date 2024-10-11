export class LoginUserDTO {
    // Déclaration des attributs privés
    private _username: string;
    private _password: string;
  
    constructor(username: string, password: string) {
      this._username = username;
      this._password = password;
    }
  
    // Getters
    public get username(): string {
      return this._username;
    }
  
    public get password(): string {
      return this._password;
    }
  
    // Setters
    public set username(value: string) {
      this._username = value;
    }
  
    public set password(value: string) {
      this._password = value;
    }
  
  }
  