import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ContactService } from '../../../services/contactService/contact-service';
import { ContactDetailsDto } from '../../../models/contactModels/ContactDetailsDto';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/authService/auth-service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-contact-details',
  imports: [RouterModule, CommonModule],
  templateUrl: './contact-details.html',
  styleUrl: './contact-details.css'
})
export class ContactDetails {

    contact: ContactDetailsDto | null = null;
    isLoggedIn$: Observable<boolean>;
    
    constructor(private contactService: ContactService,
                private route: ActivatedRoute, 
                private authService: AuthService) {
                  
                  this.isLoggedIn$ = this.authService.isLoggedIn$;
                 }

    ngOnInit(): void {
      const contactId = this.route.snapshot.paramMap.get('id');
      
      if (contactId) {
        this.contactService.getContactDetails(contactId).subscribe({
          next: (response) => this.contact = response,
          error: (error) => { 
            console.error('Error fetching contact details:', error);
            this.contact = null; 
          }
        });
      } else {
        console.error('No contact ID provided in route');
      }
    
    }
}
