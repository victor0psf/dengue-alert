import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DengueAlert } from '../../models/dengue-alert.model';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-last-three',
  templateUrl: './last-three.component.html',
  styleUrls: ['./last-three.component.css'],
  imports: [CommonModule],
})
export class LastThreeComponent implements OnInit {
  loading = false;
  alerts: DengueAlert[] = [];
  lastThree: DengueAlert[] = [];
  error = '';
  Math = Math;

  constructor(private readonly api: ApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.api.getAllAlerts().subscribe({
      next: (arr) => {
        this.alerts = arr || [];
        // Normalize: compute Year and Week number for sorting
        const normalized = this.alerts.map((a) => {
          const weekNum = a.SE ?? 0;
          // If SE is encoded as 202542 (year+week) try to parse
          let year = a.epidemiologicalYear ?? 0;
          let week = 0;
          if (!year && weekNum > 1000) {
            // e.g. 202542 => year=2025, week=42
            year = Math.floor(weekNum / 100);
            week = weekNum % 100;
          } else {
            week = weekNum;
          }
          return { ...a, _year: year, _week: week };
        });

        // sort by year/week desc
        normalized.sort((x, y) => {
          if (x._year !== y._year) return y._year - x._year;
          return y._week - x._week;
        });

        // pick unique (week+year) and take first 3
        const picked: DengueAlert[] = [];
        const seen = new Set<string>();
        for (const n of normalized) {
          const key = `${n._year}-${n._week}`;
          if (!seen.has(key)) {
            seen.add(key);
            picked.push(n);
            if (picked.length === 3) break;
          }
        }

        this.lastThree = picked;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Failed to load alerts';
        this.loading = false;
      },
    });
  }
}
