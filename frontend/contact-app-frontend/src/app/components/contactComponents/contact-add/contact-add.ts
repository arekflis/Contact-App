import { Component } from '@angular/core';
import { ContactService } from '../../../services/contactService/contact-service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule, FormControl, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ContactRequest } from '../../../models/contactModels/ContactRequest';
import { CategoryService } from '../../../services/categoryService/category-service';
import { Category } from '../../../models/categoryModels/Category';
import { Subcategory } from '../../../models/categoryModels/Subcategory';

@Component({
  selector: 'app-contact-add',
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './contact-add.html',
  styleUrl: './contact-add.css'
})
export class ContactAdd {

  constructor(private contactService: ContactService,
              private categoryService: CategoryService,
              private router: Router) {}

  addContactForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.maxLength(20)]), 
    lastName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)\\S+$')]),
    phoneNumber: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{9}$')]),
    dateOfBirth: new FormControl('', [Validators.required]),
    selectedCategory: new FormControl('', Validators.required),
    selectedSubcategory: new FormControl(''),
    subcategory: new FormControl('')
  });

  categories: Category[] = [];
  subcategories: Subcategory[] = [];
  showSubcategoryInput = false;
  showSubcategorySelect = false;

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories = response;
      },
      error: (error) => {
        console.error('Error fetching categories:', error);
        this.categories = [];
      }
    });
  }

  onCategoryChange(): void {
    const selectedCategoryId = this.addContactForm.get('selectedCategory')?.value;
    const selectedCategory = this.categories.find(c => c.categoryId === selectedCategoryId);
    
    if (selectedCategoryId) {
      if (selectedCategory?.name === "inny"){
        this.showSubcategoryInput = true;
        this.showSubcategorySelect = false;
        this.subcategories = [];
      }
      else{
        this.categoryService.getSubcategoriesByCategoryId(selectedCategoryId).subscribe({
        next: (response) => {
          this.subcategories = response;
          this.subcategories.push({ subcategoryId: "0", name: "dowolny" });
          this.showSubcategorySelect = true;
          this.showSubcategoryInput = false;
        },
        error: (error) => {
          console.error('Error fetching subcategories:', error);
          this.resetSubcategoriesView();
        }
      });
      }
      
    } else this.resetSubcategoriesView();
  }

  resetSubcategoriesView(): void{
    this.subcategories = [];
    this.showSubcategorySelect = false;
    this.showSubcategoryInput = false;
  }

  onSubmit(): void {  
    if (this.addContactForm.invalid) {
      console.error('Form is invalid');
      return;
    }

    let selectedSubcategoryId = null;
    let subcategoryName = null;

    if (this.showSubcategoryInput) {
      const inputValue = this.addContactForm.get('subcategory')?.value?.trim();
      if (inputValue && inputValue.length > 0) {
        subcategoryName = inputValue;
      }
    } 
    if (this.showSubcategorySelect) {
      const selectedValue = this.addContactForm.get('selectedSubcategory')?.value ?? null;
      if (selectedValue){
        const selectedSubcategory = this.subcategories.find(s => s.subcategoryId === selectedValue);
        if (selectedSubcategory?.name !== "dowolny") selectedSubcategoryId = selectedValue;
      }
    }
    
    const contactRequest: ContactRequest = {
      firstName: this.addContactForm.get('firstName')?.value ?? "",
      lastName: this.addContactForm.get('lastName')?.value ?? "",
      email: this.addContactForm.get('email')?.value ?? "",
      password: this.addContactForm.get('password')?.value ?? "",
      phoneNumber: this.addContactForm.get('phoneNumber')?.value ?? "",
      dateOfBirth: this.addContactForm.get('dateOfBirth')?.value ?? "",
      categoryId: this.addContactForm.get('selectedCategory')?.value ?? "",
      subcategoryId: selectedSubcategoryId,
      subcategoryName: subcategoryName
    };

    this.contactService.addNewContact(contactRequest).subscribe({
      next: (response) => {
        console.log('Contact added successfully:', response);
        this.router.navigate(['/contacts']);
      },
      error: (error) => {
        console.error('Error adding contact:', error);
      }
    });
  }

}
