import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';
import { VehicleService } from '../services/vehicle.service';

import { BrowserXhr } from '@angular/http';
import { PhotoService } from '../services/photo.service';
import { ProgressService, BrowserXhrWithProgress } from '../services/progress.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css'],
  providers: [{ provide: BrowserXhr, useClass: BrowserXhrWithProgress },ProgressService]
})
export class ViewVehicleComponent implements OnInit {
  vehicle: any;
  vehicleId: number;
  photos: any[];
  progress: any;

  @ViewChild('fileInput') fileReference: ElementRef;

  constructor(private route: ActivatedRoute, private router: Router,
    private toasty: ToastyService, private vehicleService: VehicleService,
    private photoService: PhotoService, private progressService: ProgressService,
    private zone: NgZone) {
    route.params.subscribe(p =>
    {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0)
      {
        router.navigate(['./vehicles']);
      }
    })
  }

  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId).subscribe
      (
      v => this.vehicle = v,
      err => {
        if (err.status == 400) {
          this.router.navigate(['./vehicles']);
          return;
        }
      });
    this.photoService.getPhotos(this.vehicleId).subscribe(photos => this.photos = photos); 
  }

  delete() {
    if (confirm('Are you sure you want to delete?')) {
      this.vehicleService.deleteVehicle(this.vehicleId).subscribe(
        x => {
          this.router.navigate(['./vehicles']);
        });
    }
  }

  uploadPhoto() {
    //console.log(nativeElement.files[0].size);
    this.progressService.StartTracking().subscribe(x => {
      console.log(x);
      this.zone.run(() => { this.progress = x}    );
    });
    var nativeElement: HTMLInputElement = this.fileReference.nativeElement;
    var file = nativeElement.files[0];
    nativeElement.value = '';
    this.photoService.upload(this.vehicleId, file)
      .subscribe(x => this.photos.push(x));
  }

}
