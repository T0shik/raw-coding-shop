#!/bin/bash

# Ubuntu 20.04LTS

# setup user
adduser <username>
usermod -aG sudo <username>
mkdir /home/<username>/.ssh
touch /home/<username>/.ssh/authorized_keys # <- put ssh public key here
sudo ufw allow 'OpenSSH'

# setup github actions account
sudo adduser <ci_cd_username>
sudo mkdir /var/app
sudo chown -R <ci_cd_username>:<ci_cd_username> /var/app
sudo mkdir /home/<ci_cd_username>/.ssh
sudo mkdir /home/<ci_cd_username>/.ssh/authorized_keys # <- put ssh public key here


# dotnet core 3.1
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-3.1

# postgres 12
sudo wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
echo "deb http://apt.postgresql.org/pub/repos/apt/ `lsb_release -cs`-pgdg main" | sudo tee  /etc/apt/sources.list.d/pgdg.list
sudo apt-get update
sudo apt-get install -y postgresql-12

# nginx
sudo apt-get install nginx

# firewall
sudo ufw allow 'Nginx HTTP'

sudo passwd postgres
sudo service postgresql start