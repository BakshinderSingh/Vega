import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {

  makes;
  vehicle:any = {};
  models;
  features;
  constructor(private vehicleSrv: VehicleService) {    
  }

  ngOnInit() {
    this.vehicleSrv.getMakes().subscribe(makes =>this.makes = makes);
    this.vehicleSrv.getFeatures().subscribe(features => this.features = features);
  }

  onMakeChange() {
    let make=this.makes.find(m => m.id == this.vehicle.make)
    this.models =make?make.models:[];
  }
  onModelChange() {
    console.log(this.vehicle);
  }

}
