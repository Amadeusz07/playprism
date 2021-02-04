import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Discipline } from '../models/discipline.model';

@Injectable({
  providedIn: 'root'
})
export class DisciplineService {
  private API_URL = environment.API_URL_TOURNAMENT;

  constructor(private http: HttpClient) { }

  public async getDisciplines(): Promise<Discipline[]> {
    return await this.http.get<Discipline[]>(`${this.API_URL}/discipline`).toPromise();
  }
}
