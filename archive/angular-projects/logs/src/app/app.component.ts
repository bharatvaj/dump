import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: [`
    .blue {
      background-color: blue;
    }
    `]
})

export class AppComponent {
  shouldDisplay = false;
  logs = []

  toggleDisplay() {
    this.shouldDisplay = !this.shouldDisplay;
    this.logs.push(new Date());
  }
}
