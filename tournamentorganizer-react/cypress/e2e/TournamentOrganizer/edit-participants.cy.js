describe('Edit Participants', () => {
    beforeEach(() => {
      cy.login('admin', 'admin');
    });
  
    it('should add, edit, and delete a participant in a newly created tournament', () => {
      cy.createTournament('Cypress Test Tournament', '2025-03-01').then((tournamentId) => {
        cy.get(`[href="/tournaments/${tournamentId}"]`).click();
        cy.get('[data-cy="edit-tournament-button"]').click();
  
        cy.get('[data-cy="new-participant-input"]').type('New Participant');
        cy.get('[data-cy="add-participant"]').click();
  
        cy.get('[data-cy="participant-row"]').last().within(() => {
          cy.get('[data-cy="participant-name-input"]')
            .clear()
            .type('Renamed Participant', { delay: 100 });
        });
  
        cy.get('[data-cy="participant-row"]')
          .last()
          .within(() => {
            cy.get('[data-cy="participant-name-input"]')
              .should('have.value', 'Renamed Participant');
          });
  
        cy.get('[data-cy="participant-row"]')
          .last()
          .within(() => {
            cy.get('[data-cy="delete-participant-button"]').click();
          });
      });
    });
  });