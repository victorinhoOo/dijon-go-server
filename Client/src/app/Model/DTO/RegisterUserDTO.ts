

export class RegisterUserDTO {
  // Déclaration des attributs privés
  private _username: string;
  private _email: string;
  private _password: string;
  private _profilePic: File | null;

  constructor(username: string, email: string, password: string, profilePic: File | null) {
    this._username = username;
    this._email = email;
    this._password = password;
    this._profilePic = profilePic;
  }

  // Getters
  public get username(): string {
    return this._username;
  }

  public get email(): string {
    return this._email;
  }

  public get password(): string {
    return this._password;
  }

  public get profilePic(): File | null {
    return this._profilePic;
  }

  // Setters
  public set username(value: string) {
    this._username = value;
  }

  public set email(value: string) {
    this._email = value;
  }

  public set password(value: string) {
    this._password = value;
  }

  public set profilePic(value: File | null) {
    this._profilePic = value;
  }
}
