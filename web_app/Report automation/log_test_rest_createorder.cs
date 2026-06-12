========== REQUEST (PROMPT) ==========

You are an SDET. Write a complete, valid Python `pytest` script that implements the following test cases.
Use a Service Object or Page Object Model structure (you can mock the POM classes or Service classes at the top of the file).

ORIGINAL API/FEATURE REQUIREMENT (USE THIS AS CONTEXT FOR URLS/ENDPOINTS/PAYLOADS):
curl 'https://zlpdev-mi-zlptesttools-be.zalopay.vn/zlptesttools/process/do/createorder?appid=553&key1=9phuAOYhan4urywHTh0ndEXiV3pKHr5Q&publickey=-----BEGIN+PUBLIC+KEY-----%0AMFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAMsHyik%2BFG0NjTxCu3yHTo8EczhRIlZA6Y1IE0yTGEOw+fqN4hD2prbrO0HxaWrVBUjHholVyhkmGpMm56vGHQ7UCAwEAAQ%3D%3D%0A-----END+PUBLIC+KEY-----&bankcode=&apptransid=260612_553_1781268499061&amount=10000&appuser=demo&embeddata=%7B%7D&item=\[\]&description=Test&moreparam=&subappid=&ver=v1&env=sandbox&apiendpoint=https:%2F%2Fsandbox.zalopay.com.vn%2Fv001%2Ftpe%2Fcreateorder' \
  -X 'POST' \
  -H 'sec-ch-ua-platform: "Android"' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZG1pbiI6dHJ1ZX0.bk92K9kepEY5xOTVxpf6_GlLEWMbilt7ip5y0L6lgWc' \
  -H 'Referer: https://zlpdev-mi-zlptesttools.zalopay.vn/' \
  -H 'sec-ch-ua: "Chromium";v="148", "Google Chrome";v="148", "Not/A)Brand";v="99"' \
  -H 'sec-ch-ua-mobile: ?1' \
  -H 'User-Agent: Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Mobile Safari/537.36' \
  -H 'Accept: application/json, text/plain, */*' \
  -H 'Content-Type: application/x-www-form-urlencoded'

TEST CASES TO AUTOMATE:
[{"STT":"1","Test Case Name":"TC_001: Tạo đơn hàng thành công","Description":"Kiểm tra tạo đơn với tham số hợp lệ","Preconditions":"API endpoint available, valid token","Steps":"1. Gửi POST request với đầy đủ tham số hợp lệ<br>2. Amount > 0<br>3. Apptransid duy nhất","Expected Result":"Response 200 OK, có order_id và order_code","Type":"Functional","Priority":"High"},{"STT":"2","Test Case Name":"TC_002: Token không hợp lệ","Description":"Kiểm tra xác thực JWT","Preconditions":"API endpoint available","Steps":"1. Gửi request với token hết hạn\/sai<br>2. Giữ nguyên các tham số khác","Expected Result":"Response 401 Unauthorized","Type":"Security","Priority":"High"}]

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


FILENAME: test_rest_createorder.py
```python
import pytest
import requests
import time
import uuid
from datetime import datetime
from typing import Dict, Any

# ==============================================================================
# CONFIGURATION & CONSTANTS
# ==============================================================================

# Base URL for the API
BASE_URL = "https://zlpdev-mi-zlptesttools-be.zalopay.vn/zlptesttools/process/do/createorder"

# Common Headers
COMMON_HEADERS = {
    "sec-ch-ua-platform": '"Android"',
    "Referer": "https://zlpdev-mi-zlptesttools.zalopay.vn/",
    "sec-ch-ua": '"Chromium";v="148", "Google Chrome";v="148", "Not/A)Brand";v="99"',
    "sec-ch-ua-mobile": "?1",
    "User-Agent": "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Mobile Safari/537.36",
    "Accept": "application/json, text/plain, */*",
    "Content-Type": "application/x-www-form-urlencoded"
}

# Valid Authorization Token (from context)
VALID_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZG1pbiI6dHJ1ZX0.bk92K9kepEY5xOTVxpf6_GlLEWMbilt7ip5y0L6lgWc"

# Invalid Authorization Token (for negative testing)
INVALID_TOKEN = "invalid_token_12345"

# Common Query Parameters (Decoded from context)
COMMON_PARAMS = {
    "appid": "553",
    "key1": "9phuAOYhan4urywHTh0ndEXiV3pKHr5Q",
    "publickey": """-----BEGIN PUBLIC KEY-----
MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAMsHyik+FG0NjTxCu3yHTo8EczhRIlZA6Y1IE0yTGEOw+fqN4hD2prbrO0HxaWrVBUjHholVyhkmGpMm56vGHQ7UCAwEAAQ==
-----END PUBLIC KEY-----""",
    "bankcode": "",
    "appuser": "demo",
    "embeddata": "{}",
    "item": "[]",
    "description": "Test",
    "moreparam": "",
    "subappid": "",
    "ver": "v1",
    "env": "sandbox",
    "apiendpoint": "https://sandbox.zalopay.com.vn/v001/tpe/createorder"
}

# ==============================================================================
# SERVICE OBJECT LAYER
# ==============================================================================

class CreateOrderService:
    """
    Service Object for interacting with the Create Order API.
    Encapsulates HTTP requests and parameter construction.
    """

    def __init__(self, base_url: str):
        self.base_url = base_url

    def _build_headers(self, token: str) -> Dict[str, str]:
        """Construct headers with the provided authorization token."""
        headers = COMMON_HEADERS.copy()
        headers["Authorization"] = f"Bearer {token}"
        return headers

    def _build_params(self, amount: int, apptransid: str) -> Dict[str, Any]:
        """Construct query parameters with dynamic values."""
        params = COMMON_PARAMS.copy()
        params["amount"] = str(amount)
        params["apptransid"] = apptransid
        return params

    def create_order(self, amount: int, apptransid: str, token: str) -> requests.Response:
        """
        Sends a POST request to create an order.
        
        Args:
            amount: Order amount.
            apptransid: Unique transaction ID.
            token: Bearer token for authentication.
            
        Returns:
            requests.Response object.
        """
        headers = self._build_headers(token)
        params = self._build_params(amount, apptransid)
        
        # Note: Based on the curl command, data is passed via Query Params 
        # despite Content-Type being x-www-form-urlencoded.
        response = requests.post(
            self.base_url,
            params=params,
            headers=headers,
            timeout=30
        )
        return response

# ==============================================================================
# TEST CASES
# ==============================================================================

class TestCreateOrderAPI:
    """
    Pytest test suite for Create Order API.
    """

    @pytest.fixture
    def service(self):
        """Fixture to initialize the Service Object."""
        return CreateOrderService(BASE_URL)

    @pytest.fixture
    def unique_apptransid(self):
        """Fixture to generate a unique apptransid for each test run."""
        timestamp = int(time.time() * 1000)
        unique_id = str(uuid.uuid4()).replace("-", "")[:8]
        return f"260612_553_{timestamp}_{unique_id}"

    def test_tc_001_create_order_success(self, service, unique_apptransid):
        """
        TC_001: Tạo đơn hàng thành công
        Description: Kiểm tra tạo đơn với tham số hợp lệ
        Preconditions: API endpoint available, valid token
        Steps: 
            1. Gửi POST request với đầy đủ tham số hợp lệ
            2. Amount > 0
            3. Apptransid duy nhất
        Expected Result: Response 200 OK, có order_id và order_code
        """
        # Arrange
        amount = 10000
        token = VALID_TOKEN

        # Act
        response = service.create_order(amount, unique_apptransid, token)

        # Assert
        assert response.status_code == 200, f"Expected 200 OK, got {response.status_code}"
        
        try:
            json_response = response.json()
            assert "order_id" in json_response, "Response missing 'order_id'"
            assert "order_code" in json_response, "Response missing 'order_code'"
            assert json_response.get("order_id") is not None
            assert json_response.get("order_code") is not None
        except ValueError:
            pytest.fail("Response is not valid JSON")

    def test_tc_002_invalid_token(self, service, unique_apptransid):
        """
        TC_002: Token không hợp lệ
        Description: Kiểm tra xác thực JWT
        Preconditions: API endpoint available
        Steps: 
            1. Gửi request với token hết hạn/sai
            2. Giữ nguyên các tham số khác
        Expected Result: Response 401 Unauthorized
        """
        # Arrange
        amount = 10000
        token = INVALID_TOKEN

        # Act
        response = service.create_order(amount, unique_apptransid, token)

        # Assert
        assert response.status_code == 401, f"Expected 401 Unauthorized, got {response.status_code}"
```