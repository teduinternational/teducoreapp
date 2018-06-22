import { Component } from '@angular/core';
import { Http, RequestOptions, Response, Headers } from '@angular/http';

import 'rxjs/add/operator/map';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private _http: Http) { }
  public model: any = {};
  title = 'app';
  public email:string;

  public login() {
    var data = {
      UserName: this.model.username,
      Password: this.model.password
    }
    let headers = new Headers();
    //headers.append("Content-Type", "application/x-www-form-urlencoded");
    headers.append('Content-Type', 'application/json');
    //headers.append("Authorization", "Bearer " + this.user.access_token);

    let options = new RequestOptions({ headers: headers });

    this._http.post('http://localhost:59134/api/Account/login',
      JSON.stringify(data), options).map((response: Response) => {
        let result = response.json();
        var base64Url = result.token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        var user = JSON.parse(window.atob(base64));
        console.log(user);

        this.email = user.email;

      }).subscribe(data => {
        console.log(data);
      });
  }
}
