describe('Create Tournament', () => {
    it('should create a tournament successfully', () => {
        cy.intercept('POST', '**/api/Tournaments').as('createTournament')

        cy.visit('http://localhost:3000/auth/login/')
        cy.get('#username').type('admin')
        cy.get('#password').type('admin')
        cy.get('[data-cy="login-button"]').click()
        cy.get('[data-cy="create-tournament-button"]').click()
        cy.get('div[role="dialog"]').should('be.visible')
        cy.get('input[id="name"]').type('Tournament 1')
        cy.get('input[id="startDate"]').type('2024-12-12')
        cy.get('button[type="submit"]').click()
        
        cy.wait('@createTournament').then((interception) => {
            expect(interception.response.statusCode).to.eq(201)
            const tournamentId = interception.response.body.id
        
            cy.get(`[href="/tournaments/${tournamentId}"]`).should('exist')
        })
        cy.get('div[role="dialog"]').should('not.exist')
    })
})