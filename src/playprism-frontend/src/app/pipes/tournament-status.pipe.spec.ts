import { TournamentStatusPipe } from './tournament-status.pipe';

describe('TournamentStatusPipe', () => {
  it('create an instance', () => {
    const pipe = new TournamentStatusPipe();
    expect(pipe).toBeTruthy();
  });
});
