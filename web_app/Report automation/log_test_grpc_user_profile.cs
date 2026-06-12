========== REQUEST (PROMPT) ==========

You are an SDET. Write a complete, valid Python `pytest` script that implements the following test cases.
Use a Service Object or Page Object Model structure (you can mock the POM classes or Service classes at the top of the file).

ORIGINAL API/FEATURE REQUIREMENT (USE THIS AS CONTEXT FOR URLS/ENDPOINTS/PAYLOADS):
grpcurl \
	-insecure \
	-H 'client-id':'dev' \
	-H 'client-key':'3iSBn25SGJrqwjS' \
	-H 'b3':'80f198ee56343ba864fe8b2a57d3e997-e457b5a2e4d86bd1-1' \
	-emit-defaults \
	-proto '/Users/lap15743/Documents/PE/GRPC/UM/user_profile.proto' \
	-import-path '/Users/lap15743/Documents/PE/GRPC/UM' \
	-import-path '/Users/lap15743/Documents/PE/GRPC/Promotion' \
	-d '{"basic_profile":false,"identity_profile":false,"kyc_profile":true,"lock_status":false,"pin_status":false,"user_ids":["220422805001008"]}' \
	'qcgrpc-user-profile.zalopay.vn:443' \
	user_profile.v1.UserProfile.QueryByListUserId

TEST CASES TO AUTOMATE:
[{"STT":"1","Test Case Name":"TC01 - Query Valid User Profile","Description":"Kiểm tra query profile thành công với user ID hợp lệ","Preconditions":"1. Server đang chạy<br>2. User ID tồn tại","Steps":"1. Gửi request với headers đầy đủ<br>2. Truyền user_id hợp lệ<br>3. Set kyc_profile=true","Expected Result":"1. Status code 200<br>2. Response chứa kyc_profile data<br>3. Thời gian < 500ms","Type":"Functional","Priority":"High"},{"STT":"2","Test Case Name":"TC02 - Invalid Authentication","Description":"Kiểm tra từ chối request khi auth không hợp lệ","Preconditions":"1. Server đang chạy","Steps":"1. Gửi request với client-id sai<br>2. Hoặc thiếu client-key","Expected Result":"1. Status code 401\/403<br>2. Error message rõ ràng<br>3. Không leak sensitive data","Type":"Security","Priority":"High"}]

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


FILENAME: test_grpc_user_profile.py
```python
import pytest
import time
from unittest.mock import Mock, MagicMock, patch
from dataclasses import dataclass
from typing import List, Optional

# ==============================================================================
# MOCKED SERVICE OBJECT / PAGE OBJECT MODEL STRUCTURE
# ==============================================================================
# Since actual .proto generated files are not available in this environment,
# we mock the gRPC stub and message structures to ensure the script is valid
# and runnable while demonstrating the Service Object pattern.

class MockRpcError(Exception):
    """Mock gRPC RPC Error"""
    def __init__(self, code, details):
        self.code = code
        self.details = details
        super().__init__(details)

class MockStatusCode:
    OK = "OK"
    UNAUTHENTICATED = "UNAUTHENTICATED"
    PERMISSION_DENIED = "PERMISSION_DENIED"
    INVALID_ARGUMENT = "INVALID_ARGUMENT"

@dataclass
class QueryByListUserIdRequest:
    basic_profile: bool
    identity_profile: bool
    kyc_profile: bool
    lock_status: bool
    pin_status: bool
    user_ids: List[str]

@dataclass
class QueryByListUserIdResponse:
    user_profiles: List[dict]
    status: str

class MockUserProfileStub:
    """Mock gRPC Stub for UserProfile Service"""
    def __init__(self, response_data=None, raise_error=None):
        self.response_data = response_data
        self.raise_error = raise_error

    def QueryByListUserId(self, request, metadata=None, timeout=None):
        if self.raise_error:
            raise MockRpcError(self.raise_error['code'], self.raise_error['details'])
        return self.response_data

class UserProfileService:
    """
    Service Object for interacting with the User Profile gRPC API.
    Encapsulates connection, headers, and request construction.
    """
    def __init__(self, stub: MockUserProfileStub, client_id: str, client_key: str, trace_id: str):
        self.stub = stub
        self.client_id = client_id
        self.client_key = client_key
        self.trace_id = trace_id
        self.endpoint = "qcgrpc-user-profile.zalopay.vn:443"

    def _build_metadata(self):
        """Constructs gRPC metadata (headers)"""
        return [
            ('client-id', self.client_id),
            ('client-key', self.client_key),
            ('b3', self.trace_id)
        ]

    def query_by_list_user_id(self, user_ids: List[str], kyc_profile: bool = True) -> QueryByListUserIdResponse:
        """
        Queries user profiles by list of user IDs.
        """
        request = QueryByListUserIdRequest(
            basic_profile=False,
            identity_profile=False,
            kyc_profile=kyc_profile,
            lock_status=False,
            pin_status=False,
            user_ids=user_ids
        )
        
        metadata = self._build_metadata()
        
        # Simulate network call
        return self.stub.QueryByListUserId(request, metadata=metadata)

# ==============================================================================
# TEST CASES
# ==============================================================================

class TestUserProfileAPI:
    """Pytest Test Suite for User Profile gRPC API"""

    @pytest.fixture
    def valid_user_id(self):
        return ["220422805001008"]

    @pytest.fixture
    def auth_headers(self):
        return {
            "client_id": "dev",
            "client_key": "3iSBn25SGJrqwjS",
            "trace_id": "80f198ee56343ba864fe8b2a57d3e997-e457b5a2e4d86bd1-1"
        }

    @pytest.fixture
    def mock_stub_success(self):
        """Fixture to mock successful gRPC response"""
        response = QueryByListUserIdResponse(
            user_profiles=[{"user_id": "220422805001008", "kyc_status": "VERIFIED"}],
            status=MockStatusCode.OK
        )
        return MockUserProfileStub(response_data=response)

    @pytest.fixture
    def mock_stub_auth_error(self):
        """Fixture to mock authentication failure"""
        return MockUserProfileStub(
            raise_error={
                "code": MockStatusCode.UNAUTHENTICATED,
                "details": "Invalid client credentials"
            }
        )

    def test_tc01_query_valid_user_profile(self, valid_user_id, auth_headers, mock_stub_success):
        """
        TC01 - Query Valid User Profile
        Description: Kiểm tra query profile thành công với user ID hợp lệ
        Expected: Status OK, Response contains kyc_profile data, Time < 500ms
        """
        # Arrange
        service = UserProfileService(
            stub=mock_stub_success,
            client_id=auth_headers["client_id"],
            client_key=auth_headers["client_key"],
            trace_id=auth_headers["trace_id"]
        )

        # Act
        start_time = time.time()
        response = service.query_by_list_user_id(user_ids=valid_user_id, kyc_profile=True)
        end_time = time.time()
        elapsed_ms = (end_time - start_time) * 1000

        # Assert
        # 1. Status code 200 (Mapped to gRPC OK)
        assert response.status == MockStatusCode.OK, "Expected gRPC Status OK"
        
        # 2. Response contains kyc_profile data
        assert len(response.user_profiles) > 0, "Response should contain user profiles"
        assert "kyc_status" in response.user_profiles[0], "Response should contain kyc_profile data"
        
        # 3. Time < 500ms
        assert elapsed_ms < 500, f"Request took {elapsed_ms}ms, expected < 500ms"

    def test_tc02_invalid_authentication(self, valid_user_id, auth_headers, mock_stub_auth_error):
        """
        TC02 - Invalid Authentication
        Description: Kiểm tra từ chối request khi auth không hợp lệ
        Expected: Status 401/403 (Mapped to UNAUTHENTICATED), Error message clear, No sensitive data leak
        """
        # Arrange
        # Simulate invalid auth by passing wrong client_id
        invalid_auth_headers = auth_headers.copy()
        invalid_auth_headers["client_id"] = "invalid_client_id"
        
        service = UserProfileService(
            stub=mock_stub_auth_error,
            client_id=invalid_auth_headers["client_id"],
            client_key=invalid_auth_headers["client_key"],
            trace_id=invalid_auth_headers["trace_id"]
        )

        # Act & Assert
        with pytest.raises(MockRpcError) as exc_info:
            service.query_by_list_user_id(user_ids=valid_user_id, kyc_profile=True)

        error = exc_info.value

        # 1. Status code 401/403 (Mapped to gRPC UNAUTHENTICATED)
        assert error.code == MockStatusCode.UNAUTHENTICATED, "Expected UNAUTHENTICATED status"

        # 2. Error message clear
        assert "Invalid" in error.details or "credentials" in error.details, "Error message should be clear"

        # 3. No sensitive data leak (Ensure details don't contain keys or internal IDs)
        assert "client-key" not in error.details.lower(), "Sensitive data leaked in error message"
        assert "3iSBn25SGJrqwjS" not in error.details, "Sensitive data leaked in error message"
```