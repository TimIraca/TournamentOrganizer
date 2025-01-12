describe('Login', () => {
    it('should login and create tournament successfully', () => {
        cy.intercept('POST', '**/api/Auth/login').as('login')

        cy.visit('http://localhost:3000/auth/login/')
        cy.get('#username').type('admin')
        cy.get('#password').type('admin')
        cy.get('[data-cy="login-button"]').click()
        
        cy.wait('@login').then((interception) => {
            expect(interception.response.statusCode).to.eq(200)
        })
        cy.window().its('localStorage')
        .invoke('getItem', 'token')
        .should('exist')
        .and('not.be.null')
    })
})