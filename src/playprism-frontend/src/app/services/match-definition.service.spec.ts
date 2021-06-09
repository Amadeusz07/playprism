import { TestBed } from '@angular/core/testing';

import { MatchDefinitionService } from './match-definition.service';

describe('MatchDefinitionService', () => {
  let service: MatchDefinitionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MatchDefinitionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
