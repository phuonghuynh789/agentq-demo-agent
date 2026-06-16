import streamlit as st
import requests
import io
import os
import re
import pandas as pd
import subprocess
import openai
import sys
from PyPDF2 import PdfReader
from docx import Document
from dotenv import load_dotenv

load_dotenv()

st.set_page_config(page_title="Test Case Generator", page_icon="📋", layout="wide")

st.title("📋 Test Case Generator Agent")
st.markdown("Cung cấp yêu cầu (PRD) để Agent tự động phân tích và sinh test case.")

# --- File Extraction Functions ---
def extract_text_from_pdf(file_bytes):
    reader = PdfReader(io.BytesIO(file_bytes))
    text = ""
    for page in reader.pages:
        text += page.extract_text() + "\n"
    return text

def extract_text_from_docx(file_bytes):
    doc = Document(io.BytesIO(file_bytes))
    text = ""
    for para in doc.paragraphs:
        text += para.text + "\n"
    return text

# --- UI Setup ---
input_mode = st.radio("Chọn phương thức nhập Requirement:", ("Nhập mô tả (Text)", "Tải lên file (PDF/Word)"))

requirement_text = ""

if input_mode == "Nhập mô tả (Text)":
    requirement_text = st.text_area("Description (Mô tả yêu cầu):", height=200, placeholder="Nhập chuỗi tiếng Việt mô tả yêu cầu cho PRD ở đây...")
    image_file = st.file_uploader("Hoặc dán/tải lên hình ảnh mô tả (Click vào đây và ấn Ctrl+V / Cmd+V)", type=["png", "jpg", "jpeg"])
else:
    image_file = None
    uploaded_file = st.file_uploader("File (Chọn file PRD)", type=["pdf", "docx", "doc"])
    if uploaded_file is not None:
        try:
            if uploaded_file.name.endswith('.pdf'):
                requirement_text = extract_text_from_pdf(uploaded_file.read())
            elif uploaded_file.name.endswith('.docx') or uploaded_file.name.endswith('.doc'):
                requirement_text = extract_text_from_docx(uploaded_file.read())
                
            if len(requirement_text.strip()) < 30:
                st.error(f"Nội dung trích xuất từ file quá ngắn ({len(requirement_text.strip())} ký tự). Vui lòng đảm bảo file chứa ít nhất 30 ký tự mô tả yêu cầu.")
                requirement_text = ""
            else:
                st.success("Đã trích xuất thành công nội dung từ file.")
        except Exception as e:
            st.error(f"Lỗi khi đọc file: {e}")

# --- API Integration ---
ENDPOINT_URL = "https://endpoint-d04c211f-35df-4a15-9ec4-986f332c8ef6.agentbase-runtime.aiplatform.vngcloud.vn/invocations"

if st.button("🚀 Generate Test Case"):
    if image_file is not None:
        with st.spinner("Đang trích xuất yêu cầu từ hình ảnh bằng AI..."):
            try:
                import base64
                client = openai.OpenAI(
                    api_key=os.environ.get("LLM_API_KEY"),
                    base_url=os.environ.get("LLM_BASE_URL")
                )
                base64_image = base64.b64encode(image_file.read()).decode('utf-8')
                response = client.chat.completions.create(
                    model=os.environ.get("LLM_MODEL"),
                    messages=[
                        {
                            "role": "user",
                            "content": [
                                {"type": "text", "text": "Hãy mô tả chi tiết giao diện và trích xuất toàn bộ văn bản/yêu cầu có trong hình ảnh này để làm tài liệu thiết kế Test Case."},
                                {"type": "image_url", "image_url": {"url": f"data:image/jpeg;base64,{base64_image}"}}
                            ]
                        }
                    ],
                    max_tokens=1500
                )
                extracted_text = response.choices[0].message.content
                requirement_text += f"\n\n[Mô tả từ Hình ảnh đính kèm]:\n{extracted_text}"
            except Exception as e:
                st.error(f"Lỗi khi phân tích hình ảnh (có thể model hiện tại không hỗ trợ Vision): {e}")

    if not requirement_text.strip():
        st.warning("Vui lòng cung cấp mô tả yêu cầu, hình ảnh, hoặc tải lên file hợp lệ trước khi Generate.")
    elif len(requirement_text.strip()) < 30:
        st.warning("Mô tả yêu cầu quá ngắn. Vui lòng nhập ít nhất 30 ký tự để Agent có đủ thông tin phân tích và sinh Test Case.")
    else:
        final_requirement = requirement_text
                
        st.session_state.original_requirement = final_requirement
        st.info("Đang gọi Agent... (có thể mất 1-2 phút tùy vào độ dài của yêu cầu)")
        
        prompt_message = f"""Step 1:
Analyze the feature complexity and assign a Complexity Score from 1 to 10.

Step 2:
Determine the recommended number of test cases based on complexity.

Complexity Rules:

Score 1-3: Generate up to 15 test cases
Score 4-6: Generate up to 30 test cases
Score 7-8: Generate up to 50 test cases
Score 9-10: Generate up to 80 test cases

Step 3:
Distribute test cases by risk:

Functional: 40%
Negative: 30%
Boundary & Edge: 15%
Security: 10%
Regression: 5%

Step 4:
Generate test cases.

Output:

Complexity Score
Recommended Number of Test Cases
Risk Areas Identified
Test Cases Table

Requirement:
{final_requirement}"""

        payload = {
            "message": prompt_message
        }
        headers = {
            "Content-Type": "application/json",
            "X-GreenNode-AgentBase-User-Id": "streamlit-user",
            "X-GreenNode-AgentBase-Session-Id": "streamlit-session"
        }
        
        try:
            with st.spinner("Agent đang suy nghĩ và tạo Test Cases..."):
                response = requests.post(ENDPOINT_URL, json=payload, headers=headers)
                response.raise_for_status()
                result = response.json()
                
            st.success("Tạo Test Case thành công!")
            
            # Display response
            st.markdown("### Kết quả")
            if "response" in result:
                full_response = result["response"]
                st.session_state.full_response = full_response
                st.markdown("### Kết quả (Test Cases Generator)")
                
                # Parse markdown tables manually for robustness
                lines = full_response.strip().split('\n')
                tables = []
                current_table = []
                for i, line in enumerate(lines):
                    if '|' in line:
                        current_table.append((i, line))
                    else:
                        if len(current_table) >= 2:
                            tables.append(current_table)
                        current_table = []
                if len(current_table) >= 2:
                    tables.append(current_table)
                    
                if tables:
                    # Tìm bảng có nhiều cột nhất -> Chính là bảng Test Cases chính
                    def get_num_cols(table_tuples):
                        return len([c for c in table_tuples[0][1].split('|') if c.strip()])
                    
                    test_case_table = max(tables, key=get_num_cols)
                    
                    start_idx = test_case_table[0][0]
                    end_idx = test_case_table[-1][0]
                    
                    # Text trước và sau bảng Test Case (để giữ nguyên cấu trúc Markdown gốc)
                    eval_text_before = '\n'.join(lines[:start_idx])
                    eval_text_after = '\n'.join(lines[end_idx+1:])
                    
                    t_lines = [item[1] for item in test_case_table]
                    
                    def split_row(row_str):
                        cols = [c.strip() for c in row_str.split('|')]
                        if cols and cols[0] == '': cols.pop(0)
                        if cols and cols[-1] == '': cols.pop(-1)
                        return cols
                        
                    headers = split_row(t_lines[0])
                    data = []
                    for line in t_lines[2:]:
                        cols = split_row(line)
                        if len(cols) == len(headers):
                            data.append(cols)
                        elif len(cols) > 0:
                            data.append(cols + [""] * (len(headers) - len(cols)))
                    
                    df = pd.DataFrame(data, columns=headers)
                    df.insert(0, 'Automation test', False)
                    
                    st.session_state.tc_df = df
                    st.session_state.eval_text_before = eval_text_before
                    st.session_state.eval_text_after = eval_text_after
                else:
                    st.session_state.full_response_only = full_response
            else:
                st.json(result)
                
        except requests.exceptions.RequestException as e:
            st.error(f"Lỗi khi gọi Endpoint Agent: {e}")
            if hasattr(e, 'response') and e.response is not None:
                st.error(f"Chi tiết lỗi: {e.response.text}")

if 'tc_df' in st.session_state:
    st.markdown("---")
    
    # Hiển thị phần Markdown TRƯỚC bảng Test Case
    st.markdown(st.session_state.eval_text_before)
    
    # Render bảng Test Case có Checkbox ngay vị trí của nó
    edited_df = st.data_editor(st.session_state.tc_df, hide_index=True, use_container_width=True)
    
    # Render phần Markdown SAU bảng Test Case (chẳng hạn như Summary Table)
    st.markdown(st.session_state.eval_text_after)
    
    st.markdown("---")
    if st.button("🚀 Run test Automation"):
        selected_rows = edited_df[edited_df['Automation test'] == True]
        if selected_rows.empty:
            st.warning("Vui lòng tick chọn ít nhất 1 Test Case trong cột 'Automation test' để chạy.")
        else:
            st.info("Đang sinh Python Test Script từ các Test Case đã chọn...")
            try:
                client = openai.OpenAI(
                    api_key=os.environ.get("LLM_API_KEY"),
                    base_url=os.environ.get("LLM_BASE_URL")
                )
                
                tc_json = selected_rows.drop(columns=['Automation test']).to_json(orient="records", force_ascii=False)
                original_req = st.session_state.get('original_requirement', '')
                
                prompt = f'''
You are an SDET. Write a complete, valid Python `pytest` script that implements the following test cases.
Use a Service Object or Page Object Model structure (you can mock the POM classes or Service classes at the top of the file).

CRITICAL REQUIREMENT:
- You must include detailed print or logging statements for the actual API Request (headers, payloads, URL/endpoint) and API Response (status code, body) within the Service Object, Page Object, or test cases so that it can be inspected when running tests.

ORIGINAL API/FEATURE REQUIREMENT (USE THIS AS CONTEXT FOR URLS/ENDPOINTS/PAYLOADS):
{original_req}

TEST CASES TO AUTOMATE:
{tc_json}

Return your response in this exact format:
FILENAME: <suggested_filename>.py
```python
<python_code_here>
```

Naming rules for the filename:
- If this is a gRPC API test, use format: test_grpc_<api_name>.py
- If this is a REST API test, use format: test_rest_<api_name>.py
- Otherwise, use format: test_<feature_name>.py
'''
                response = client.chat.completions.create(
                    model=os.environ.get("LLM_MODEL"),
                    messages=[{"role": "user", "content": prompt}],
                    temperature=0.1
                )
                code_text = response.choices[0].message.content
                
                # Extract filename
                filename_match = re.search(r'FILENAME:\s*(\S+\.py)', code_text)
                if filename_match:
                    file_name = filename_match.group(1)
                else:
                    file_name = "test_generated.py"
                    
                match = re.search(r'```python\n(.*?)\n```', code_text, re.DOTALL)
                code = match.group(1) if match else code_text
                
                os.makedirs("web_app/automation/tests", exist_ok=True)
                os.makedirs("web_app/Report automation", exist_ok=True)
                
                import datetime
                date_prefix = datetime.datetime.now().strftime("%y%m%d_%H%M%S_")
                
                # File Python Script
                py_file_name = f"{date_prefix}{file_name}"
                test_file_path = os.path.join("web_app/automation/tests", py_file_name)
                with open(test_file_path, "w", encoding="utf-8") as f:
                    f.write(code)
                    
                # File dữ liệu CSV
                csv_file_name = f"{date_prefix}{file_name.replace('.py', '.csv')}"
                csv_path = os.path.join("web_app/Report automation", csv_file_name)
                selected_rows.to_csv(csv_path, index=False, encoding="utf-8-sig")
                    
                # File Log
                base_log_name = file_name.replace("test_", "log_test_").replace(".py", ".cs")
                log_file_name = f"{date_prefix}{base_log_name}"
                log_file_path = os.path.join("web_app/Report automation", log_file_name)
                with open(log_file_path, "w", encoding="utf-8") as f:
                    f.write("========== REQUEST (PROMPT) ==========\n")
                    f.write(prompt)
                    f.write("\n\n========== RESPONSE ==========\n")
                    f.write(code_text)
                    
                st.success("✅ Đã sinh mã kiểm thử thành công! Đang thực thi Pytest...")
                
                # File Báo cáo HTML
                report_file_name = f"{date_prefix}report_{file_name.replace('.py', '.html')}"
                report_path = os.path.join("web_app/Report automation", report_file_name)
                result = subprocess.run(
                    [sys.executable, "-m", "pytest", test_file_path, f"--html={report_path}", "--self-contained-html"],
                    capture_output=True,
                    text=True
                )
                
                st.markdown(f"**Kết quả Pytest (Exit code: {result.returncode})**")
                with st.expander("Xem chi tiết Log Console"):
                    st.code(result.stdout)
                    
                if os.path.exists(report_path):
                    with open(report_path, "rb") as f:
                        st.download_button("📥 Tải xuống Report HTML", data=f, file_name="report.html", mime="text/html")
                        
            except Exception as e:
                st.error(f"Lỗi trong quá trình Automation: {e}")
                
                # Ghi log request/response (hoặc error) khi chạy lỗi
                try:
                    os.makedirs("web_app/Report automation", exist_ok=True)
                    import datetime
                    date_prefix = datetime.datetime.now().strftime("%y%m%d_%H%M%S_")
                    log_file_path = os.path.join("web_app/Report automation", f"{date_prefix}log_test_failed.cs")
                    
                    with open(log_file_path, "w", encoding="utf-8") as f:
                        f.write("========== REQUEST (PROMPT) ==========\n")
                        f.write(prompt if 'prompt' in locals() else "Chưa tạo được prompt")
                        f.write("\n\n========== ERROR / RESPONSE ==========\n")
                        f.write(str(e))
                        if 'code_text' in locals():
                            f.write("\n\n========== RAW CODE TEXT ==========\n")
                            f.write(code_text)
                except Exception:
                    pass

elif 'full_response_only' in st.session_state:
    st.markdown("---")
    st.markdown(st.session_state.full_response_only)
