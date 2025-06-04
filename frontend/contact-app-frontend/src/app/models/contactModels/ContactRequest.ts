export interface ContactRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    phoneNumber: string;
    dateOfBirth: string;
    categoryId: string;
    subcategoryId: string | null; 
    subcategoryName: string | null;
}