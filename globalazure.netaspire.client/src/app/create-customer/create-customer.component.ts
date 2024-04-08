import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { CustomerService } from '../customer.service';
import { CreateCustomerRequest } from '../customer';

@Component({
  selector: 'app-create-customer',
  templateUrl: './create-customer.component.html',
  styleUrl: './create-customer.component.css'
})
export class CreateCustomerComponent {
  createCustomerForm = this.formBuilder.group({
    firstName: '',
    lastName: '',
    gitHubUsername: ''
  });

  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder,
  ) {}

  onSubmit(): void {
    // Process checkout data here
    console.log('The create customer request has been submitted', this.createCustomerForm.value);
    this.customerService.addCustomer(this.createCustomerForm.value as CreateCustomerRequest)
        .subscribe(newCustomer => alert("New customer with id: " + newCustomer.id));
    this.createCustomerForm.reset();
  }
}
