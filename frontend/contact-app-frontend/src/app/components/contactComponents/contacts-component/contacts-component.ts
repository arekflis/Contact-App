import { Component } from '@angular/core';
import { ContactService } from '../../../services/contactService/contact-service';
import { Contact } from '../../../models/contactModels/Contact';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-contacts-component',
  imports: [CommonModule, RouterModule],
  templateUrl: './contacts-component.html',
  styleUrl: './contacts-component.css'
})
export class ContactsComponent {

    constructor(private contactService: ContactService) { }

    contacts: Contact[] = [];

    ngOnInit(): void {
      this.contactService.getContacts().subscribe({
        next: (response) => this.contacts = response,
        error: (error) => { 
          console.error('Error fetching contacts:', error);
          this.contacts = []; 
        }
      });
    }
}
