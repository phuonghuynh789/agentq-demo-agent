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