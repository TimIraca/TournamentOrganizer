describe('Tournament Basic Flow', () => {
    beforeEach(() => {
      cy.login('admin', 'admin');
    });
  
    it('should create a tournament, add participants, start, and complete it', () => {
      cy.createTournament('Basic Flow Tournament', '12122024')
        .then((tournamentId) => {
          cy.get(`[href="/tournaments/${tournamentId}"]`).click();
          cy.get('[data-cy="edit-tournament-button"]').click();
          ['Alice', 'Bob', 'Charlie', 'Diana'].forEach((name) => {
            cy.addParticipant(name);
          });
          cy.startTournament();
          cy.get('[data-cy="return-to-tournament-button"]').click();
          cy.declareWinner(1);
          cy.declareWinner(2);
          cy.declareWinner(3);
          cy.get('[data-cy="edit-tournament-button"]').click();
          cy.get('[data-cy="delete-tournament-button"]').click();
          cy.get('[data-cy="confirm-delete-button"]').click();
          cy.get(`[href="/tournaments/${tournamentId}"]`).should('not.exist');
        });
    });
  });