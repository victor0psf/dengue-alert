import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DengueAlert } from '../../models/dengue-alert.model';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css'],
  imports: [CommonModule],
})
export class SyncComponent {
  loading = false;
  message = '';
  count = 0;
  error = '';
  alerts: DengueAlert[] = []; // array para armazenar os alertas

  constructor(private readonly api: ApiService) {}

  async sync() {
    this.loading = true;
    this.message = '';
    this.error = '';
    this.alerts = [];

    try {
      const res = await lastValueFrom(this.api.syncAlerts());
      console.log('sync result:', res); // <--- verifique o console para saber o formato exato

      // Ajuste automático: se res.data existe e é array, use; senão se res é array, use
      if (res?.data && Array.isArray(res.data)) {
        this.alerts = res.data;
      } else if (Array.isArray(res)) {
        this.alerts = res;
      } else {
        this.alerts = [];
      }

      this.message = res?.message ?? 'Sync completed';
      this.count = this.alerts.length; // total de alertas carregados
    } catch (err: any) {
      console.error(err);
      this.error = err?.message || 'Falha ao sincronizar';
    } finally {
      this.loading = false;
    }
  }
}
