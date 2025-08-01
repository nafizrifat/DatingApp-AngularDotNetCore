import { Component, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryModule, ImageItem } from 'ng-gallery';
import { map } from 'rxjs';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../../member/member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { PresenceService } from '../../_service/presence.service';
import { AccountService } from '../../_services/account.service';
import { HubConnectionState } from '@microsoft/signalr';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy{

  @ViewChild('memberTabs',{static:true}) memberTabs?:TabsetComponent;
  private messageService = inject(MessageService);
  private accountService = inject(AccountService);
  presenceService = inject(PresenceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member:Member={} as Member;
  images:GalleryModule[]=[];
  activeTab?: TabsModule;
  

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data=>{
        this.member = data['member'];
        this.member && this.member.pothos.map(p=>{
          this.images.push(new ImageItem({src:p.url, thumb:p.url}))

        })
      }
    })

    this.route.paramMap.subscribe({
      next:_=>this.onRouteParamsChange()
    })

    this.route.queryParams.subscribe({
      next: params=>{
        params['tab'] && this.selectTab(params['tab'])
      }
    })
  }


  selectTab(heading: string){
    if(this.memberTabs){
      const messageTab = this.memberTabs.tabs.find(x=>x.heading===heading);
      if(messageTab) messageTab.active = true;
    }
  }

  onRouteParamsChange(){
    debugger;
    var xx =this.activeTab;
    const user = this.accountService.currentUser();
    if(!user) return;
    if(this.messageService.hubConnection?.state === HubConnectionState.Connected && this.activeTab=='Messages')
      this.messageService.hubConnection.stop().then(()=>{
        this.messageService.createHubConnection(user, this.member.userName);
      })
  }


  onTabActivated(data: TabDirective){

    this.activeTab = data?.heading;
    // this.activeTab = data;
    this.router.navigate([],{
      relativeTo: this.route,
      queryParams:{tab: this.activeTab},
      queryParamsHandling:'merge'
    })

    if(this.activeTab ==='Messages' && this.member){
      const user = this.accountService.currentUser();
      if(!user) return;
      this.messageService.createHubConnection(user, this.member.userName);
    }
    else{
      this.messageService.stopHubConnection();
    }
  }
    ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
