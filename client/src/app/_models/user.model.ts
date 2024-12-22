export class User {
  private username: string = '';
  private password: string = '';
  private token: string = '';
  // Define properties of the User model

  // Getter and Setter for 'username'
  get Username(): string {
    return this.username;
  }

  set Username(value: string) {
    this.username = value;
  }

  // Getter and Setter for 'password'
  get Password(): string {
    return this.password;
  }

  set Password(value: string) {
    this.password = value;
  }

  // Getter and Setter for 'token'
  get Token(): string {
    return this.token;
  }

  set Token(value: string) {
    this.token = value;
  }
}
