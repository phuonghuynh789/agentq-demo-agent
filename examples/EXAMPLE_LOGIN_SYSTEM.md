# Test Case Generator Examples

## Example 1: User Authentication System

### Sample Requirement

```
Feature: User Login System

Users should be able to log in to the application using their email and password.

Requirements:
- Users can enter email and password in a login form
- System validates credentials against the database
- On successful login, user is redirected to dashboard
- On failed login, display error message and clear password field
- Account should be locked after 5 failed login attempts
- Password must be at least 8 characters with uppercase, lowercase, numbers, and special characters
- Login session expires after 30 minutes of inactivity
- Support "Remember Me" functionality (valid for 7 days)
- Implement rate limiting (max 10 login attempts per minute per IP)
```

### Expected Agent Output

```
# Test Case Generation Report: User Login System

## Step-by-Step Reasoning

### Requirement Analysis
The login system is a critical authentication feature that:
- Validates user credentials
- Manages sessions and access
- Handles errors gracefully
- Implements security measures (account lockout, rate limiting)
- Provides user convenience (remember me)
- Manages session expiry

### Key Features to Test
1. Credential validation
2. Successful login flow
3. Failed login handling
4. Account lockout mechanism
5. Password requirements
6. Session management
7. Remember me functionality
8. Rate limiting
9. Session expiry
10. UI/UX aspects
11. Security aspects

### Test Scenarios Identified

**Happy Path:**
- Valid email and password login
- Login with remember me selected
- Dashboard redirect after successful login

**Error Paths:**
- Invalid email format
- Invalid password
- Non-existent email
- Wrong password
- Expired session trying to access protected resource

**Security Scenarios:**
- SQL injection attempts
- Account lockout after multiple failures
- Rate limiting on multiple attempts
- Session expiry after 30 minutes

**Edge Cases:**
- Max length email/password
- Special characters in password
- Simultaneous login attempts
- Login after logout
- Remember me expiration

### Test Type Distribution
- Functional Testing: 60% (core login flow, validation, lockout, remember me)
- Security Testing: 25% (injection prevention, rate limiting, session management)
- Performance Testing: 10% (login response time)
- Edge Cases: 5% (boundary conditions, concurrent access)

### Priority Assessment
- High: Core login functionality, security features (lockout, rate limiting)
- Medium: Remember me, session management, error messages
- Low: Edge cases with max values, concurrent scenarios

## Test Cases

| TC ID | Test Case Name | Type | Priority | Description | Preconditions | Steps | Expected Result |
|-------|---|---|---|---|---|---|---|
| TC_LOGIN_001 | Successful login with valid credentials | Functional | High | Verify user can login with correct email and password | User account exists with valid credentials | 1. Navigate to login page<br/>2. Enter email: user@example.com<br/>3. Enter password: SecurePass123!<br/>4. Click Login button | 1. Login form displays<br/>2. Email entered successfully<br/>3. Password masked in field<br/>4. User redirected to dashboard with welcome message |
| TC_LOGIN_002 | Login with invalid email format | Functional | High | Verify system rejects invalid email formats | Login page is accessible | 1. Navigate to login page<br/>2. Enter email: invalidemail<br/>3. Enter password: SecurePass123!<br/>4. Click Login button | 1. Error message displays: "Invalid email format"<br/>2. Login not attempted<br/>3. User remains on login page |
| TC_LOGIN_003 | Login with non-existent email | Functional | High | Verify appropriate error for non-existent user | Login page is accessible | 1. Navigate to login page<br/>2. Enter email: nonexistent@example.com<br/>3. Enter password: SecurePass123!<br/>4. Click Login button | 1. Generic error displays: "Invalid email or password"<br/>2. Password field cleared<br/>3. User remains on login page |
| TC_LOGIN_004 | Login with incorrect password | Functional | High | Verify user cannot login with wrong password | User account exists, login page accessible | 1. Navigate to login page<br/>2. Enter email: user@example.com<br/>3. Enter password: WrongPassword123!<br/>4. Click Login button | 1. Error message displays: "Invalid email or password"<br/>2. Password field cleared<br/>3. Failed attempt counter incremented<br/>4. User remains on login page |
| TC_LOGIN_005 | Account lockout after 5 failed attempts | Security | High | Verify account locks after 5 consecutive failed login attempts | User account exists, all previous failed attempts recorded | 1. Attempt login 5 times with wrong password<br/>2. On 5th attempt, enter any credentials<br/>3. Click Login button | 1. After 5th failure, error displays: "Account locked due to multiple failed attempts. Contact support."<br/>2. Login button disabled<br/>3. Account flagged as locked in database<br/>4. Support contact information provided |
| TC_LOGIN_006 | Rate limiting on login attempts | Security | High | Verify system blocks excessive login attempts from single IP | User IP configured, multiple failed attempts simulated | 1. Send 11 login requests within 60 seconds from same IP<br/>2. Observe 11th request | 1. First 10 requests processed<br/>2. 11th request returns 429 status: "Too many login attempts. Try again in 1 minute"<br/>3. Error message displayed to user |
| TC_LOGIN_007 | Successful login with Remember Me | Functional | Medium | Verify Remember Me functionality persists login for 7 days | User account exists, login page accessible | 1. Navigate to login page<br/>2. Enter email: user@example.com<br/>3. Enter password: SecurePass123!<br/>4. Check "Remember Me" checkbox<br/>5. Click Login button<br/>6. Close browser | 1. Login successful<br/>2. Cookie/token created with 7-day expiry<br/>3. User redirected to dashboard<br/>4. Session maintained |
| TC_LOGIN_008 | Remember Me checkbox remains unchecked by default | UI | Low | Verify Remember Me is unchecked for security | Login page is accessible | 1. Navigate to login page<br/>2. Observe Remember Me checkbox | 1. Checkbox is displayed<br/>2. Checkbox is unchecked by default |
| TC_LOGIN_009 | Password field is masked | UI | Medium | Verify password input is masked for security | Login page is accessible | 1. Navigate to login page<br/>2. Click password field<br/>3. Type password | 1. Password field accepts input<br/>2. Each character displays as dot (•) or asterisk (*)<br/>3. Password value not visible in plain text |
| TC_LOGIN_010 | Session expires after 30 minutes of inactivity | Functional | Medium | Verify user session expires and requires re-login | User logged in successfully | 1. User logs in successfully<br/>2. No interaction for 30 minutes<br/>3. User attempts action (click button, navigate) | 1. User redirected to login page<br/>2. Message displays: "Session expired. Please login again"<br/>3. Previous page state not restored |
| TC_LOGIN_011 | Password requirements validation | Functional | High | Verify system enforces strong password requirements during registration | Password field is accessible during registration | 1. Attempt set password: "weak"<br/>2. Check error message | 1. Error displays: "Password must be 8+ characters with uppercase, lowercase, numbers, and special characters"<br/>2. Password rejected |
| TC_LOGIN_012 | SQL injection attempt prevention | Security | High | Verify system prevents SQL injection through login fields | Login page is accessible | 1. Navigate to login page<br/>2. Enter in email field: admin' OR '1'='1<br/>3. Enter any password<br/>4. Click Login button | 1. Login attempt rejected<br/>2. Error message: "Invalid email or password"<br/>3. No database error messages exposed<br/>4. Attempt logged for security monitoring |
| TC_LOGIN_013 | Login performance under normal load | Performance | Medium | Verify login completes within acceptable time | User account exists, login page loaded | 1. Measure login start time<br/>2. Enter valid credentials<br/>3. Click Login button<br/>4. Measure time to dashboard display | 1. Authentication completes within 2 seconds<br/>2. Dashboard fully loads within 5 seconds<br/>3. No timeout errors |
| TC_LOGIN_014 | Empty email field submission | Edge Case | Low | Verify system handles empty email field | Login page is accessible | 1. Leave email field empty<br/>2. Enter password: SecurePass123!<br/>3. Click Login button | 1. Validation error: "Email is required"<br/>2. Login not attempted<br/>3. User remains on login page |
| TC_LOGIN_015 | Empty password field submission | Edge Case | Low | Verify system handles empty password field | Login page is accessible | 1. Enter email: user@example.com<br/>2. Leave password field empty<br/>3. Click Login button | 1. Validation error: "Password is required"<br/>2. Login not attempted<br/>3. Email value retained |
| TC_LOGIN_016 | Logout clears session | Functional | Medium | Verify logout properly terminates session | User logged in successfully | 1. User logged in and on dashboard<br/>2. Click Logout button<br/>3. Attempt to access protected resource directly via URL | 1. Session cookie/token deleted<br/>2. User redirected to login page<br/>3. Back button doesn't return to dashboard |

## Summary Statistics

**Total Test Cases:** 16
- **High Priority:** 8 (50%) - Core login, security features
- **Medium Priority:** 6 (37.5%) - Session management, convenience features
- **Low Priority:** 2 (12.5%) - UI refinements, edge cases

**By Test Type:**
- Functional: 9 (56%) - Core login functionality
- Security: 4 (25%) - Authentication and injection prevention
- UI: 2 (12.5%) - User interface aspects
- Performance: 1 (6%) - Response time
- Edge Cases: 0 (0%)

**Requirement Coverage:**
- ✅ Credential validation: 4 test cases (TC_LOGIN_002, 003, 004, 005)
- ✅ Successful login: 2 test cases (TC_LOGIN_001, 007)
- ✅ Error handling: 3 test cases (TC_LOGIN_003, 004, 014, 015)
- ✅ Account lockout: 1 test case (TC_LOGIN_005)
- ✅ Password requirements: 1 test case (TC_LOGIN_011)
- ✅ Session management: 1 test case (TC_LOGIN_010)
- ✅ Remember me: 2 test cases (TC_LOGIN_007, 008)
- ✅ Rate limiting: 1 test case (TC_LOGIN_006)
- ✅ Security: 1 test case (TC_LOGIN_012)
- ✅ Performance: 1 test case (TC_LOGIN_013)
```

## How to Use This Agent

### In VS Code with Copilot

1. **Save your requirement** to a file or copy it
2. **Ask the test case generator agent:**
   ```
   @test-case-generator Create comprehensive test cases for this requirement: [paste requirement here]
   ```

3. **The agent will return:**
   - Step-by-step reasoning showing analysis
   - Comprehensive Markdown table with all test cases
   - Summary statistics and coverage report

### Example Prompts

- "Generate test cases for the user login feature"
- "Create comprehensive test suite for payment processing API"
- "Design test cases for file upload functionality"
- "Generate security and functional test cases for user registration"
- "Create test cases for inventory management system"

## Tips for Best Results

1. **Provide Complete Requirements** - Include business rules, constraints, and acceptance criteria
2. **Specify Constraints** - Mention performance requirements, security needs, browser support
3. **Include Examples** - Provide sample data or scenarios you want tested
4. **Ask for Specific Test Types** - Request "focus on security testing" if you have specific concerns
5. **Request Coverage Report** - Ask for "test coverage analysis" to verify all requirements are tested

