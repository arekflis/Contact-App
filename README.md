# Contact-App

## How to Run This Application

Below is a step-by-step guide to run the entire project, which consists of three backend microservices and an Angular frontend.

---

### 1. Clone the repository

```bash
git clone https://github.com/arekflis/Contact-App.git
cd Contact-App
```

---

### 2. Run the authorization microservice (authMicroservice)

```bash
cd backend/authMicroservice/authMicroservice
dotnet restore
dotnet run
```

The application will be available at:  
[http://localhost:5265](http://localhost:5265)

---

### 3. Run the contacts microservice (contactMicroservice)

```bash
cd backend/contactMicroservice/contactMicroservice
dotnet restore
dotnet run
```

The application will be available at:  
[http://localhost:5266](http://localhost:5266)

---

### 4. Run the gateway microservice (gatewayMicroservice)

```bash
cd backend/gatewayMicroservice/gatewayMicroservice
dotnet restore
dotnet run
```

The application will be available at:  
[http://localhost:5236](http://localhost:5236)

---

### 5. Run the Angular frontend

```bash
cd frontend/contact-app-frontend
ng serve
```

The frontend application will be available at:  
[http://localhost:4200](http://localhost:4200)

---

## Prerequisites

- Make sure you have installed:  
  - [.NET SDK (version 6 or higher)](https://dotnet.microsoft.com/en-us/download)  
  - [Node.js](https://nodejs.org/) and [Angular CLI](https://angular.io/cli)  

---

If you encounter any issues, feel free to reach out.
