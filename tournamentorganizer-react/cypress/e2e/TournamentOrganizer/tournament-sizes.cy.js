const participantCounts = [6, 9, 50];

describe('Tournament Size Variations', () => {
  beforeEach(() => {
    cy.login('admin', 'admin');
  });

  participantCounts.forEach((count) => {
    it(`should handle a tournament with ${count} participants`, () => {
      cy.createTournament(`Tournament with ${count} participants`, '12122024')
        .then((tournamentId) => {
          cy.get(`[href="/tournaments/${tournamentId}"]`).click();
          cy.get('[data-cy="edit-tournament-button"]').click();

          for (let i = 1; i <= count; i++) {
            cy.addParticipant(`Participant ${i}`);
          }

          cy.startTournament();

          const totalMatches = count - 1;

          for (let matchNumber = 1; matchNumber <= totalMatches; matchNumber++) {
            cy.declareWinner(matchNumber);
          }

          cy.get('[data-cy="edit-tournament-button"]').click();
          cy.get('[data-cy="delete-tournament-button"]').click();
          cy.get('[data-cy="confirm-delete-button"]').click();
          cy.get(`[href="/tournaments/${tournamentId}"]`).should('not.exist');
        });
    });
  });
});
