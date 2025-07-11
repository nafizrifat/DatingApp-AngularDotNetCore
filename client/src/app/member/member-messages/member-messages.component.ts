import { AfterViewChecked, Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent  implements AfterViewChecked{

  @ViewChild('nessaegForm') messageForm?: NgForm;
  @ViewChild('scrollMe') scrollContainer?: any;
  messageService = inject(MessageService);
  username = input.required<string>();
  messageContent = '';

  sendMessage(){
    this.messageService.sendMessage(this.username(), this.messageContent)?.then(()=>{
      this.messageForm?.reset();
       this.scrollToBottom();
    })

  }

    ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom(){
    if(this.scrollContainer){
      if(this.scrollContainer){
        this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
      }
    }
  }

}
