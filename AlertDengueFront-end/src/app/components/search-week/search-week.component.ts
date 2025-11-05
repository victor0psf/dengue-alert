import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DengueAlert } from '../../models/dengue-alert.model';
import { ApiService } from '../../services/api.service';

@Component({
  standalone: true,
  selector: 'app-search-week',
  templateUrl: './search-week.component.html',
  styleUrls: ['./search-week.component.css'],
  imports: [CommonModule, FormsModule],
})
export class SearchWeekComponent {
  week!: number;
  year!: number;
  loading = false;
  error = '';
  result?: DengueAlert;

  constructor(private readonly api: ApiService) {}

  search() {
    if (!this.week || !this.year) {
      this.error = 'Informe Semana e Ano.';
      return;
    }

    this.loading = true;
    this.error = '';
    this.result = undefined;

    this.api.getAlertByWeekAndYear(this.week, this.year).subscribe({
      next: (res) => {
        this.result = res;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Nenhum dado encontrado para essa semana/ano.';
        this.loading = false;
      },
    });
  }
}
