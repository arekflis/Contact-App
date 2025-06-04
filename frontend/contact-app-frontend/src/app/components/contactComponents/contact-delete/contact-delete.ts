import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ContactService } from '../../../services/contactService/contact-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-contact-delete',
  imports: [RouterModule, FormsModule],
  templateUrl: './contact-delete.html',
  styleUrl: './contact-delete.css'
})
export class ContactDelete {

  constructor(private contactService: ContactService,
              private router: Router,
              private route: ActivatedRoute) { }

  contactId: string = "";
  password: string = "";

  ngOnInit(): void{
    this.contactId = this.route.snapshot.paramMap.get('id') ?? "";
  }

  onSubmit(): void {
    const deleteRequest = {
      password: this.password
    }

    this.contactService.deleteContact(this.contactId, deleteRequest).subscribe({
      next: (response) => {
        console.log('Contact deleted successfully:', response);
        this.router.navigate(['/contacts']);
      },
      error: (error) => {
        console.error('Error deleting contact:', error);
      }
    });
  }
}
