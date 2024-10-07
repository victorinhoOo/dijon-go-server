export class RegisterUserDTO {
    constructor(
      public username: string,
      public email: string,
      public password: string,
      public profilePic?: File 
    ) {}
  }