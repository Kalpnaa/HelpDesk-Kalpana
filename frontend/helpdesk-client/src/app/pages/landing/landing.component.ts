
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {

  benefits = [
    {
      icon: '🎫',
      title: 'Easy Ticket Creation',
      description: 'Raise support requests quickly and share all the details your support team needs.'
    },
    {
      icon: '⚡',
      title: 'Faster Resolution',
      description: 'Tickets are assigned to the right support agents for efficient problem solving.'
    },
    {
      icon: '📊',
      title: 'Track Every Request',
      description: 'Stay updated on ticket progress, priority and resolution status in one place.'
    }
  ];

}
