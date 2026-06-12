# Test Case Generator Agent

**A comprehensive AI-powered test case generation system that acts as a Senior QA Engineer with 5+ years of experience.**

---

## 🎯 Overview

The Test Case Generator Agent transforms requirements documents, user stories, and feature specifications into comprehensive, production-ready test cases. It analyzes requirements through the lens of a seasoned QA professional and generates detailed test cases covering functional, security, performance, and edge case scenarios.

### Key Features

✅ **Comprehensive Test Coverage** - Generates test cases for all scenarios (happy path, error paths, edge cases)  
✅ **Multiple Test Types** - Supports Functional, UI, API, Security, Performance, and Edge Case testing  
✅ **Priority Classification** - Intelligently prioritizes tests by importance and risk  
✅ **Detailed Analysis** - Provides step-by-step reasoning before finalizing test cases  
✅ **Production-Ready Output** - Markdown tables that QA teams can immediately use  
✅ **Requirement Traceability** - Ensures all requirements are covered by test cases  

---

## 📋 Quick Start

### Option 1: Using VS Code with Copilot Extension

1. Open your project in VS Code
2. Have your requirement document ready (or copy the text)
3. Use the `@test-case-generator` mention:

```
@test-case-generator Create test cases for: [Your requirement here]
```

4. The agent will analyze your requirement and generate:
   - Step-by-step reasoning
   - Comprehensive test case table
   - Coverage statistics

### Option 2: Paste Your Requirement

Create a new file `requirement.md` in your project:

```markdown
## Feature: User Login

Users should be able to log in using email and password...
```

Then ask: "@test-case-generator Generate test cases from requirement.md"

---

## 📁 Project Structure

```
agentdemo/
├── agents/
│   └── test-case-generator/
│       ├── .instructions.md       # Detailed agent instructions
│       └── .prompt.md             # System prompt configuration
├── examples/
│   └── EXAMPLE_LOGIN_SYSTEM.md    # Complete working example
├── templates/
│   └── REQUIREMENT_TEMPLATE.md    # Template for your requirements
└── README.md                       # This file
```

---

## 🤖 Agent Capabilities

### 1. Requirement Analysis
- Identifies all functional and non-functional requirements
- Extracts key features and workflows
- Determines dependencies and integration points
- Identifies risk areas and security concerns

### 2. Scenario Identification
- **Happy Path**: Normal expected usage
- **Error Paths**: Invalid inputs and error handling
- **Edge Cases**: Boundary conditions and extreme values
- **Security Scenarios**: Authentication, authorization, data protection
- **Performance Scenarios**: Load, stress testing
- **Integration Scenarios**: System interactions

### 3. Test Case Generation
Creates detailed test cases with:
- **Unique ID**: TC_[Feature]_[Number]
- **Clear Name**: Describes what's being tested
- **Complete Description**: Why the test is important
- **Preconditions**: What needs to be set up first
- **Step-by-Step Instructions**: Numbered, actionable steps
- **Expected Results**: Specific, measurable outcomes
- **Test Type**: Functional, UI, API, Security, Performance, Edge Case
- **Priority**: High, Medium, or Low

### 4. Reasoning & Documentation
- Documents analysis rationale
- Provides coverage verification
- Classifies by test type and priority
- Generates summary statistics

---

## 📊 Test Case Structure

### Markdown Table Format

| Field | Description | Example |
|-------|-------------|---------|
| TC ID | Unique identifier | TC_LOGIN_001 |
| Test Case Name | What is being tested | Successful login with valid credentials |
| Type | Category of test | Functional, Security, UI, etc. |
| Priority | Importance level | High, Medium, Low |
| Description | Why this test matters | Verify core login functionality |
| Preconditions | Setup required | User account must exist |
| Steps | Numbered actions | 1. Navigate to login<br/>2. Enter email |
| Expected Result | What should happen | User logged in, redirected to dashboard |

### Example Test Case Row

```markdown
| TC_LOGIN_001 | Successful login with valid credentials | Functional | High | Verify user can login with correct email and password | User account exists with valid credentials | 1. Navigate to login page<br/>2. Enter email: user@example.com<br/>3. Enter password: SecurePass123!<br/>4. Click Login button | 1. Login form displays<br/>2. Email entered successfully<br/>3. Password masked in field<br/>4. User redirected to dashboard |
```

---

## 🎓 Test Types Explained

### Functional Testing
- Verifies features work as specified
- Tests business logic and workflows
- **Example**: "User can successfully create a new order"

### UI Testing
- Verifies user interface elements display correctly
- Tests responsiveness and layout
- Tests user experience
- **Example**: "Login form displays error message in red text"

### API Testing
- Verifies API endpoints respond correctly
- Tests request/response contracts
- Tests error handling
- **Example**: "GET /api/users returns 200 with user array"

### Security Testing
- Tests authentication and authorization
- Verifies data protection
- Tests against common vulnerabilities (SQL injection, XSS, etc.)
- **Example**: "SQL injection attempts are blocked with 400 error"

### Performance Testing
- Tests response times under load
- Tests system scalability
- Tests resource usage
- **Example**: "Login completes within 2 seconds under 100 concurrent users"

### Edge Case Testing
- Tests boundary conditions
- Tests extreme or unusual values
- Tests combinations of conditions
- **Example**: "System handles 1,000,000 character strings correctly"

---

## 🔍 Priority Classification

### High Priority
✅ Critical business functionality  
✅ Core user workflows  
✅ Security-related features  
✅ Features used by all or most users  
✅ Show-stoppers (if they fail, feature is unusable)

**→ These must pass before release**

### Medium Priority
✅ Important but not critical features  
✅ Secondary workflows  
✅ Features with moderate usage frequency  
✅ Convenience features  
✅ Would cause significant issues if they fail

**→ Should pass before release**

### Low Priority
✅ Nice-to-have features  
✅ Rare edge cases  
✅ Features with minimal usage frequency  
✅ Optional functionality  
✅ Would cause minor inconvenience if they fail

**→ Good to have working, but not release-critical**

---

## 📝 How to Use the Agent

### Step 1: Prepare Your Requirement

Use the template at `templates/REQUIREMENT_TEMPLATE.md` to structure your requirement:

```markdown
## Feature: User Payment Processing

### Overview
Users should be able to pay for orders using credit/debit cards or PayPal.

### Requirements
- Accept Visa, Mastercard, American Express
- Support one-time and recurring payments
- Process payments within 5 seconds
- PCI DSS compliant

### Constraints
- Must support desktop and mobile browsers
- Must handle network failures gracefully
```

### Step 2: Request Test Case Generation

In VS Code, use:

```
@test-case-generator Generate comprehensive test cases for: [paste your requirement]
```

Or reference your file:

```
@test-case-generator Create test cases from templates/my-requirement.md
```

### Step 3: Review the Output

The agent returns:

1. **Reasoning Section**
   - Requirement analysis
   - Identified scenarios
   - Coverage plan

2. **Test Cases Table**
   - All test cases in Markdown format
   - Ready to copy into your test management system

3. **Summary Statistics**
   - Total test cases by type and priority
   - Coverage verification

### Step 4: Use in Your QA Workflow

- Import test cases into your testing tool (TestRail, Xray, Azure Test Plans, etc.)
- Execute tests during QA phase
- Track results and defects
- Update as requirements change

---

## 💡 Best Practices

### ✅ DO

- **Be Specific**: Use exact values, not vague terms
  - ✓ "Test with 1,000,000 records"
  - ✗ "Test with a lot of data"

- **Include All Context**: Provide complete requirement information
  - ✓ Acceptance criteria, business rules, constraints
  - ✗ Just a vague feature description

- **Use the Template**: Follow the structure in `REQUIREMENT_TEMPLATE.md`

- **Provide Examples**: Include sample data or scenarios
  - ✓ "Valid email formats: user@example.com"
  - ✗ "Some email"

- **Specify Constraints**: Mention performance, security, or compliance needs

### ❌ DON'T

- Don't provide incomplete requirements
- Don't skip acceptance criteria
- Don't assume the agent will guess what you mean
- Don't ask for test cases without requirements
- Don't expect perfect results from vague inputs

---

## 📚 Examples

### Complete Working Example

See `examples/EXAMPLE_LOGIN_SYSTEM.md` for a complete test case generation example:
- Shows full reasoning process
- Includes 16 detailed test cases
- Demonstrates all test types
- Shows priority classification

### Use Cases

1. **User Authentication** → See login example
2. **API Endpoints** → Create test cases for REST endpoints
3. **Payment Processing** → Security and functional testing
4. **File Upload** → Edge cases and error handling
5. **Reporting Features** → Performance and data accuracy
6. **Mobile Apps** → UI/UX and responsiveness

---

## 🎯 Tips for Best Results

### 1. Clear Requirements
```
✓ GOOD: "Users can filter products by category, price range (0-$1000), 
         and brand. Results update without page reload and show count."
✗ BAD: "Implement filtering"
```

### 2. Complete Context
```
✓ GOOD: Include acceptance criteria, user roles, integrations, 
        performance targets, security needs
✗ BAD: Just a brief description
```

### 3. Specific Examples
```
✓ GOOD: "Test with names up to 100 characters, containing special chars"
✗ BAD: "Test with various names"
```

### 4. Mention Constraints
```
✓ GOOD: "Must support IE11, mobile browsers, work offline"
✗ BAD: "Should work on all browsers"
```

### 5. Ask for Specific Focus
```
✓ GOOD: "Focus on security testing and edge cases"
✗ BAD: Generic request without specific needs
```

---

## 🔧 Configuration

### Customizing the Agent

Edit `.instructions.md` or `.prompt.md` to:
- Add company-specific test standards
- Include custom test types
- Add specific quality criteria
- Modify priority classification rules

### Integration with Test Management Tools

Once you have test cases, import into:
- **TestRail**: Via CSV or API
- **Xray/Jira**: Via Jira issue creation
- **Azure Test Plans**: Copy-paste or API integration
- **Google Sheets**: Maintain test matrix
- **Git**: Commit test cases as markdown

---

## 📞 Common Questions

### Q: How many test cases should I have?
**A:** It depends on complexity. Simple features might need 5-10 test cases. Complex systems with security needs might need 50+. The agent will create comprehensive coverage.

### Q: Do I need all High priority tests to pass?
**A:** Yes, High priority tests must pass before release. They represent critical functionality.

### Q: Can I combine test cases?
**A:** Some can be combined logically, but err on the side of separate cases for clarity and debugging.

### Q: How often should I regenerate test cases?
**A:** When requirements change significantly. Update incrementally rather than recreating everything.

### Q: What if a requirement is ambiguous?
**A:** Ask the agent to clarify specific aspects. It will ask follow-up questions.

---

## 🚀 Next Steps

1. **Review the example** at `examples/EXAMPLE_LOGIN_SYSTEM.md`
2. **Use the template** at `templates/REQUIREMENT_TEMPLATE.md` for your requirement
3. **Invoke the agent**: `@test-case-generator Create test cases for...`
4. **Review generated test cases** and customize as needed
5. **Import into your test management system**
6. **Execute and track results**

---

## 📖 Additional Resources

- [Agent Instructions](agents/test-case-generator/.instructions.md) - Detailed agent guidelines
- [Agent Prompt](agents/test-case-generator/.prompt.md) - System configuration
- [Complete Example](examples/EXAMPLE_LOGIN_SYSTEM.md) - Working test case generation
- [Requirement Template](templates/REQUIREMENT_TEMPLATE.md) - Template for your requirements

---

## 📝 License & Notes

This test case generator agent is designed to assist QA professionals in creating comprehensive test coverage. It leverages AI to analyze requirements and generate well-structured test cases that follow industry best practices.

**Version:** 1.0  
**Last Updated:** 2026-06-10  
**Created for:** VS Code with Copilot Extension
