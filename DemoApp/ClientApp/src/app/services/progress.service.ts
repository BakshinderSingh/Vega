import { Injectable } from '@angular/core';
import { BrowserXhr } from '@angular/http';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class ProgressService {
  private uploadProgress: Subject<any>;
  downloadProgress: Subject<any> = new Subject();
  constructor() { }

  StartTracking() {
    this.uploadProgress = new Subject();
    return this.uploadProgress;
  }

  Notify(progress) {
    if (this.uploadProgress)
      this.uploadProgress.next(progress);
  }

  EndTracking() {
    if (this.uploadProgress)
      this.uploadProgress.complete();
  }

}

@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {

  constructor(private service:ProgressService) {
    super();    
  }
  build(): XMLHttpRequest {
    var xhr: XMLHttpRequest = super.build();
    xhr.onprogress = (event) => {
      this.service.downloadProgress.next(this.createProgress(event));
    };
    xhr.upload.onprogress = (event) => {
      this.service.Notify(this.createProgress(event));
    };

    xhr.upload.onloadend = () => {
      this.service.EndTracking();
    }
    return xhr;
  };
  createProgress(event) {
    return {
      total: event.total,
      percentage: Math.round(event.loaded / event.total*100)
    }
  }
}
