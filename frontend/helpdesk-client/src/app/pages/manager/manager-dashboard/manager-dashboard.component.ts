import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../../services/auth.service';
import { TicketService } from '../../../services/ticket.service';
import { UserData } from '../../../models/auth.model';
import { TicketResponse } from '../../../models/ticket.model';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './manager-dashboard.component.html',
  styleUrl: './manager-dashboard.component.scss'
})
export class ManagerDashboardComponent implements OnInit {

  currentUser: UserData | null = null;

  tickets: TicketResponse[] = [];
  agents: any[] = [];

  loading = true;
  errorMessage = '';
  agentsError = '';

  constructor(
    private authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getUser();
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;
    this.errorMessage = '';

    // Load all tickets
    this.ticketService.getAllTickets().subscribe({
      next: (response) => {
        this.tickets = response;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tickets:', error);
        this.errorMessage = 'Unable to load tickets. Please try again.';
        this.loading = false;
      }
    });

    // Load agents
    this.ticketService.getAgents().subscribe({
      next: (response) => {
        this.agents = response;
      },
      error: (error) => {
        console.error('Error loading agents:', error);
        this.agentsError = 'Unable to load agent count.';
      }
    });
  }

  get totalTickets(): number {
    return this.tickets.length;
  }

  get unassignedTickets(): number {
    return this.tickets.filter(ticket => ticket.assignedToId === null).length;
  }

  get assignedTickets(): number {
    return this.tickets.filter(ticket => ticket.assignedToId !== null).length;
  }

  get activeTickets(): number {
    return this.tickets.filter(ticket =>
      ['New', 'Open', 'Pending'].includes(ticket.status)
    ).length;
  }

  get resolvedTickets(): number {
    return this.tickets.filter(ticket =>
      ['Resolved', 'Closed'].includes(ticket.status)
    ).length;
  }

  get totalCustomers(): number {
    return new Set(this.tickets.map(ticket => ticket.createdById)).size;
  }

  get totalAgents(): number {
    return this.agents.length;
  }

  get recentTickets(): TicketResponse[] {
    return [...this.tickets]
      .sort((a, b) =>
        new Date(b.createdAt).getTime() -
        new Date(a.createdAt).getTime()
      )
      .slice(0, 6);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}