import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { CreateCustomerComponent } from './create-customer/create-customer.component';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: CustomerListComponent },
      { path: 'customers/:customerId', component: CustomerDetailsComponent },
      { path: 'create-customer', component: CreateCustomerComponent },
    ])
  ],
  declarations: [
    AppComponent,
    TopBarComponent,
    CustomerListComponent,
    CustomerDetailsComponent,
    CreateCustomerComponent
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }