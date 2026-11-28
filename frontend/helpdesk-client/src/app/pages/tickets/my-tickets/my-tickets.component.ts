
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TicketService } from '../../../services/ticket.service';
import { TicketResponse } from '../../../models/ticket.model';

@Component({
  selector: 'app-my-tickets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-tickets.component.html',
  styleUrl: './my-tickets.component.scss'
})
export class MyTicketsComponent implements OnInit {

  tickets: TicketResponse[] = [];
  loading = false;
  errorMessage = '';

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
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
      error: () => {
        this.errorMessage = 'Unable to load your tickets.';
        this.loading = false;
      }
    });
  }
}
