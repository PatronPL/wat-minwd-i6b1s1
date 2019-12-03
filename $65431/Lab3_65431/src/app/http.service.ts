import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  url="https://localhost:44373/data/";
  constructor(private http: HttpClient) { }

  getAllTrams() {
    return this.http.get(this.url);
  }
  getByLine(number) {
    return this.http.get(this.url+"line/"+number);
  }
}
