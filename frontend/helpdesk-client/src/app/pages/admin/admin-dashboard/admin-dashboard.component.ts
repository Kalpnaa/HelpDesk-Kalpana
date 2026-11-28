import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../../services/auth.service';
import { TicketService } from '../../../services/ticket.service';
import { UserData } from '../../../models/auth.model';
import { TicketResponse } from '../../../models/ticket.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent implements OnInit {

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

    // Fetch organization-wide tickets
    this.ticketService.getAllTickets().subscribe({
      next: (response) => {
        this.tickets = response;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tickets:', error);
        this.errorMessage = 'Unable to load dashboard information.';
        this.loading = false;
      }
    });

    // Fetch support agents
    this.ticketService.getAgents().subscribe({
      next: (response) => {
        this.agents = response;
        this.agentsError = '';
      },
      error: (error) => {
        console.error('Error loading agents:', error);
        this.agentsError = 'Unable to load agent information.';
      }
    });
  }

  get totalTickets(): number {
    return this.tickets.length;
  }

  get unassignedTickets(): number {
    return this.tickets.filter(
      ticket => ticket.assignedToId === null
    ).length;
  }

  get assignedTickets(): number {
    return this.tickets.filter(
      ticket => ticket.assignedToId !== null
    ).length;
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

  get criticalTickets(): number {
    return this.tickets.filter(
      ticket => ticket.priority === 'Critical'
    ).length;
  }

  get totalCustomers(): number {
    return new Set(
      this.tickets.map(ticket => ticket.createdById)
    ).size;
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