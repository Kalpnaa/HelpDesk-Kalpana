import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TicketService } from '../../../services/ticket.service';
import { TicketResponse, TicketStatus } from '../../../models/ticket.model';

@Component({
  selector: 'app-assigned-tickets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './assigned-tickets.component.html',
  styleUrl: './assigned-tickets.component.scss',
})
export class AssignedTicketsComponent implements OnInit {
  tickets: TicketResponse[] = [];
  loading = false;
  errorMessage = '';
  updatingTicketIds = new Set<number>();

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadAssignedTickets();
  }

  loadAssignedTickets(): void {
    this.loading = true;
    this.errorMessage = '';

    this.ticketService.getAssignedTickets().subscribe({
      next: (response) => {
        this.tickets = response;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load assigned tickets.';
        this.loading = false;
      },
    });
  }

  isUpdating(ticketId: number): boolean {
    return this.updatingTicketIds.has(ticketId);
  }

  updateStatus(ticket: TicketResponse, status: string): void {
    const validStatuses: TicketStatus[] = [
      'New',
      'Open',
      'Pending',
      'Resolved',
      'Closed',
    ];

    if (!validStatuses.includes(status as TicketStatus)) {
      return;
    }

    this.updatingTicketIds = new Set([...this.updatingTicketIds, ticket.id]);

    this.errorMessage = '';

    this.ticketService
      .updateTicketStatus(ticket.id, status as TicketStatus)
      .subscribe({
        next: (response) => {
          ticket.status = response.status;

          const updatedIds = new Set(this.updatingTicketIds);
          updatedIds.delete(ticket.id);
          this.updatingTicketIds = updatedIds;
        },
        error: () => {
          this.errorMessage = `Unable to update ticket ${ticket.ticketNumber}.`;

          const updatedIds = new Set(this.updatingTicketIds);
          updatedIds.delete(ticket.id);
          this.updatingTicketIds = updatedIds;
        },
      });
  }
}
