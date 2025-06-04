import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '../../models/categoryModels/Category';
import { Observable } from 'rxjs';
import { Subcategory } from '../../models/categoryModels/Subcategory';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private httpClient: HttpClient) { }

  getCategories(): Observable<Category[]>{
    return this.httpClient.get<Category[]>('/api/Category/categories');
  }

  getSubcategoriesByCategoryId(categoryId: string): Observable<Subcategory[]> {
    return this.httpClient.get<Subcategory[]>(`/api/Category/subcategories/${categoryId}`);
  }
}
