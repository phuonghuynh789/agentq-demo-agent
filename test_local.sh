#!/bin/bash
echo "Starting server..."
venv/bin/python main.py &
SERVER_PID=$!

echo "Waiting for health endpoint..."
for i in $(seq 1 15); do
  STATUS=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:8080/health 2>/dev/null)
  if [ "$STATUS" = "200" ]; then echo "Health OK"; break; fi
  sleep 2
done

echo "Testing valid invocation..."
curl -s -w "\n%{http_code}" -X POST http://localhost:8080/invocations \
  -H "Content-Type: application/json" \
  -H "X-GreenNode-AgentBase-User-Id: test-user" \
  -H "X-GreenNode-AgentBase-Session-Id: test-session" \
  -d '{"message": "Generate test cases for a login page"}'

echo -e "\n\nTesting empty invocation..."
curl -s -w "\n%{http_code}" -X POST http://localhost:8080/invocations \
  -H "Content-Type: application/json" \
  -H "X-GreenNode-AgentBase-User-Id: test-user" \
  -H "X-GreenNode-AgentBase-Session-Id: test-session" \
  -d '{}'

echo -e "\n\nStopping server..."
kill $SERVER_PID 2>/dev/null
