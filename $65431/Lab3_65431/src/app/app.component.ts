import { Component, OnInit} from '@angular/core';
import { HttpService } from './http.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  mapType = 'satellite';
  trams = [];
  selectedTram;
  numberForm : FormGroup;
  selectedNumber=20;
  selectedBrigade

  constructor(private http: HttpService, private formBuilder : FormBuilder) {  }

  ngOnInit(): void {
    this.numberForm = this.formBuilder.group({
      number: ['', Validators.required]
    });
    
    // this.http.getAllTrams().subscribe((data: any[]) =>
    //   this.trams = data);

    // setInterval(() => {
    //   this.http.getAllTrams().subscribe((data: any[]) =>
    //   {
    //     this.trams=[];
    //     this.trams = data
    //   }
    //   );
    //     }, 10000);
  }

  selectTram(event, id) {
    this.selectedBrigade = id;

    this.trams.forEach(value =>{
      if(value.brigade==id)
      {
      this.selectedTram = {
        lat: value.latitude,
        lng: value.longitude,
        line: value.line,
        brigade : value.brigade,
        time : value.time
      }
    }})
    // this.selectedTram = {
    //   lat: this.trams[event._id].latitude,
    //   lng: this.trams[event._id].longitude,
    //   line: this.trams[event._id].line,
    //   brigade : this.trams[event._id].brigade,
    //   time : this.trams[event._id].time

   //   };
   }

   onSubmit(modelForm)
   {
     if(modelForm.value.number != null)
     {
       this.selectedNumber = modelForm.value.number;
      this.http.getByLine(this.selectedNumber).subscribe((data : any[]) => {this.trams=data; console.log(this.trams);});
      
       setInterval(() => {
        this.http.getByLine(this.selectedNumber).subscribe((data: any[]) =>
        {
          this.trams=data;
          
          this.trams.forEach(value =>{
            if(value.brigade==this.selectedBrigade)
            {
              console.log("tram: "+this.selectedBrigade);
            this.selectedTram = {
              lat: value.latitude,
              lng: value.longitude,
              line: value.line,
              brigade : value.brigade,
              time : value.time
            }
          }})
        }
        );
          }, 10000);
       
     }
      
    
   }

  
}
