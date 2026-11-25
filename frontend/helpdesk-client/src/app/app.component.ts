import { Component, OnInit } from '@angular/core';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  apiMessage = 'Connecting to backend...';

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.checkHealth().subscribe({
      next: (response: any) => {
        this.apiMessage = response.message;
      },
      error: (error) => {
        this.apiMessage = 'Backend connection failed!';
        console.error(error);
      }
    });
  }
}