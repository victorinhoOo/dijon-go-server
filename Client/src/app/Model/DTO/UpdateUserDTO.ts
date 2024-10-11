export class UpdateUserDTO {
    private _tokenuser: string;
    private _username: string;
    private _email: string;
    private _password: string;
    private _profilePic: File | null;
  
  
    constructor(token: string, user: string, mail: string,password: string, profile: File) 
    {
        this._tokenuser = token;
        this._username = user;
        this._email = mail;
        this._password = password;
        this._profilePic = profile;

    }
  
    // renvoie la valeur de tokenuser
    get tokenuser(): string 
    {
        return this._tokenuser;
    }
  
    // set la valeur de tokenuser
    set tokenuser(value: string) 
    {
        this._tokenuser = value;
    }

    // renvoie le nom d'utilisateur
    get username(): string 
    {
        return this._username;
    }

    // set un nom d'utilisateur
    set username(value: string) 
    {
        this._username = value;
    }

    // renvoie l'email
    get email(): string 
    {
        return this._email;
    }

    // set l'email
    set email(value: string) 
    {
        this._email = value;
    }

    // renvoie l'image de profil
    get profilePic(): File | null
    {
        return this._profilePic;
    }

    // set l'image de profil
    set profilePic(value: File) 
    {
        this._profilePic = value;
    }

    // renvoie le mot de passe
    get password(): string 
    {
        return this._password;
    }

    // set le mot de passe
    set password(value: string) 
    {
        this._password = value;
    }
}
