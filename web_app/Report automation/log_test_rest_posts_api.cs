========== REQUEST (PROMPT) ==========

You are an SDET. Write a complete, valid Python `pytest` script that implements the following test cases.
Use a Service Object or Page Object Model structure (you can mock the POM classes or Service classes at the top of the file).

ORIGINAL API/FEATURE REQUIREMENT (USE THIS AS CONTEXT FOR URLS/ENDPOINTS/PAYLOADS):
curl 'https://gorest.co.in/public/v2/posts' \
  -H 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
  -H 'accept-language: en-US,en;q=0.9' \
  -H 'cache-control: no-cache' \
  -b 'ahoy_visitor=70a4c15a-f123-4471-921d-8c602b794d32; ahoy_visit=7905181c-c8dc-48f5-9eca-ac15c2dabeec; cf_clearance=KWc65ng2MNBTi7E1qsll6WzLl8t7ONyoCxXR8FHBUq0-1781259946-1.2.1.1-Zd30PoA7kHU3b7KSMETpZs2rF8N5p9x0tfBSvOTyKOyKIC41sVtBfSr8O42CLafIIAYMACvvd2zDWlUf8WX8uTPvhb08WlxYc7EMOj38uG3WytkVx2WbeLV5xeAxc.KSR1YUECCQvmKKyPAt1Y55HwxsjavnAZTpdRwVhc5x1hjhdlit99VFztu1n3xUdlG.J6Eh7MXwN2fHQWd3mgZR8vLBhQth7mMD5GLTXecAsj40ZanncFne3rv8dqpQ1IbQq1oshkLFAXFbTCzdV58Ghm6eQaERbjTtMgHgGUBqcBlJ4ZbcaXWjz2J3xnKmdagyslJjyokohN0U52O0jJe38w; _gorest_session=ucJn4naQ2IwB63fCIOuVk3giWQh8ZGSuYdUkVIksVXv%2FKB%2FrWfXAzuTC1VxCSmeR3n%2Fvyt%2Fy2ktUzIvGDGLchyoK0EkJHZH1cfOj0ot6S7lhJaGay7mWLbJ8vktnEsqmOpIwvikC8DG3sRwUwkgyeVDuT7DN28IZBl3iiP2J7DfWeVmhCd9kFZZetTawDy2sDZeKj5crgEy8iDUDXbL2mYJmDS5IpsW6edDOLoat6Brr%2F3nSqEygxXM5Jca%2BFMxo%2BZYSqTcN9Y5FS%2BiJYIQlIsRfH5ycqKI%3D--9lT6TtOd%2F%2BMfXE01--OEIkJmM78xn200Jj%2BiBn2w%3D%3D' \
  -H 'pragma: no-cache' \
  -H 'priority: u=0, i' \
  -H 'sec-ch-ua: "Chromium";v="148", "Google Chrome";v="148", "Not/A)Brand";v="99"' \
  -H 'sec-ch-ua-mobile: ?1' \
  -H 'sec-ch-ua-platform: "Android"' \
  -H 'sec-fetch-dest: document' \
  -H 'sec-fetch-mode: navigate' \
  -H 'sec-fetch-site: none' \
  -H 'sec-fetch-user: ?1' \
  -H 'upgrade-insecure-requests: 1' \
  -H 'user-agent: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Mobile Safari/537.36'

TEST CASES TO AUTOMATE:
[{"ID":"TC001","Test Case Name":"Valid Session - Retrieve Posts","Description":"Verify posts can be fetched with valid session","Preconditions":"Valid session cookie exists","Steps":"1. Send GET request to \/posts endpoint<br>2. Include valid session cookie<br>3. Verify response","Expected Result":"HTTP 200, JSON array with post objects","Type":"Functional","Priority":"High"},{"ID":"TC002","Test Case Name":"Invalid\/Expired Session","Description":"Verify proper error for invalid session","Preconditions":"Session cookie is missing or expired","Steps":"1. Send GET request without valid cookie<br>2. Check response status","Expected Result":"HTTP 401\/403 with error message","Type":"Security","Priority":"High"}]

Return your response in this exact format:
FILENAME: <suggested_filename>.py
```python
<python_code_here>
```

Naming rules for the filename:
- If this is a gRPC API test, use format: test_grpc_<api_name>.py
- If this is a REST API test, use format: test_rest_<api_name>.py
- Otherwise, use format: test_<feature_name>.py


========== RESPONSE ==========


FILENAME: test_rest_posts_api.py
```python
import os
import pytest
import requests
from typing import Dict, Any, Optional

# ==============================================================================
# Service Object Layer (Mocked/Implemented Structure)
# ==============================================================================

class PostsService:
    """
    Service Object for interacting with the Posts API.
    Encapsulates HTTP requests and response handling.
    """
    BASE_URL = "https://gorest.co.in/public/v2"
    ENDPOINT_POSTS = "/posts"

    def __init__(self, session: Optional[requests.Session] = None):
        self.session = session or requests.Session()
        self.session.headers.update({
            "Accept": "application/json",
            "Content-Type": "application/json",
            "User-Agent": "SDET-Automation-Script/1.0"
        })

    def get_posts(self, session_cookie: Optional[str] = None) -> requests.Response:
        """
        Retrieves the list of posts.
        
        Args:
            session_cookie: The session cookie string (e.g., "_gorest_session=...").
            
        Returns:
            requests.Response object.
        """
        url = f"{self.BASE_URL}{self.ENDPOINT_POSTS}"
        headers = self.session.headers.copy()
        
        if session_cookie:
            headers["Cookie"] = session_cookie
            
        return self.session.get(url, headers=headers)

# ==============================================================================
# Pytest Fixtures
# ==============================================================================

@pytest.fixture
def posts_service():
    """Fixture to provide a PostsService instance."""
    return PostsService()

@pytest.fixture
def valid_session_cookie():
    """
    Fixture to provide a valid session cookie.
    In a real CI/CD environment, this should be fetched from a secure vault 
    or obtained via a login flow fixture.
    """
    # Placeholder for valid session cookie. 
    # In production tests, use os.getenv("VALID_SESSION_COOKIE")
    return "_gorest_session=valid_session_token_placeholder"

@pytest.fixture
def invalid_session_cookie():
    """
    Fixture to provide an invalid or expired session cookie.
    """
    return "_gorest_session=expired_or_invalid_token"

# ==============================================================================
# Test Cases
# ==============================================================================

class TestPostsAPI:
    """Test suite for the Posts API endpoints."""

    def test_tc001_valid_session_retrieve_posts(self, posts_service, valid_session_cookie):
        """
        TC001: Valid Session - Retrieve Posts
        Description: Verify posts can be fetched with valid session
        Expected Result: HTTP 200, JSON array with post objects
        """
        # Step 1 & 2: Send GET request with valid session cookie
        response = posts_service.get_posts(session_cookie=valid_session_cookie)
        
        # Step 3: Verify response
        assert response.status_code == 200, f"Expected 200, got {response.status_code}"
        
        # Verify JSON structure
        try:
            data = response.json()
            assert isinstance(data, list), "Response body should be a JSON array"
            # Optional: Check if list is not empty if API guarantees data
            # assert len(data) > 0, "Response should contain posts"
        except ValueError:
            pytest.fail("Response content is not valid JSON")

    def test_tc002_invalid_session_retrieve_posts(self, posts_service, invalid_session_cookie):
        """
        TC002: Invalid/Expired Session
        Description: Verify proper error for invalid session
        Expected Result: HTTP 401/403 with error message
        """
        # Step 1: Send GET request without valid cookie (using invalid one)
        response = posts_service.get_posts(session_cookie=invalid_session_cookie)
        
        # Step 2: Check response status
        # Accepting 401 (Unauthorized) or 403 (Forbidden) as per requirement
        assert response.status_code in [401, 403], \
            f"Expected 401 or 403 for invalid session, got {response.status_code}"
        
        # Verify error message exists in response (if JSON)
        try:
            data = response.json()
            assert "error" in data or "message" in data, "Error response should contain error details"
        except ValueError:
            # Some APIs return HTML or plain text for auth errors
            assert response.status_code in [401, 403]
```