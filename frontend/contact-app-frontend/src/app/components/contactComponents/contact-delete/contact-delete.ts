import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ContactService } from '../../../services/contactService/contact-service';
import { FormsModule, FormControl, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-contact-delete',
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './contact-delete.html',
  styleUrl: './contact-delete.css'
})
export class ContactDelete {

  constructor(private contactService: ContactService,
              private router: Router,
              private route: ActivatedRoute) { }

  contactId: string = "";
  
  deleteContactForm = new FormGroup({
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W)\\S+$')])
  }); 

  ngOnInit(): void{
    this.contactId = this.route.snapshot.paramMap.get('id') ?? "";
  }

  onSubmit(): void {
    if (this.deleteContactForm.invalid) {
      console.error('Form is invalid');
      return;
    }

    const deleteRequest = {
      password: this.deleteContactForm.get('password')?.value ?? ""
    }

    this.contactService.deleteContact(this.contactId, deleteRequest).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['/contacts']);
      },
      error: (error) => {
        console.error('Error deleting contact:', error);
      }
    });
  }
}
