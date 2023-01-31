#!/bin/bash
if [ -z "$1" ]; then
  echo "File not supplied."
  exit 1
fi
md5=`md5 -q "$1"`
sha1=`shasum "$1" | cut -d ' ' -f 1`
sha256=`sha256sum "$1" | cut -d ' ' -f 1`
crc=`crc32 "$1"`
sha3=`sha3sum "$1" | cut -d ' ' -f 1`


creationDateUS=`GetFileInfo -d "$1"`
creationdate=`date -j -f "%m/%d/%Y %H:%M:%S" "$creationDateUS" +"%Y-%m-%d"`
filename=`basename "$1"`

json=$(jq --null-input \
  --arg filename "$filename" \
  --arg creationDate "$creationdate" \
  --arg md5 "$md5" \
  --arg sha1 "$sha1" \
  --arg sha256 "$sha256" \
  --arg sha3 "$sha3" \
  --arg crc "$crc" \
  '{"filename": $filename, "created": $creationDate, hashes: { "md5": $md5, "sha1": $sha1, "sha256": $sha256, "sha3": $sha3, "crc": $crc } }')
  # echo "${1}.json: $json"
  echo "$json" >  "${1}.json"
  echo "${1}.json"