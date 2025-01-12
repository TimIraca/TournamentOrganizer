describe('Login', () => {
    it('should register a new account and login successfully', () => {
        cy.intercept('POST', '**/api/Auth/register').as('register')
        cy.intercept('POST', '**/api/Auth/login').as('login')

        cy.visit('http://localhost:3000/auth/register/')
        cy.get('#username').type('test')
        cy.get('#password').type('test')
        cy.get('#confirmPassword').type('test')
        cy.get('[data-cy="register-button"]').click()
        
        cy.wait('@register').then((interception) => {
            expect(interception.response.statusCode).to.eq(200)
        })
        cy.visit('http://localhost:3000/auth/login/')
        cy.get('#username').type('test')
        cy.get('#password').type('test')
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