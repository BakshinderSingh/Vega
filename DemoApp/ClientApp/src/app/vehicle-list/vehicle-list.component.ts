import { Component, OnInit } from '@angular/core';
import { Vehicle, KeyValuePair } from '../models/vehicle';
import { VehicleService } from '../services/vehicle.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[];
  //allVehicles: Vehicle[];
  makes: KeyValuePair[];
  filter: any = {};
  columns = [
    { title: 'Contact Name', key: 'contactName', isSortable: true },
    { title: 'Make', key: 'make', isSortable: true },
    {title:'Model',key:'model',isSortable:true}
  ]
  constructor(private VehicleService: VehicleService) {
  }

  ngOnInit() {
    Observable.forkJoin(this.VehicleService.getMakes(), this.VehicleService.getVehicles(this.filter)).subscribe(
      data => {
        this.makes = data[0];
        this.vehicles = data[1];
      }
    )
  }

  onFilterChange() {
    this.VehicleService.getVehicles(this.filter).subscribe(vehicles => this.vehicles = vehicles);
  }
  sortBy(sortElement) {
    if (this.filter.SortBy === sortElement)
      this.filter.isSortAscending = !this.filter.isSortAscending;
    else {
      this.filter.isSortAscending = true;
      this.filter.SortBy = sortElement;
    }
    this.VehicleService.getVehicles(this.filter).subscribe(vehicles => this.vehicles = vehicles);
  }
  ResetFilter() {
    this.filter = {};
    this.onFilterChange();
  }

}
