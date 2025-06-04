import { Component } from '@angular/core';
import { ContactService } from '../../../services/contactService/contact-service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule, FormControl, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ContactRequest } from '../../../models/contactModels/ContactRequest';
import { CategoryService } from '../../../services/categoryService/category-service';
import { Category } from '../../../models/categoryModels/Category';
import { Subcategory } from '../../../models/categoryModels/Subcategory';
import { ActivatedRoute } from '@angular/router';
import { ContactDetailsDto } from '../../../models/contactModels/ContactDetailsDto';

@Component({
  selector: 'app-contact-update',
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './contact-update.html',
  styleUrl: './contact-update.css'
})
export class ContactUpdate {
    constructor(private contactService: ContactService,
              private categoryService: CategoryService,
              private router: Router,
              private activatedRoute: ActivatedRoute) {
                this.subcategoryUnknown = { subcategoryId: "0", name: "dowolny" }
              }

  editContactForm = new FormGroup({
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

  contactId: string = "";
  contactDetails: ContactDetailsDto | undefined;
  subcategoryUnknown: Subcategory | undefined

  ngOnInit(): void {
    this.contactId = this.activatedRoute.snapshot.paramMap.get('id') ?? "";

    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories = response;
      },
      error: (error) => {
        console.error('Error fetching categories:', error);
        this.categories = [];
      }
    });

    this.contactService.getContactDetails(this.contactId).subscribe({
      next: (response) => {
        this.contactDetails = response;
        if (this.contactDetails.subcategory !== null && this.contactDetails.category.name !== "inny") this.showSubcategorySelect = true;
        if (this.contactDetails.subcategory !== null && this.contactDetails.category.name === "inny") this.showSubcategoryInput = true;
        this.getSubcategoriesByCategoryId(this.contactDetails.category, this.contactDetails.category.categoryId);
        this.editContactForm.patchValue({
          firstName: response.firstName,
          lastName: response.lastName,
          email: response.email,
          phoneNumber: response.phoneNumber,
          dateOfBirth: response.dateOfBirth ? this.formatDate(response.dateOfBirth) : '',
          selectedCategory: response.category.categoryId,
          selectedSubcategory: response.subcategory?.subcategoryId ?? this.subcategoryUnknown?.subcategoryId,
          subcategory: response.subcategory?.name ?? ''
        });
      }
    });
  }

  formatDate(date: Date): string {
    const d = new Date(date);
    const month = ('0' + (d.getMonth() + 1)).slice(-2);
    const day = ('0' + d.getDate()).slice(-2);
    return `${d.getFullYear()}-${month}-${day}`;
  }

  onCategoryChange(): void {
    const selectedCategoryId = this.editContactForm.get('selectedCategory')?.value;
    const selectedCategory = this.categories.find(c => c.categoryId === selectedCategoryId);
    
    if (selectedCategoryId && selectedCategory) {
      this.getSubcategoriesByCategoryId(selectedCategory, selectedCategoryId);
      
    } else this.resetSubcategoriesView();
  }

  getSubcategoriesByCategoryId(selectedCategory: Category, selectedCategoryId: string): void {
    if (selectedCategory?.name === "inny"){
        this.showSubcategoryInput = true;
        this.showSubcategorySelect = false;
        this.subcategories = [];
      }
      else{
        this.categoryService.getSubcategoriesByCategoryId(selectedCategoryId).subscribe({
        next: (response) => {
          this.subcategories = response;
          this.subcategories.push(this.subcategoryUnknown!);
          this.showSubcategorySelect = true;
          this.showSubcategoryInput = false;
        },
        error: (error) => {
          console.error('Error fetching subcategories:', error);
          this.resetSubcategoriesView();
        }
      });
      }
    }

  resetSubcategoriesView(): void{
    this.subcategories = [];
    this.showSubcategorySelect = false;
    this.showSubcategoryInput = false;
  }

  onSubmit(): void {  
    if (this.editContactForm.invalid) {
      console.error('Form is invalid');
      return;
    }

    let selectedSubcategoryId = null;
    let subcategoryName = null;

    if (this.showSubcategoryInput) {
      const inputValue = this.editContactForm.get('subcategory')?.value?.trim();
      if (inputValue && inputValue.length > 0) {
        subcategoryName = inputValue;
      }
    } 
    if (this.showSubcategorySelect) {
      const selectedValue = this.editContactForm.get('selectedSubcategory')?.value ?? null;
      if (selectedValue){
        const selectedSubcategory = this.subcategories.find(s => s.subcategoryId === selectedValue);
        if (selectedSubcategory?.name !== "dowolny") selectedSubcategoryId = selectedValue;
      }
    }
    
    const contactRequest: ContactRequest = {
      firstName: this.editContactForm.get('firstName')?.value ?? "",
      lastName: this.editContactForm.get('lastName')?.value ?? "",
      email: this.editContactForm.get('email')?.value ?? "",
      password: this.editContactForm.get('password')?.value ?? "",
      phoneNumber: this.editContactForm.get('phoneNumber')?.value ?? "",
      dateOfBirth: this.editContactForm.get('dateOfBirth')?.value ?? "",
      categoryId: this.editContactForm.get('selectedCategory')?.value ?? "",
      subcategoryId: selectedSubcategoryId,
      subcategoryName: subcategoryName
    };

    this.contactService.updateContact(this.contactId, contactRequest).subscribe({
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
