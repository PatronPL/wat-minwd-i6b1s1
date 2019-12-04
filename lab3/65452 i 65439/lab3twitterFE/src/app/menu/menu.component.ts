import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CloudData, CloudOptions, ZoomOnHoverOptions } from 'angular-tag-cloud-module';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  options: CloudOptions = {
    width: 1000,
    height: 400,
    overflow: false
  };

  zoomOnHoverOptions: ZoomOnHoverOptions = {
    scale: 2.0,
    transitionTime: 0,
    delay: 0
  };

  data: CloudData[] = [];

  user: any;
  username: any;
  days: any;
  userInfo = false;
  tweets = false;
  showCloud = false;

  fullname: any;
  twittername: any;
  joined: any;
  ftweet: any;
  ltweet: any;
  description: any;

  url: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    document
      .getElementById('submitBtn')
      .addEventListener('click', this.getInput.bind(this), true);
    document
      .getElementById('submitBtn2')
      .addEventListener('click', this.getInput2.bind(this), true);
    document
      .getElementById('submitBtn3')
      .addEventListener('click', this.getInput3.bind(this), true);
  }

  getInput() {
    this.url = `http://localhost:5000/twitter/user-info/${this.username}`;
    this.userInfo = true;
    this.username = (document.getElementById(
      'username'
    ) as HTMLInputElement).value;
    this.days = (document.getElementById('days') as HTMLInputElement).value;

    this.http.get(this.url).subscribe(
      response => {
        this.user = response;
        this.fullname = this.user.name;
        this.twittername = this.user.screenName;
        this.description = this.user.userDTO.description;
        this.joined = this.user.userDTO.createdAt;

        console.log(this.user);
      },
      error => {
        console.log(error);
      }
    );
  }

  getInput2() {
    this.tweets = true;
    this.username = (document.getElementById(
      'username'
    ) as HTMLInputElement).value;
    const url = `http://localhost:5000/twitter/first-and-last/${this.username}`;
    this.http.get(url).subscribe(
      response => {
        console.log(response);
        this.ftweet = response ? response[0].text : 'Brak';
        this.ltweet = response ? response[1].text : 'Brak';
      },
      error => {
        console.log(error);
      }
    );
  }

  getInput3() {
    this.showCloud = false;
    this.data = [];
    this.username = (document.getElementById(
      'username'
    ) as HTMLInputElement).value;
    this.days = (document.getElementById('days') as HTMLInputElement).value;
    const url = `http://localhost:5000/twitter/top-words/${this.username}?fromDate=${this.days}`;
    this.http.get(url).subscribe(
      response => {
        console.log(response);
        Object.entries(response).forEach(([key, value]) =>
          this.data.push({
            text: value.Key,
            weight: value.Value,
            color: this.getRandomColor()
          })
        );
        this.showCloud = true;
      },
      error => {
        console.log(error);
      }
    );
  }

  getRandomColor() {
    const chars = '0123456789ABCDEF'.split('');
    let hex = '#';
    for (let i = 0; i < 6; i++) {
      hex += chars[Math.floor(Math.random() * 16)];
    }
    return hex;
  }
}
