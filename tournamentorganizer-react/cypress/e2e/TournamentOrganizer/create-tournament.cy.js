describe('Create Tournament', () => {
    it('should navigate to the tournaments page', () => {
        cy.visit('localhost:3000/tournaments')
        cy.get('.bg-primary').click()
        cy.get('div[role="dialog"]').should('be.visible')
        cy.get('input[id="name"]').type('Tournament 1')
        cy.get('input[id="startDate"]').type('2024-12-11')
        cy.get('button[type="submit"]').click()
        cy.get('div[role="dialog"]').should('not.exist')
    })
})
