#!/bin/bash
# usage:
# /Users/davidallison/StudioProjects/manx-audio-search/ManxAudioSearch/ManxAudioSearch/ClientApp/public/audio
# davidallison@Davids-MacBook-Pro audio % /Users/davidallison/StudioProjects/manx-audio-search/ManxAudioSearch/tools/macos_json_folder.sh /Users/davidallison/StudioProjects/manx-audio-search/ManxAudioSearch/ManxAudioSearch/ClientApp/public/audio

if [ -z "$1" ]; then
  echo "File not supplied."
  exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

audio="${1}/*.mp3"
for filename in $audio; do
    $SCRIPT_DIR//macos_gen_json.sh "$filename"
done