import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TicketService } from '../../../services/ticket.service';
import { TicketResponse } from '../../../models/ticket.model';

interface Agent {
  id: number;
  fullName: string;
  email: string;
}

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manager-dashboard.component.html',
  styleUrl: './manager-dashboard.component.scss'
})
export class ManagerDashboardComponent implements OnInit {

  tickets: TicketResponse[] = [];
  agents: Agent[] = [];

  selectedAgents: { [ticketId: number]: number | null } = {};

  loading = false;
  assigningTicketId: number | null = null;

  errorMessage = '';
  successMessage = '';

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadAgents();
    this.loadAllTickets();
  }

  loadAgents(): void {
    this.ticketService.getAgents().subscribe({
      next: response => this.agents = response,
      error: () => this.errorMessage = 'Unable to load agents.'
    });
  }

  loadAllTickets(): void {
    this.loading = true;
    this.errorMessage = '';

    this.ticketService.getAllTickets().subscribe({
      next: response => {
        this.tickets = response;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load tickets.';
        this.loading = false;
      }
    });
  }

  assignTicket(ticket: TicketResponse): void {
    const agentId = this.selectedAgents[ticket.id];

    if (!agentId) {
      this.errorMessage = 'Please select an agent.';
      return;
    }

    this.assigningTicketId = ticket.id;
    this.errorMessage = '';
    this.successMessage = '';

    this.ticketService.assignTicket(ticket.id, agentId).subscribe({
      next: () => {
        const agent = this.agents.find(a => a.id === agentId);

        ticket.assignedToId = agentId.toLocaleString();
        ticket.assignedToName = agent?.fullName ?? null;

        this.successMessage = `Ticket ${ticket.ticketNumber} assigned successfully.`;
        this.assigningTicketId = null;
        this.selectedAgents[ticket.id] = null;
      },
      error: () => {
        this.errorMessage = `Unable to assign ticket ${ticket.ticketNumber}.`;
        this.assigningTicketId = null;
      }
    });
  }
}