export interface Category {
  id: number;
  name: string;
  description: string;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  categoryId: number;
  priority: string;
}

export interface TicketResponse {
  id: number;
  ticketNumber: string;
  title: string;
  description: string;
  categoryId: number;
  categoryName: string;
  priority: string;
  status: string;
  createdById: string;
  createdByName: string;
  assignedToId: string | null;
  assignedToName: string | null;
  createdAt: string;
  updatedAt: string | null;
}


export type TicketPriority = 'Low' | 'Medium' | 'High' | 'Critical';

export type TicketStatus =
  | 'New'
  | 'Open'
  | 'Pending'
  | 'Resolved'
  | 'Closed';