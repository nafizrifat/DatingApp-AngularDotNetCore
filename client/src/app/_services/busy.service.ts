import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  busySpinnerCount =0;
  private spinnerService = inject(NgxSpinnerService);

  busy(){
    this.busySpinnerCount++;
    this.spinnerService.show(undefined,{
      type:'line-scale-party',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    })
  }

  idle(){
    this.busySpinnerCount--;
    if(this.busySpinnerCount<=0){
      this.busySpinnerCount=0;
      this.spinnerService.hide();
    }
  }

}
