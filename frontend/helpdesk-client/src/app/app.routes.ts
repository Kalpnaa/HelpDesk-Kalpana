import { Routes } from '@angular/router';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';
import { CreateTicketComponent } from './pages/tickets/create-ticket/create-ticket.component';
import { MyTicketsComponent } from './pages/tickets/my-tickets/my-tickets.component';

export const routes: Routes = [
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
 
  {
  path: 'customer-dashboard',
  component: DashboardComponent,
  canActivate: [authGuard, roleGuard(['Customer'])]
},
{
  path: 'customer/create-ticket',
  component: CreateTicketComponent,
  canActivate: [authGuard, roleGuard(['Customer'])]
},
{
  path: 'customer/my-tickets',
  component: MyTicketsComponent,
  canActivate: [authGuard, roleGuard(['Customer'])]
},
{
  path: 'agent-dashboard',
  component: DashboardComponent,
  canActivate: [authGuard, roleGuard(['Agent'])]
},
{
  path: 'manager-dashboard',
  component: DashboardComponent,
  canActivate: [authGuard, roleGuard(['Manager'])]
},
{
  path: 'admin-dashboard',
  component: DashboardComponent,
  canActivate: [authGuard, roleGuard(['Admin'])]
},
{
  path: 'unauthorized',
  component: UnauthorizedComponent
},

 {
    path: '**',
    redirectTo: 'login',
  },
];
