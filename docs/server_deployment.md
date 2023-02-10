```
su david2

git clone https://github.com/david-allison/manx-audio-data.git
git clone https://github.com/david-allison/manx-audio-search.git
cd manx-audio-data && git submodule update --init --recursive
```

# load 
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
----

cd /home/david2/manx-audio-data && git pull
cd /home/david2/manx-audio-search && git pull
rm -rf /var/www/manx-audio/manx-audio-search
cp -r /home/david2/manx-audio-search/ /var/www/manx-audio/
cp -r /home/david2/audio /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch
cp -r /home/david2/manx-audio-data/AudioData /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch

cd /var/www/manx-audio/manx-audio-search/ManxAudioSearch && dotnet build && dotnet publish && cd /var/www/manx-audio/manx-audio-search/ManxAudioSearch/ManxAudioSearch/bin/Debug/net6.0/publish/ 
ASPNETCORE_URLS=http://localhost:5555 ./ManxAudioSearch

----
setup service