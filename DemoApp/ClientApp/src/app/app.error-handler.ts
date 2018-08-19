import { ErrorHandler, Inject, NgZone,isDevMode } from "@angular/core";
import { ToastyService } from 'ng2-toasty';
import * as Raven from 'raven-js';
export class AppErrorHandler implements ErrorHandler {
  constructor(@Inject(ToastyService) private toastyService: ToastyService) {

  }
  handleError(error: any): void {
    if (!isDevMode()) {
      Raven.captureMessage("mYErro");
      Raven.captureException(error.originalException || error);
    }
    else
      //throw error;
    this.toastyService.error({
      title: 'Error',
      msg: 'An unexpected error',
      theme: 'bootstrap', showClose: true, timeout: 5000
    })
    ///throw new Error("Method not implemented.");
  } 
}
