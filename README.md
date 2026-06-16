# test-case-generator

A GreenNode AgentBase agent.

## Use Case: AI QA Engineer - Automated Testing Pipeline

**1. The Problem:**
In the software development lifecycle, analyzing Product Requirements Documents (PRDs) to design Test Cases and write Automation Scripts is highly time-consuming, labor-intensive, and prone to human error when done manually.

**2. Target Users:**
- QA Engineers (Manual & Automation Testers)
- QA Managers / Test Leads
- Software Developers

**3. How it works:**
Acting as a Senior QA Engineer, the Agent receives requirement descriptions or document files (PDF/Word) from users via a Web interface. Leveraging the power of LLMs, the Agent automatically analyzes the business logic, **evaluates the complexity (Complexity Score), and allocates risk distribution (Risk Areas)** to calculate the optimal number of test scenarios *before* generating a complete Test Case table. 
Subsequently, when the user selects the Test Cases they want to automate, the Agent directly writes the Automation Test source code (using Pytest) following the Service/Page Object Model standards, automatically executes them, and exports an HTML report.

**4. Business Value:**
- **Save 80% of Time**: Frees QA engineers from repetitive Test Case design and writing boilerplate code.
- **Increase Test Coverage**: Ensures that Edge Cases, Security, and Performance scenarios are not overlooked.
- **Rapid Deployment**: Generates a standardized Regression test suite that is easy to reuse and integrate into CI/CD pipelines.

---

## Prerequisites

- Python 3.10+
- A GreenNode IAM Service Account ([create one here](https://iam.console.vngcloud.vn/service-accounts))

## Setup

1. Create and activate a virtual environment:
   ```bash
   # macOS/Linux:
   python3 -m venv venv && source venv/bin/activate

   # Windows (PowerShell):
   python -m venv venv; venv\Scripts\Activate.ps1
   ```

2. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```

3. Configure credentials for **local development** (choose one method):

   **Option A** - Environment variables:
   ```bash
   cp .env.example .env
   # Edit .env with your credentials
   ```

   **Option B** - Config file (already created):
   Edit `.greennode.json` with your `client_id` and `client_secret` from your IAM Service Account.

   > **Note**: When deployed on AgentBase Runtime, the IAM service account and Agent Identity are managed by the runtime system and automatically available to the SDK — no manual credential configuration needed in the container.

4. (Optional, for local dev) Create an Agent Identity at https://aiplatform.console.vngcloud.vn/access-control and set `agent_identity` in `.greennode.json` or `GREENNODE_AGENT_IDENTITY` env var. On AgentBase Runtime, this is managed automatically by the runtime system.

## Configure LLM (LangChain/LangGraph only)

This project uses any OpenAI-compatible LLM provider. Set the following in `.env`:

```
LLM_API_KEY=your-api-key
LLM_BASE_URL=your-provider-base-url
LLM_MODEL=your-model-name
```

**Provider examples:**
- **GreenNode AIP**: Use `/agentbase-llm` to get an API key. Set `LLM_BASE_URL=https://maas-llm-aiplatform-hcm.api.vngcloud.vn/v1`
- **OpenAI**: Set `LLM_BASE_URL=https://api.openai.com/v1`, model e.g. `gpt-4o`
- **Ollama** (local): Set `LLM_BASE_URL=http://localhost:11434/v1` (no key needed)

**Production**: Use `/agentbase-identity` to store your API key on the platform and inject it at runtime.

## Run Locally

```bash
python3 main.py
```

The agent starts on `http://127.0.0.1:8080`.

Test it:
```bash
curl -X POST http://127.0.0.1:8080/invocations \
  -H "Content-Type: application/json" \
  -d '{"message": "Hello, agent!"}'
```

**Testing tips** — the SDK extracts metadata from request headers (defined in `greennode_agentbase.runtime.models`):
- If the agent uses **memory** (short-term or long-term), **both headers are required** — the agent will return an error without them:
  `-H "X-GreenNode-AgentBase-User-Id: test-user"` `-H "X-GreenNode-AgentBase-Session-Id: test-session-1"`
- If the agent uses **user identity features** (delegated API key, OAuth2 3LO token), pass a user header so credentials resolve correctly:
  `-H "X-GreenNode-AgentBase-User-Id: user-abc"`
- To pass **custom headers** to the agent, use the `X-GreenNode-AgentBase-Custom-` prefix. The SDK collects all headers with this prefix (plus `Authorization`) into `context.request_headers`:
  `-H "X-GreenNode-AgentBase-Custom-My-Key: some-value"`
  Then access in handler: `context.request_headers.get("X-GreenNode-AgentBase-Custom-My-Key")`

Health check:
```bash
curl http://127.0.0.1:8080/health
```

## Deploy to AgentBase Runtime

1. Build and push your Docker image (or use `/agentbase-deploy` skill)
2. Create a Runtime at https://aiplatform.console.vngcloud.vn/agent-runtime?tab=runtime
3. Create an Endpoint pointing to your Runtime

See the [AgentBase Console](https://aiplatform.console.vngcloud.vn) to manage runtimes, identities, and memory.

## Add Conversation Memory (Optional)

When you need conversation history or long-term memory, use `/agentbase-memory` to set up AgentBase Memory and integrate it with your agent.

## Key Features

- **Test Case Generation**: Automatically analyzes requirements and PRDs to evaluate complexity, distribute test coverage by risk areas, and generate structured, comprehensive test cases.
- **Streamlit Web Interface**: User-friendly frontend (`web_app/app.py`) allowing users to input descriptions or upload PRD documents (PDF/Word).
- **Dynamic Test Automation Generation**: Converts selected Test Cases directly into executable Python `pytest` scripts following Page Object Model (POM) or Service Object architectures.
- **Automated Test Execution & Reporting**: Runs the generated pytest scripts directly from the browser, logs LLM interactions, and exports detailed HTML regression reports.

## Run Automation Python Scripts Manually

You can execute the generated Python test scripts (`.py`) manually from the terminal using **pytest**. Ensure your terminal is at the root of the project.

### 1. Basic Test Execution (Console Output)
```bash
pytest web_app/automation/tests/<file_name>.py
```
- Add `-v` for verbose output detailing each test case.
- Add `-s` to display `print()` statements.

### 2. Execute and Generate HTML Report
```bash
pytest web_app/automation/tests/<file_name>.py --html="web_app/Report automation/report_<custom_name>.html" --self-contained-html
```
*(Note: Ensure `pytest-html` is installed via `pip install pytest-html`)*

### 3. Run All Test Scripts
```bash
pytest web_app/automation/tests/
```

> **Tip:** If you encounter a `pytest: command not found` error, you can run it as a Python module instead:
> ```bash
> python -m pytest web_app/automation/tests/<file_name>.py
> ```

## Project Structure

- `main.py` - Agent entrypoint with handler and health check (AgentBase Runtime)
- `agents/test-case-generator/` - Contains the system prompt (`.prompt.md`) defining the QA persona
- `web_app/` - The Web Interface and Automation Execution Engine
  - `app.py` - Streamlit application frontend
  - `requirements.txt` - Dependencies for the web app (`streamlit`, `pytest`, `pandas`, `openai`, etc.)
  - `automation/tests/` - Dynamically generated `pytest` scripts (e.g. `test_rest_<api_name>.py`)
  - `Report automation/` - Execution logs (`.cs`) and generated HTML test reports
- `Dockerfile` - Container image definition
- `requirements.txt` - Python dependencies for the backend Agent
- `.greennode.json` - AgentBase configuration
- `.env` - Environment variable file (API Keys, URLs)
