import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../../services/auth.service';
import { TicketService } from '../../../services/ticket.service';
import { UserData } from '../../../models/auth.model';
import { TicketResponse } from '../../../models/ticket.model';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './customer-dashboard.component.html',
  styleUrl: './customer-dashboard.component.scss'
})
export class CustomerDashboardComponent implements OnInit {

  currentUser: UserData | null = null;

  tickets: TicketResponse[] = [];

  loading = true;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getUser();
    this.loadMyTickets();
  }

  loadMyTickets(): void {
    this.loading = true;
    this.errorMessage = '';

    this.ticketService.getMyTickets().subscribe({
      next: (response) => {
        this.tickets = response;
        this.loading = false;
      },
      error: (error) => {
        console.error('Unable to load tickets:', error);
        this.errorMessage = 'Unable to load your tickets. Please try again.';
        this.loading = false;
      }
    });
  }

  get totalTickets(): number {
    return this.tickets.length;
  }

  get newTickets(): number {
    return this.tickets.filter(ticket => ticket.status === 'New').length;
  }

  get inProgressTickets(): number {
    return this.tickets.filter(
      ticket => ticket.status === 'Open' || ticket.status === 'Pending'
    ).length;
  }

  get resolvedTickets(): number {
    return this.tickets.filter(
      ticket => ticket.status === 'Resolved' || ticket.status === 'Closed'
    ).length;
  }

  get recentTickets(): TicketResponse[] {
    return [...this.tickets]
      .sort(
        (a, b) =>
          new Date(b.createdAt).getTime() -
          new Date(a.createdAt).getTime()
      )
      .slice(0, 5);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}