import { Routes } from '@angular/router';
import { LastThreeComponent } from './components/last-three/last-three.component';
import { SearchWeekComponent } from './components/search-week/search-week.component';
import { SyncComponent } from './components/sync/sync.component';

export const routes: Routes = [
  { path: '', component: LastThreeComponent },
  { path: 'search', component: SearchWeekComponent },
  { path: 'sync', component: SyncComponent },
  { path: '**', redirectTo: '' },
];
