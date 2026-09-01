#!/bin/sh -l

# echo "Updating..."
apt-get update
apt-get install -y git gnupg gnupg2 gnupg1
#  unzip wget

# Install Mono 
apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | tee /etc/apt/sources.list.d/mono-official-stable.list
apt update
apt install mono-complete --yes

# Get DocFX
# wget https://github.com/dotnet/docfx/releases/download/v2.45/docfx.zip
# unzip docfx.zip -d _docfx

# Git Clone
git clone https://emiliano84:$1@github.com/emiliano84/Toolkit.git
git clone -b dev https://emiliano84:$1@github.com/emiliano84/Toolkit.Docs.git

# Echo current directory and list
echo $PWD
echo "$(ls)"

# cd Toolkit.Docs Echo directory and list
cd Toolkit.Docs/
echo $PWD
echo "$(ls)"

# Echo lists
echo "$(ls /github/workspace/Toolkit/UwpCommunity.Uwp.Controls/)"
echo "$(ls /github/workspace/Toolkit.Docs/metadata/uwp.controls/)"

# Build docs
mono docfx/docfx.exe docfx.json



# echo "Hello $1"

# time=$(date)
# echo ::set-output name=time::$time