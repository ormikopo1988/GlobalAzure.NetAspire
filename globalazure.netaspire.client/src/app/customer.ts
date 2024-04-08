export interface Customer {
  id: string;
  fullName: string;
  gitHubUsername: string;
}

export interface CreateCustomerRequest {
  id: string;
  firstName: string;
  lastName: string;
  gitHubUsername: string;
}