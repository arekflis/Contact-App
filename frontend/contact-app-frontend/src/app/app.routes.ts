import { Routes } from '@angular/router';
import { LoginComponent } from './components/authComponents/login-component/login-component';
import { RegisterComponent } from './components/authComponents/register-component/register-component';
import { ContactsComponent } from './components/contactComponents/contacts-component/contacts-component';
import { ContactDetails } from './components/contactComponents/contact-details/contact-details';
import { ContactDelete } from './components/contactComponents/contact-delete/contact-delete';
import { authGuard } from './services/guards/auth-guard';
import { ContactUpdate } from './components/contactComponents/contact-update/contact-update';
import { ContactAdd } from './components/contactComponents/contact-add/contact-add';

export const routes: Routes = [
    {path: '', redirectTo: 'contacts', pathMatch: 'full'},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'contacts', component: ContactsComponent},
    {path: 'contact/add', component: ContactAdd, canActivate: [authGuard]},
    {path: 'contact/delete/:id', component: ContactDelete, canActivate: [authGuard]},
    {path: 'contact/edit/:id', component: ContactUpdate, canActivate: [authGuard]},
    {path: 'contact/:id', component: ContactDetails},
    {path: '**', redirectTo: 'contacts'}
];
