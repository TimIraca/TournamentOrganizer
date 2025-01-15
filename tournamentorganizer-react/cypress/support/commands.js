Cypress.Commands.add('login', (username, password) => {
  cy.visit('http://localhost:3000/auth/login');
  cy.get('#username').type(username);
  cy.get('#password').type(password);
  cy.get('[data-cy="login-button"]').click();
});

Cypress.Commands.add('createTournament', (name, startDate) => {
  cy.intercept('POST', '**/api/Tournaments').as('createTournament');
  cy.get('[data-cy="create-tournament-button"]').click();
  
  cy.get('div[role="dialog"]').should('be.visible');
  
  cy.get('input[id="name"]').type(name);
  cy.get('input[id="startDate"]').type(startDate);
  cy.get('button[type="submit"]').click();

  cy.wait('@createTournament').then((interception) => {
    expect(interception.response.statusCode).to.eq(201);
    return interception.response.body.id;
  });
});

Cypress.Commands.add('addParticipant', (participantName) => {
  cy.get('[data-cy="new-participant-input"]').type(participantName);
  cy.get('[data-cy="add-participant"]').click();
});

Cypress.Commands.add('startTournament', () => {
  cy.intercept('POST', '**/api/Tournaments/*/start').as('startTournament');
  cy.get('[data-cy="start-tournament-button"]').click();
  cy.get('[data-cy="confirm-delete-button"]').click();
  cy.wait('@startTournament').its('response.statusCode').should('eq', 200);
});

Cypress.Commands.add('declareWinner', (matchNumber) => {
  cy.get(`[data-match-number="${matchNumber}"]`)
    .find('[data-cy$="-trigger"]')
    .first()
    .click();
    cy.get('[data-cy^="declare-winner-"]').click();
    cy.get(`[data-match-number="${matchNumber}"]`)
    .find('[data-cy="winnerbadge"]')
    .should('exist');
});
