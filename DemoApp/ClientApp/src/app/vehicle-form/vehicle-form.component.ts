import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { Subject } from 'rxjs/Subject';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes;
  vehicle: any = {
    features: [], contact: {}};
  models;
  features;
  constructor(private vehicleSrv: VehicleService, private toastyService: ToastyService
    , private route: ActivatedRoute, private router: Router) {
    route.params.subscribe(data => this.vehicle.id = +data['id']);
  }

  ngOnInit() {
    Observable.forkJoin([
      this.vehicleSrv.getMakes(),
      this.vehicleSrv.getFeatures(),
      this.vehicleSrv.getVehicle(this.vehicle.id)
    ]).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      this.vehicle = data[2];
      }, err => {
        if (err.status == 404)
          this.router.navigate(['/home']);
      }
      )

    //this.vehicleSrv.getVehicle(this.vehicle.id).subscribe(v => this.vehicle = v, err => { console.log("Not Fiound"); this.router.navigate(['/home']); }, () => { console.log(this.vehicle) });
    //this.vehicleSrv.getMakes().subscribe(makes => this.makes = makes, err => { console.log(err); }, () => { console.log(this.makes) });
    //this.vehicleSrv.getFeatures().subscribe(features => this.features = features, err => { console.log(err); }, () => { console.log(this.features) });
  }

  onMakeChange() {
    let make=this.makes.find(m => m.id == this.vehicle.makeId)
    this.models = make ? make.models : [];
    delete this.vehicle.modelId;
  }
  onModelChange() {
    console.log(this.vehicle);
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
    console.log("submit");
    this.vehicleSrv.createVehicle(this.vehicle).subscribe(res => console.log(res)
    )
  }

}
