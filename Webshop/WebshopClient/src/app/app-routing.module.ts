import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './helpers/auth.guard';
import { Role } from './models/role';
import { AdminCategoryComponent } from './components/admin-category.component';
import { AdminProductComponent } from './components/admin-product.component';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/frontpage.component')
      .then(_ => _.FrontpageComponent)
  },
  {
    path: 'product/:productId',
    loadComponent: () => import('./components/prodcut-detail.component')
      .then(_ => _.ProdcutDetailComponent)
  },
  {
    path: 'cart',
    loadComponent: () => import('./components/cart.component').then(_ => _.CartComponent)
  },
  {
    path: 'cart/address',
    loadComponent: () => import('./components/address-checkout.component')
      .then(_ => _.AddressCheckoutComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login.component')
      .then(_ => _.LoginComponent)
  },
  {
    path:'register',
    loadComponent: () => import('./components/register.component')
     .then(_ => _.RegisterComponent) 
  },
  {
    path:'admin/order',
    loadComponent: () => import('./components/order-admin.component')
     .then(_ => _.OrderAdminComponent),
     canActivate: [AuthGuard], data: { roles: [Role.Admin] }
     
  },
  {
    path: 'admin/order/Order/:orderId',
    loadComponent: () => import('./components/order-detail.component')
      .then(_ => _.OrderDetailComponent) ,
      canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  },
  {
    path:'admin/customers',
    loadComponent: () => import('./components/admin-customers.component')
     .then(_ => _.AdminUsersComponent) ,
     canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  },
  {
    path: 'admin/customers/customer/:customerId',
    loadComponent: () => import('./components/admin-customers-detail.component')
      .then(_ => _.AdminCustomersDetailComponent),
      canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  },
  {
    path: 'admin/product',
    loadComponent: () => import('./components/admin-product.component')
      .then(_ => _.AdminProductComponent),
      canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  },
  {
    path: 'admin/category',
    loadComponent: () => import('./components/admin-category.component')
      .then(_ => _.AdminCategoryComponent),
      canActivate: [AuthGuard], data: { roles: [Role.Admin] }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
