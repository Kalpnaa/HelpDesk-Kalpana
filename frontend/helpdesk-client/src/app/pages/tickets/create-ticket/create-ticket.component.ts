
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { Router } from '@angular/router';

import { TicketService } from '../../../services/ticket.service';
import {
  Category,
  TicketPriority
} from '../../../models/ticket.model';

@Component({
  selector: 'app-create-ticket',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-ticket.component.html',
  styleUrl: './create-ticket.component.scss'
})
export class CreateTicketComponent implements OnInit {

  ticketForm: FormGroup;
  categories: Category[] = [];

  priorities: TicketPriority[] = [
    'Low',
    'Medium',
    'High',
    'Critical'
  ];

  loading = false;
  submitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    private router: Router
  ) {
    this.ticketForm = this.fb.group({
      title: ['', [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(150)
      ]],
      description: ['', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(2000)
      ]],
      categoryId: ['', Validators.required],
      priority: ['Medium', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading = true;

    this.ticketService.getCategories().subscribe({
      next: (response) => {
        this.categories = response;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load categories.';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.ticketForm.invalid) {
      this.ticketForm.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.ticketService.createTicket(this.ticketForm.value).subscribe({
      next: (response) => {
        this.successMessage =
          `Ticket ${response.ticketNumber} created successfully!`;

        this.submitting = false;
        this.ticketForm.reset({
          title: '',
          description: '',
          categoryId: '',
          priority: 'Medium'
        });
      },
      error: () => {
        this.errorMessage = 'Unable to create ticket. Please try again.';
        this.submitting = false;
      }
    });
  }
}
