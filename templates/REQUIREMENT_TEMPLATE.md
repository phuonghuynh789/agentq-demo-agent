# Test Case Generation Template

Use this template when requesting test case generation. Fill out as much information as you can to get the most comprehensive and accurate test cases.

---

## Project & Feature Information

**Project Name:** [Your Project Name]

**Feature/Module:** [What feature needs testing?]

**Feature ID/Reference:** [JIRA, GitHub Issue, or internal reference]

---

## Requirement Description

### Overview
[Provide a clear, concise description of what the feature does]

### User Story (if applicable)
```
As a [user role]
I want to [action/capability]
So that [business value/benefit]
```

### Acceptance Criteria
- [ ] [Criterion 1]
- [ ] [Criterion 2]
- [ ] [Criterion 3]

### Detailed Requirements
[List all functional and non-functional requirements]

1. **Functional Requirements:**
   - [Requirement 1]
   - [Requirement 2]

2. **Non-Functional Requirements:**
   - Performance: [e.g., Response time < 200ms]
   - Security: [e.g., HTTPS, encryption]
   - Accessibility: [e.g., WCAG 2.1 Level AA]
   - Browser Support: [e.g., Chrome, Firefox, Safari]

---

## Context & Constraints

### Business Context
[Why is this feature important? What problem does it solve?]

### User Roles
- [Role 1]: [Description]
- [Role 2]: [Description]

### Integration Points
[What other systems/features does this interact with?]

### Known Constraints
- [Constraint 1]
- [Constraint 2]

---

## Technical Details (if applicable)

### API Endpoints
```
[Method] /api/endpoint
Request: [Request structure]
Response: [Response structure]
```

### Database/Data Model
[Relevant data structure information]

### Error Scenarios
- [Error 1]: [Expected behavior]
- [Error 2]: [Expected behavior]

---

## Testing Scope

### What to Test
- [ ] Positive scenarios (happy path)
- [ ] Negative scenarios (error handling)
- [ ] Security testing
- [ ] Performance testing
- [ ] Edge cases/boundary testing
- [ ] UI/UX testing
- [ ] API testing
- [ ] Integration testing

### What NOT to Test
[Anything explicitly out of scope]

### Priority Focus
[If you want emphasis on specific areas: security, performance, etc.]

---

## Additional Information

### Screenshots/Wireframes
[Attach or link to UI mockups if applicable]

### Sample Test Data
[Provide examples of data to use in testing]

### External Documentation
[Links to API docs, design docs, requirements specs, etc.]

### Questions/Clarifications Needed
[Any uncertainties about the requirement]

---

## Submission Example

Here's how to submit your requirement:

```
I need comprehensive test cases for the following:

**Feature:** User Payment Processing

**Overview:** 
Users should be able to pay for orders using credit/debit cards, PayPal, or Apple Pay.

**Acceptance Criteria:**
- Accept Visa, Mastercard, American Express
- Support one-time and recurring payments
- Display clear error messages for failed payments
- Process payments within 5 seconds
- PCI DSS compliance

**Requirements:**
- Validate card details
- Handle 3D Secure authentication
- Support multiple payment methods
- Handle network failures gracefully
- Log all transactions

**Testing Scope:**
- Functional: Payment flow, validation, multiple methods
- Security: PCI compliance, data encryption
- Performance: Payment processing time
- Edge Cases: Network failures, invalid cards

[Include more details as needed]
```

