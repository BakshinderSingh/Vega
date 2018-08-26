import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { SaveVehicle } from '../models/vehicle';

@Injectable()
export class VehicleService {

  updateVehicle(vehicle: SaveVehicle): any {
    return this.http.put('/api/vehicles/' + vehicle.id, vehicle).map(res => res.json());
  }
  constructor(private http: Http) { }

  getMakes() {
    return this.http.get('/api/makes').map(res => res.json());
  }
  getFeatures() {
    return this.http.get('api/features').map(res => res.json());
  }
  createVehicle(vehicle) {
    return this.http.post('api/vehicles', vehicle).map(res => res.json());
  }
  getVehicle(id) {
    return this.http.get('api/vehicles/' + id).map(res => res.json());
  }
  getVehicles(filter) {
    return this.http.get('api/vehicles/' + '?' + this.toQueryString(filter)).map(res => res.json());
  }

  toQueryString(obj) {
    var parts = [];
    console.log(obj);
    for (var property in obj) {
      var value = obj[property];
      if (value != null && value != undefined)
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(obj[property]))
    }
    console.log(parts);
    return parts.join('&');
  }
  deleteVehicle(id) {
    return this.http.delete('api/vehicles/' + id).map(res => res.json());
  }

}
