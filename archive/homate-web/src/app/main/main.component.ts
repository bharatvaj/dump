import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { Room } from '../room';
import { Device } from '../device'
import { from } from 'rxjs';
import { Informer } from '../informer';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  @Input() room: Room;

  devices = new Array<Device>()

  constructor() {
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
   
    Informer.getInstance().devices(changes.room.currentValue).subscribe(h => {
      this.devices = h;
    });
  }

}

