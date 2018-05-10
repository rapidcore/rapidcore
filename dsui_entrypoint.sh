#!/bin/sh

echo "{
  \"projects\": [
    {
      \"service\": \"datastore\",
      \"projectId\": \"${PROJECT_ID}\",
      \"apiEndpoint\": \"${DATASTORE_ENDPOINT}\",
      \"id\": \"0d08bae6-1cb0-4973-8d11-a7b483f15406\"
    }
  ]
}" > /root/.google-cloud-gui-db.json

echo About to start google-cloud-gui on http://0.0.0.0:$UI_PORT

google-cloud-gui --port=$UI_PORT --skip-browser
