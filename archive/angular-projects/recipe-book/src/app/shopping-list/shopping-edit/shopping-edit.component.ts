import { Component, EventEmitter, Output } from '@angular/core';
import { Ingredient } from 'src/app/shared/ingredient.model';

@Component({
  selector: 'app-shopping-edit',
  templateUrl: './shopping-edit.component.html'
})
export class ShoppingEditComponent {

  @Output() onAdded = new EventEmitter<{name: string, amount: number}>();

  onAdd(name: string, amount: number){
    this.onAdded.emit( new Ingredient(name,  amount));
  }
}
