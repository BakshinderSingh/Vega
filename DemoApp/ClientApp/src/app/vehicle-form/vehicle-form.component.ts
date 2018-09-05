import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { Subject } from 'rxjs/Subject';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';
import { SaveVehicle, Vehicle } from '../models/vehicle';
import * as _ from 'underscore';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes;
  vehicle: SaveVehicle = {
    id:0,
    makeId: 0,
    isRegistered: false,
    modelId: 0,
    features: [],
    contact:
      {
        email: '',
        name: '',
        phone: ''
      }
  };
  models;
  features;
  constructor(private vehicleSrv: VehicleService, private toastyService: ToastyService
    , private route: ActivatedRoute, private router: Router)
  {
    route.params.subscribe(data => {
      if(data['id'])
        this.vehicle.id = +data['id']
    });
  }

  ngOnInit() {
    let sources = [this.vehicleSrv.getMakes(),
      this.vehicleSrv.getFeatures()];
    if (this.vehicle.id)
      sources.push(this.vehicleSrv.getVehicle(this.vehicle.id));
    Observable.forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2]);
        this.populateModels();
      }
      }, err => {
        if (err.status == 404) { }
          this.router.navigate(['/home']);
      }
      )
  }

  private setVehicle(v: Vehicle) {
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.contact = v.contact;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.features = _.pluck(v.features, 'id');;
  }

  populateModels() {
    let make = this.makes.find(m => m.id == this.vehicle.makeId)
    this.models = make ? make.models : []; 
  }

  onMakeChange() {

    console.log(this.vehicle);
    this.populateModels(); 
    delete this.vehicle.modelId;
  }
  onModelChange() {
  }
  onFeatureChanged(featureId, event) {
    if (event.target.checked) {
      this.vehicle.features.push(featureId);
    }
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);

    }
  }
  submit() {
    if (this.vehicle.id) {
      this.vehicleSrv.updateVehicle(this.vehicle).subscribe(res => {
        console.log(res);
        this.toastyService.success({
          title: 'Success',
          msg: 'The Vehicle updated successfully.',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        })
      })
    }
    else {
      console.log(this.vehicle);
      this.vehicleSrv.createVehicle(this.vehicle).subscribe(res => console.log(res));

    }
  }

  deleteVehicle() {
    if (confirm('Are you sure?'))
      this.vehicleSrv.deleteVehicle(this.vehicle.id).subscribe
        (res => {
          this.router.navigate(['/home']);
        });
  }

}
