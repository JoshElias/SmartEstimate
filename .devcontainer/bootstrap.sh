#!/usr/bin/env bash

# Check if the SmartOrderApi directory exists and is a Git repository
for project in SmartOrderApi daemon-dotnet; do
    [[ -d "../$project/.git" ]] && continue;
    git clone "$GITHUB_DAEMONTECHTOOLS/$project" "../$project"
done

## Sanity checking....
declare -x IN_DOCKER=false IN_VSCODE=false

### If we are running docker, then the container knows what to do
if [[ -f .dockerenv ]]; then
    export IN_DOCKER=true;
fi

### Same thing as vscode; the extension handles setting things up
if ${VSCODE:-false}; then
    export IN_VSCODE=true;
fi

### Exit early if setup is handled by others
if $IN_DOCKER || $IN_VSCODE; then
    exit 0;
fi

### Otherwise, we need to make we have some dependencies
declare -a deps=( 'docker' 'jq' );
declare red="" reset=""
red="$(printf "%s\n" 'enacs' 'smacs' 'setaf 1' |tput -S)";
reset="$(printf "sgr0\nrmacs" |tput -S)";

alias printerr='printf "$red%s$reset\n"';
for dep in "${deps[@]}"; do
    if '!' command -v $dep &>/dev/null; then
        printerr \
            "Dependency is not installed: '$dep'"\
            "Please install it and try again";
        exit 1
    fi
done

cd "./.devcontainer" || exit 1;
declare metafl="$PWD/devcontainer.json"
declare -x IMAGENAME=""
IMAGENAME="$(jq -Mr '.service' "$metafl")"

declare build_image=false;
[[ -z "$(docker images --all -q "$IMAGENAME")" ]] && build_image=true;

if $build_image; then
    declare builder_name="${IMAGENAME}-build";
    docker buildx create --name "$builder_name" --use &&
    docker buildx bake \
        -f docker-compose.yml \
        --set "*.platform=linux/amd64" \
        --set "*.cache-from=type=registry,ref=${IMAGENAME,,}:cache" \
        --load ||
    docker buildx rm "$builder_name"
fi