import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {
  @Output() shoppingClick = new EventEmitter<string>();
  @Output() recipeClick = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  onRecipeClick(){
    this.recipeClick.emit();
  }

  onShoppingClick(){
    this.shoppingClick.emit();
  }

}
