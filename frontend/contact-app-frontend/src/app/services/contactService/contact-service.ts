import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contact } from '../../models/contactModels/Contact';
import { ContactDetailsDto } from '../../models/contactModels/ContactDetailsDto';
import { DeleteContactDto } from '../../models/contactModels/DeleteContactDto';
import { ContactRequest } from '../../models/contactModels/ContactRequest';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private httpClient: HttpClient) { }

  getContacts(): Observable<Contact[]> {
    return this.httpClient.get<Contact[]>('/api/Contact');
  }

  getContactDetails(contactId: string): Observable<ContactDetailsDto> {
    return this.httpClient.get<ContactDetailsDto>(`/api/Contact/${contactId}`);
  }

  deleteContact(contactId: string, deleteContactDto: DeleteContactDto): Observable<string> {
    return this.httpClient.delete(`/api/Contact/${contactId}`, { body: deleteContactDto, responseType: 'text' });
  }

  addNewContact(contactRequest: ContactRequest): Observable<ContactDetailsDto> {
    return this.httpClient.post<ContactDetailsDto>('/api/Contact', contactRequest);
  }

  updateContact(contactId: string, contactRequest: ContactRequest): Observable<any> {
    return this.httpClient.put(`/api/Contact/${contactId}`, contactRequest);
  }
}
