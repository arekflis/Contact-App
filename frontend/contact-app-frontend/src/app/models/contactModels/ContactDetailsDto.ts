import { Category } from "../categoryModels/Category";
import { Subcategory } from "../categoryModels/Subcategory";

export interface ContactDetailsDto {
    contactId: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: Date;
    category: Category;
    subcategory?: Subcategory;
}