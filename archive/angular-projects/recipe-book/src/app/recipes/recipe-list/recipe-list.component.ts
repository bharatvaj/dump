import { Component, Output, EventEmitter } from '@angular/core';
import {Recipe } from '../recipe.model'

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html'
})
export class RecipeListComponent {
  recipes: Recipe[] = [
    new Recipe("A Test Recipe", "This is simply a test", "https://proxy.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.QVXRXYy0XUyMv5tp-tN_HgHaGu%26pid%3D15.1&f=1"),
    new Recipe("Another Test Recipe", "Another test recipe", "https://proxy.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.QVXRXYy0XUyMv5tp-tN_HgHaGu%26pid%3D15.1&f=1")
  ];

  @Output() recipeClick = new EventEmitter<Recipe>();

  onRecipeItem(recipe: Recipe){
    this.recipeClick.emit(recipe);
  }
}
