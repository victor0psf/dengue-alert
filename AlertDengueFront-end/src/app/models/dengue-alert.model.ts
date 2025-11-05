export interface DengueAlert {
  id: number;
  data_ini_SE: string;
  SE: number; // week (ex: 202542 - or numeric week code)
  epidemiologicalYear?: number;
  casos_est?: number;
  casos_est_min?: number;
  casos_est_max?: number;
  casos?: number;
  p_rt1?: number;
  p_inc100k?: number;
  nivel?: number;
  versao_modelo?: string;
  Rt?: number;
  pop?: number;
  tempmin?: number;
  tempmed?: number;
  tempmax?: number;
  umidmin?: number;
  umidmed?: number;
  umidmax?: number;
  receptivo?: number;
  transmissao?: number;
  nivel_inc?: number;
  notif_accum_year?: number;
}
