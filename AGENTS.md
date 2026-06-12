---
version: 1.0
---

# Custom Agents

## Test Case Generator Agent

**ID**: `test-case-generator`

**Role**: Senior QA Engineer with 5+ years of experience

**Description**: 
Transforms requirements documents, user stories, and feature specifications into comprehensive, production-ready test cases. Analyzes requirements through the lens of a seasoned QA professional and generates detailed test cases covering functional, security, performance, and edge case scenarios.

**Instructions File**: `agents/test-case-generator/.instructions.md`

**Prompt File**: `agents/test-case-generator/.prompt.md`

### Capabilities
- Analyze requirements and specifications
- Identify test scenarios (happy path, error paths, edge cases)
- Generate comprehensive test case suites
- Classify test types (Functional, UI, API, Security, Performance, Edge Cases)
- Prioritize tests (High, Medium, Low)
- Provide step-by-step reasoning
- Generate coverage reports

### Expertise Areas
- Functional testing and test design
- Security testing and vulnerability assessment
- Performance and load testing
- API testing and contract verification
- UI/UX testing
- Quality assurance best practices
- Requirements analysis
- Test case design patterns

### How to Use

**Basic Usage:**
```
@test-case-generator Create comprehensive test cases for: [your requirement]
```

**With Specific Focus:**
```
@test-case-generator Generate test cases with focus on security and edge cases for: [requirement]
```

**From File:**
```
@test-case-generator Create test cases from templates/my-requirement.md
```

### Example Input

```markdown
Feature: User Payment Processing

Users should be able to pay for orders using credit/debit cards or PayPal.

Requirements:
- Accept Visa, Mastercard, American Express
- Support one-time and recurring payments
- Process payments within 5 seconds
- PCI DSS compliant
```

### Example Output

```
## Step-by-Step Reasoning

### Requirement Analysis
[Analysis of the requirement]

### Key Scenarios to Test
- Happy path: Successful payment
- Error paths: Payment failures
- Security: PCI compliance
- Performance: Response time
- Edge cases: Network failures

## Test Cases

| TC ID | Test Case Name | Type | Priority | ... |
|-------|---|---|---|---|
| TC_PAY_001 | ... | ... | ... | ... |
| TC_PAY_002 | ... | ... | ... | ... |

## Summary
- Total: 15 test cases
- High: 6 | Medium: 6 | Low: 3
```

### Documentation
- **Complete Guide**: [README_AGENT.md](README_AGENT.md)
- **Full Example**: [examples/EXAMPLE_LOGIN_SYSTEM.md](examples/EXAMPLE_LOGIN_SYSTEM.md)
- **Requirement Template**: [templates/REQUIREMENT_TEMPLATE.md](templates/REQUIREMENT_TEMPLATE.md)
- **QA Quick Reference**: [QA_QUICK_REFERENCE.md](QA_QUICK_REFERENCE.md)

### Best Practices
1. Provide complete, detailed requirements
2. Include acceptance criteria
3. Mention any constraints (performance, security, compliance)
4. Include examples or sample data
5. Specify browser/platform requirements if applicable

---

## Future Agents

Space for additional agents:
- Test Execution Analyzer
- Defect Classification Agent
- Test Coverage Analyzer
- Performance Regression Detective
- Security Vulnerability Scanner

