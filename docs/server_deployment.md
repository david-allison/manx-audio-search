
# update site (after setup below is done(
```
su david2
sudo /bin/systemctl stop manx-audio.service
cd /home/david2/manx-audio-data && git pull
cd /home/david2/manx-audio-search && git pull
rm -rf /var/www/manx-audio/manx-audio-search
cp -r /home/david2/manx-audio-search/ /var/www/manx-audio/
cp -r /home/david2/audio /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch
cp -r /home/david2/manx-audio-data/AudioData /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch
cd /var/www/manx-audio/manx-audio-search/ManxAudioSearch && dotnet build && dotnet publish
sudo /bin/systemctl start manx-audio.service

```


# setup git repos
```
su david2

git clone https://github.com/david-allison/manx-audio-data.git
git clone https://github.com/david-allison/manx-audio-search.git
cd manx-audio-data && git submodule update --init --recursive
```

# load audio from remote machine
```
mkdir audio
# on a remote machine. CHANGE `SERVER_NAME` to the server 
scp -r /Users/davidallison/StudioProjects/manx-audio-search/ManxAudioSearch/ManxAudioSearch/ClientApp/public/audio root@SERVER_NAME:/home/david2/audio
```

# back on local machine: setup /var/www folder
```
cd /var/www
rmkdir manx-audio
chown david2 manx-audio

exit # to root
```

# setup nginx config
```
sudo certbot --nginx -d manxaudio.com -d www.manxaudio.com

... edit nginx

... chown david2 manx-audio

su david2
```


# enable david2 to modify service using sudo with no password

```
# as root
cd /etc/

nano sudoers

# add lines:

david2 ALL= NOPASSWD: /bin/systemctl start manx-audio.service
david2 ALL= NOPASSWD: /bin/systemctl stop manx-audio.service
david2 ALL= NOPASSWD: /bin/systemctl restart manx-
```

# setup service
```
# 
cd /etc/systemd/system
touch manx-audio.service

# content

[Unit]
Description=Manx Audio Searchh

[Service]
WorkingDirectory=/var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch/bin/Debug/net6.0/publish/
ExecStart=/usr/bin/dotnet /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch/bin/Debug/net6.0/publish/ManxAudioSearch.dll
Restart=always
RestartSec=10
SyslogIdentifier=manxaudio
User=david2
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=ASPNETCORE_URLS="http://localhost:5555"

[Install]
WantedBy=multi-user.target
```