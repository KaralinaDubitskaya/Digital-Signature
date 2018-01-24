# Digital Signature

The program calculates and verifies the electronic digital signature based on the RSA algorithm.
SHA-1 is used to calculate the hash function.

## SHA-1

SHA-1 (Secure Hash Algorithm 1) is a cryptographic hash function which takes an input and produces
a 160-bit (20-byte) hash value. SHA-1 is used to verify that a file has been unaltered. This is done by
producing a checksum before the file has been transmitted, and then again once it reaches its destination.
The transmitted file can be considered genuine only if both checksums are identical. Even a small change
in the message will, with overwhelming probability, result in many bits changing due to the avalanche effect. 

## RSA algorithm

In RSA, signing a message is equal to encrypting the hash of the message with your private key.
Due to the specific properties of RSA, this allows anyone to decrypt the signature if they have the
public key corresponding to the private key. However, this is not necessarily the case for every 
signature algorithm.

The keys for the RSA algorithm are generated the following way:

1. Choose two distinct prime numbers P and Q.
   > For security purposes, the integers P and Q should be chosen at random, and should be similar in magnitude but
   > differ in length by a few digits to make factoring harder. Prime integers can be efficiently found using a 
   > primality test.
2. Compute R = P * Q.
   > R is used as the modulus for both the public and private keys.
3. Compute the Euler totient function φ(R) = (P − 1)(Q − 1). This value is kept private.
4. Choose an integer E such that 1 < E < φ(R) and gcd(E, φ(R)) = 1; i.e., E and φ(R) are coprime.
5. Determine D as D ≡ E − 1 (mod φ(R)); i.e., D is the modular multiplicative inverse of E (modulo φ(R)).

E is released as the public key exponent.
D is kept as the private key exponent.
The public key consists of the modulus R and the public (or encryption) exponent E.
The private key consists of the modulus R and the private (or decryption) exponent D, which must be kept secret.
P, Q, and φ(R) must also be kept secret because they can be used to calculate D.

The sender signs the document M using the private key (D, R) as follows:
```
                                  S = H(M)^D mod R,
```
where H(M) is a hash of the message M.

The recipient verifies the authenticity of the document M using the sender's public key (E, R) as follows: 
```
                                  H(M) = S^E mod R.
```
After what calculate a hash of the received document and compare it with H(M).
If the values are equal, the document is authentic.

## Usage

###### 1. How to sign file

Click "Browse..." and select the file. The program can process files of small size (up to several megabytes)
of various types (text files, pictures, etc.). To sign the file, enter the private keys (P, Q and D (or E)) 
and click the button "Sign". In this case, a new text file containing a digital signature will be created
in the folder with the source file.

###### 2. How to verify file

Select the source file and the file, that contains digital signature. Enter the public keys (E and R) and click 
the button "Verify". The program will display a message whether the file is authentic.

![alt text](Digital-Signature/Digital Signature/Digital Signature/screen.JPG)

## Contact
KarolinaDubitskaya                                                                                                                     
KarolinaDubitskaya@gmail.com
