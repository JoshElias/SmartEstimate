# Start from a base image
FROM ubuntu:20.04 as base
ARG SHELL="/usr/bin/bash"
ARG NODE_VERSION=20

## Set up other repositories
RUN apt-get update && \
    apt-get install -y software-properties-common && \
    ## git-flow-avh
    add-apt-repository -y ppa:git-core/ppa

# Install zsh, python, perl, nodejs, and .NET Core
RUN apt-get update && apt-get install -y \
    curl wget gnupg2 lsb-release \
    git git-flow

## Setup NodeJS via Volta
ENV PATH="/root/.volta/bin:$PATH"
ENV SHELL=${SHELL}
RUN curl https://get.volta.sh |bash

RUN volta install "node@$NODE_VERSION" && \
    volta install pnpm && \
    pnpm setup && \
    . /root/.bashrc && \
    pnpm add -g tailwindcss@latest postcss@latest 

# Install .NET Core SDK
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get update && \
    apt-get install -y \
        apt-transport-https \
        dotnet-sdk-8.0
        
# Cleanup package manager
RUN apt-get clean && \
    rm -rf /var/lib/apt/lists/

# Set bash as the default shell
RUN chsh -s $(which bash)

FROM base as dev

ENV PROJECT_LOCATION=/workspace/SmartOrder
WORKDIR $PROJECT_LOCATION

COPY ./.devcontainer/bootstrap.sh /tmp/bootstrap.sh
RUN chmod +x /tmp/bootstrap.sh && /tmp/bootstrap.sh

VOLUME [\
    "/workspaces/SmartOrder", \
    "/workspaces/SmartOrderApi", \
    "/workspaces/daemon-dotnet" \
]