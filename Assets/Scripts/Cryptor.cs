using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class Cryptor : MonoBehaviour {

    // These 3 private values define the pseudo encryption algorithm.
    private int complexity = 3;
    private string salt_prefix = "Go West";
    private string salt_postfix = "Reset";

    // php function ported to unity
    private string base64_encode(string input) { 
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    // php function ported to unity
    private string base64_decode(string input) { 
        byte[] bytes = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(bytes);
    }

    // encrypts an input string
    public string Encrypt(string input_clean) {
        input_clean = this.salt_prefix + input_clean + this.salt_postfix;
        for (int i=0; i<this.complexity; i++) {
            input_clean = base64_encode(input_clean);
        }
        string input_encrypted = input_clean;
        return input_encrypted;
    }

    // decrypts an input string
    public string Decrypt(string input_encrypted) {
        for (int i=0; i<this.complexity; i++) {
            input_encrypted = base64_decode(input_encrypted);
        }
        int salt_len_prefix = this.salt_prefix.Length;
        int salt_len_postfix = this.salt_postfix.Length;
        string input_decrypted = input_encrypted.Substring(salt_len_prefix, input_encrypted.Length - salt_len_prefix - salt_len_postfix);
        return input_decrypted;
    }
}
