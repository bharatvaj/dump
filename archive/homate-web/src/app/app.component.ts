import { Component } from '@angular/core';
import { Informer } from './informer';
import { Room } from './room';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'homate-web';

  rooms = Array<Room>()


  a = Informer.getInstance().rooms.subscribe(h => {
    this.rooms = h;
  });

  selectedRoom = new Room("", "", "")

  roomClick(room) {
    this.selectedRoom = room;
    console.log(room);
  }
}
