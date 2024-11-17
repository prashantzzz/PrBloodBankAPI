# Blood Bank Management API by Prashant

## Project Overview
This is a RESTful API built with ASP.NET Core for managing blood bank entries with CRUD operations, searching, filtering, sorting, and pagination.

## Technologies Used
- ASP.NET Core 6.0
- C#
- Swagger
- In-memory data storage

## Setup Instructions

### Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (or later)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 

### Installation Steps
1. Clone the repository
   ```bash
   git clone https://github.com/prashantzzz/PrBloodBankAPI.git
   ```

2. Open the solution in Visual Studio
   ```bash
   cd PrBloodBankAPI
   start PrBloodBankAPI.sln
   ```

3. Build & Run the application

## API Endpoints

### CRUD Operations

#### Get All Entries
```http
GET /api/bloodbank
```

#### Get Entry by ID
```http
GET /api/bloodbank/{id}
```
#### Create Blood Bank Entry
```http
POST /api/bloodbank
```
```json
{
  "donorName": "John Doe",
  "age": 28,
  "bloodType": "A+",
  "contactInfo": "john@example.com",
  "quantity": 450,
  "collectionDate": "2024-11-17T10:00:00",
  "expirationDate": "2024-12-29T10:00:00",
  "status": "Available"
}
```

#### Update Entry
```http
PUT /api/bloodbank/{id}
```

#### Delete Entry
```http
DELETE /api/bloodbank/{id}
```

### Advanced Features

#### Pagination
```http
GET /api/bloodbank/page?page={pageNumber}&size={pageSize}
```
Parameters:
- `page`: Page number (default: 1)
- `size`: Items per page (default: 10)

#### Sorting
```http
GET /api/bloodbank?sortBy={field}&sortOrder={order}
```
Parameters:
- `sortBy`: Field to sort by (bloodtype, collectiondate, status, etc.)
- `sortOrder`: "asc" or "desc"

#### Filtering
```http
GET /api/bloodbank?bloodType={type}&status={status}&donorName={name}
```
Available filters:
- `bloodType`: Blood type (e.g., "A+", "O-")
- `status`: Entry status (e.g., "Available", "Requested")
- `donorName`: Donor's name (partial match)
- `startDate`: Collection date range start
- `endDate`: Collection date range end
- `minAge`: Minimum donor age
- `maxAge`: Maximum donor age

### example API Calls

1. GET all available A+ blood donations, sorted by collection date:
```http
GET https://localhost:7106/api/bloodbank?bloodType=A+&status=Available&sortBy=collectiondate&sortOrder=desc
```

2. GET page 2 of entries with 4 items per page:
```http
GET https://localhost:7106/api/bloodbank/page?page=3&size=4
```

3. Find by donor name containing "Prashant" and specific age range:
```http
GET https://localhost:7106/api/bloodbank?donorName=Prahsant&minAge=20&maxAge=50
```

## Model Validation Rules
- DonorName: Required, max length 100 characters
- Age: Required, range 18-65
- BloodType: Required, max length 3 characters
- ContactInfo: Required, max length 100 characters
- Quantity: Required, range 200-500 ml
- CollectionDate: Required
- ExpirationDate: Required, must be after collection date
- Status: Required, max length 20 characters

## Error Handling
The API includes comprehensive error handling:
- 404 Not Found: When requested resource doesn't exist
- 400 Bad Request: For invalid input data
- 200 OK: Successful GET requests
- 201 Created: Successful POST requests
- 204 No Content: Successful PUT/DELETE requests
