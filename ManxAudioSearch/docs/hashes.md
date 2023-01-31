

If audio files go missing, there is a possibility that searching for the hash will return the files. 
We provide audio with the following hash outputs to improve the likeihood of this occurring

* MD5
* SHA-1
* SHA-2-256
* SHA-3
* CRC32

## SHA-3 data

```
davidallison@Davids-MacBook-Pro audio % sha3sum -v laajea.mp3           
rate: 1152
capacity: 448
output size: 224
state size: 1600
word size: 64
squeezes: 1
suffix: 01
```

# macOS utils used

* `sha256sum  | cut -d ' ' -f 1`
* `shasum  | cut -d ' ' -f 1`
* `md5 -q`
* `crc32`
* `sha3sum	 | cut -d ' ' -f 1`
  * `brew install sha3sum`
