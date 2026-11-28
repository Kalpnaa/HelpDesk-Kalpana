
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Category,
  CreateTicketRequest,
  TicketResponse
} from '../models/ticket.model';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private apiUrl = 'http://localhost:5029/api';

  constructor(private http: HttpClient) {}

  // Get all active categories
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/Categories`);
  }

  // Customer creates a ticket
  createTicket(data: CreateTicketRequest): Observable<TicketResponse> {
    return this.http.post<TicketResponse>(
      `${this.apiUrl}/Tickets`,
      data
    );
  }

  // Customer gets their own tickets
  getMyTickets(): Observable<TicketResponse[]> {
    return this.http.get<TicketResponse[]>(
      `${this.apiUrl}/Tickets/my-tickets`
    );
  }

  // Agent gets assigned tickets
  getAssignedTickets(): Observable<TicketResponse[]> {
    return this.http.get<TicketResponse[]>(
      `${this.apiUrl}/Tickets/assigned`
    );
  }

  // Manager/Admin gets all tickets
  getAllTickets(): Observable<TicketResponse[]> {
    return this.http.get<TicketResponse[]>(
      `${this.apiUrl}/Tickets`
    );
  }
}
