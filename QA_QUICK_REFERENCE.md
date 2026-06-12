# QA Quick Reference Guide

**For QA Teams Using the Test Case Generator Agent**

---

## 🚀 Getting Started in 2 Minutes

### Step 1: Have Your Requirement Ready
Copy your requirement to the clipboard or have it in a file

### Step 2: Ask the Agent
```
@test-case-generator Create test cases for: [requirement]
```

### Step 3: Get Your Test Cases
The agent generates a Markdown table you can import into any test tool

---

## ✅ Checklist: Before You Ask

- [ ] Requirement is clearly written
- [ ] Acceptance criteria are defined
- [ ] User stories or use cases are documented
- [ ] Any constraints (performance, security, browser support) are mentioned
- [ ] You know what test types are needed (Functional, Security, etc.)

---

## 📋 Quick Test Type Reference

| Type | When to Use | Example |
|------|------------|---------|
| **Functional** | Test if feature works as specified | "Login with valid credentials" |
| **UI** | Test user interface, layout, responsiveness | "Error message displays in red" |
| **API** | Test REST endpoints, responses, contracts | "POST /login returns 200 OK" |
| **Security** | Test authentication, data protection, vulnerabilities | "SQL injection is blocked" |
| **Performance** | Test response times, load handling, scalability | "Login completes in < 2 seconds" |
| **Edge Case** | Test boundary conditions, extreme values | "Max field length is 255 chars" |

---

## 🎯 Priority Quick Guide

| Level | When to Use | Examples |
|-------|------------|----------|
| **HIGH** | Must work or feature is broken | Core workflows, security, critical paths |
| **MEDIUM** | Should work, impacts users | Secondary features, convenience features |
| **LOW** | Nice to have | Edge cases, rare scenarios, minor issues |

---

## 📝 Requirement Template Cheat Sheet

Quick version to get started:

```markdown
## Feature: [Name]

### What it does
[1-2 sentence description]

### Requirements
- [Requirement 1]
- [Requirement 2]
- [Requirement 3]

### Who uses it
- User role: [Description]
- Admin role: [Description]

### Success looks like
- User sees [this]
- System does [this]
- Data shows [this]

### Special needs
- Security: [If needed]
- Performance: [If needed]
- Compliance: [If needed]
```

---

## 🔄 Workflow: From Requirement to Test Execution

```
1. DEFINE REQUIREMENT
   ↓
2. REQUEST TEST CASES
   (@test-case-generator Generate test cases for: ...)
   ↓
3. REVIEW TEST CASES
   ✓ Check coverage
   ✓ Verify clarity
   ✓ Adjust as needed
   ↓
4. EXPORT TO TEST TOOL
   (TestRail, Xray, Azure Test Plans, etc.)
   ↓
5. EXECUTE TESTS
   ✓ Run each test case
   ✓ Log results
   ✓ Report defects
   ↓
6. TRACK & REPORT
   ✓ Pass/Fail metrics
   ✓ Coverage report
   ✓ Defect analysis
```

---

## 💬 Common Prompts to Use

### Full Test Suite
```
@test-case-generator Create comprehensive test cases for this requirement:
[Paste full requirement here with all details]
```

### Specific Test Type
```
@test-case-generator Generate security and edge case test cases for:
[Requirement]
```

### API Testing
```
@test-case-generator Create API test cases for this endpoint:
[Endpoint details and requirements]
```

### Mobile Testing
```
@test-case-generator Generate test cases for mobile app testing, focus on:
[Mobile-specific requirements]
```

### Performance Testing
```
@test-case-generator Create performance test cases for:
[Feature with performance requirements]
```

---

## 📊 What You'll Get

### Example Output Structure

```
# Test Case Generation Report

## Step-by-Step Reasoning
[Agent explains its analysis]

## Test Cases
[Markdown table with all test cases]

## Summary Statistics
- Total: X test cases
- High: X | Medium: X | Low: X
- Functional: X | Security: X | Performance: X | etc.
```

### Using the Test Cases Table

Each row in the table includes:
- **TC ID**: Unique test case identifier
- **Name**: What is being tested
- **Type**: Category (Functional, Security, etc.)
- **Priority**: High, Medium, or Low
- **Description**: Why this test matters
- **Preconditions**: Setup needed
- **Steps**: How to execute (numbered)
- **Expected Result**: What should happen

---

## 🔍 Typical Numbers by Feature Complexity

| Complexity | Example | Test Cases |
|-----------|---------|-----------|
| **Simple** | "Toggle button on/off" | 3-5 |
| **Low** | "Single page form" | 5-10 |
| **Medium** | "Login feature" | 10-20 |
| **High** | "Payment processing" | 20-50 |
| **Very High** | "Complex workflow" | 50+ |

---

## 🎓 Test Case Quality Checklist

For each test case, verify:

- [ ] Name clearly describes what's being tested
- [ ] Description explains why it's important
- [ ] Preconditions are complete and clear
- [ ] Each step is a single action
- [ ] Expected results are specific and measurable
- [ ] Test is independent (doesn't depend on other tests)
- [ ] Test is executable by any team member
- [ ] Test type is correctly classified
- [ ] Priority is appropriate
- [ ] No ambiguity or vague language

---

## 🛠️ Troubleshooting

### Problem: Too few test cases
**Solution**: Provide more detailed requirements, mention edge cases, security needs, or performance requirements

### Problem: Test cases are too vague
**Solution**: Increase detail in your requirement, include specific values and scenarios

### Problem: Missing security test cases
**Solution**: Explicitly mention security concerns or ask for "security test cases" focus

### Problem: Performance test cases needed
**Solution**: Include performance requirements like "must respond in 2 seconds"

### Problem: Need more edge cases
**Solution**: Mention "include comprehensive edge case coverage" in your request

---

## 📤 Exporting to Test Tools

### To TestRail
```
Copy the Markdown table → Paste into Excel → Import to TestRail
```

### To Jira/Xray
```
Create Jira issues from test cases → Link to Xray
```

### To Azure Test Plans
```
Copy test cases → Create test cases in Azure DevOps
```

### To Google Sheets
```
Copy table → Paste special (unformatted) into Sheets
```

### To CSV
```
Save Markdown table → Convert using online Markdown to CSV tool
```

---

## 🎯 Best Practices

1. **Be Specific**
   - ❌ "Test login"
   - ✅ "Test login with valid email and password, verify redirect to dashboard"

2. **Include Examples**
   - ❌ "Test with user data"
   - ✅ "Test with email: user@example.com, password: SecurePass123!"

3. **State Requirements Clearly**
   - ❌ "Should work on browsers"
   - ✅ "Must work on Chrome v90+, Firefox v88+, Safari v14+"

4. **Mention Constraints**
   - ❌ "Test the feature"
   - ✅ "Test feature considering PCI compliance, max 5 failed attempts before lockout"

5. **Provide Context**
   - Include business rules
   - Mention related features
   - Reference acceptance criteria

---

## 💡 Pro Tips

- **Batch Similar Requests**: Ask for multiple related features together
- **Iterate**: Start with basic test cases, then ask for "add security test cases"
- **Reference Examples**: "Similar to the login test cases example"
- **Ask for Focus**: "Prioritize performance and security aspects"
- **Request Coverage**: "Ensure all edge cases are covered"
- **Include Data**: Provide sample test data to make tests more specific

---

## 📞 Need Help?

1. **See Full Documentation**: [README_AGENT.md](../README_AGENT.md)
2. **Check Example**: [EXAMPLE_LOGIN_SYSTEM.md](../examples/EXAMPLE_LOGIN_SYSTEM.md)
3. **Use Template**: [REQUIREMENT_TEMPLATE.md](../templates/REQUIREMENT_TEMPLATE.md)

---

**Remember**: The better your requirement, the better your test cases! 🎯
