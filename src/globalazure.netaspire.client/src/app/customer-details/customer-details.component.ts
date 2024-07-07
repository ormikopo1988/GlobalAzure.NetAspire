import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { Customer } from '../customer';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-customer-details',
  templateUrl: './customer-details.component.html',
  styleUrl: './customer-details.component.css'
})
export class CustomerDetailsComponent {
  customer: Customer | undefined;

  constructor(
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.getCustomer();
  }

  getCustomer(): void {
    const id = this.route.snapshot.paramMap.get('customerId');
    console.log(this.route.snapshot.paramMap);
    this.customerService.getCustomer(id!)
      .subscribe(customer => this.customer = customer);
  }

  goBack(): void {
    this.location.back();
  }
}
