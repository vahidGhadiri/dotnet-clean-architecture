GET    /members/{memberId}/photos
GET    /members/{memberId}/photos/{photoId}
POST   /members/{memberId}/photos
DELETE /members/{memberId}/photos/{photoId}
PUT    /members/{memberId}/photos/{photoId}


**Add this script to generate contracts automatically**
```angular2html
#!/bin/bash

OPENAPI_URL="http://localhost:4000/openapi/v1.json"

OUTPUT_DIR="src/features"

rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

echo "Downloading OpenAPI..."

curl -s "$OPENAPI_URL" -o openapi.json

TAGS=$(jq -r '.paths[][]?.tags[]?' openapi.json | sort -u)

for tag in $TAGS; do

  SAFE_TAG=$(echo "$tag" \
    | tr '[:upper:]' '[:lower:]' \
    | tr -cs 'a-z0-9' '-')

  echo "Generating contracts for: $SAFE_TAG"

  npx openapi-typescript-codegen \
    --input openapi.json \
    --output "$OUTPUT_DIR/$SAFE_TAG/contracts" \
    --useUnionTypes \
    --client fetch

done

rm openapi.json

FULL_PATH="$(pwd)/$OUTPUT_DIR"

echo ""
echo "Generation completed"
echo "Output: file://$FULL_PATH"
```