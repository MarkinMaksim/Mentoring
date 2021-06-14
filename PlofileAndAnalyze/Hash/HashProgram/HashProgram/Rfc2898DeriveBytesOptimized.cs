

using System;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace HashProgram
{
    public class Rfc2898DeriveBytesOptimized : DeriveBytes
    {
        private const int MinimumSaltSize = 8;

        private readonly byte[] _password;
        private byte[] _salt;
        private uint _iterations;
        private HMAC _hmac;
        private int _blockSize = 20;

        private byte[] _buffer;
        private uint _block;
        private int _startIndex;
        private int _endIndex;

        public HashAlgorithmName HashAlgorithm { get; }

        public Rfc2898DeriveBytesOptimized(string password, int saltSize)
            : this(password, saltSize, 1000)
        {
        }

        public Rfc2898DeriveBytesOptimized(string password, int saltSize, int iterations)
            : this(password, saltSize, iterations, HashAlgorithmName.SHA1)
        {
        }

        [SecuritySafeCritical]
        public Rfc2898DeriveBytesOptimized(byte[] password, byte[] salt, int iterations)
        {
            Salt = salt;
            IterationCount = iterations;
            _password = password;
            _hmac = new HMACSHA1(password);
            Initialize();
        }

        public Rfc2898DeriveBytesOptimized(string password, int saltSize, int iterations, HashAlgorithmName hashAlgorithm)
        {
            if (saltSize < 0)
                throw new ArgumentOutOfRangeException();
            if (saltSize < MinimumSaltSize)
                throw new ArgumentException();
            if (iterations <= 0)
                throw new ArgumentOutOfRangeException();

            _salt = new byte[saltSize + sizeof(uint)];
            RandomNumberGenerator.Fill(_salt.AsSpan(0, saltSize));

            _iterations = (uint)iterations;
            _password = Encoding.UTF8.GetBytes(password);
            HashAlgorithm = hashAlgorithm;
            _hmac = OpenHmac();
            // _blockSize is in bytes, HashSize is in bits.
            //_blockSize = _hmac.HashSize >> 3;

            Initialize();
        }

        public int IterationCount
        {
            get
            {
                return (int)_iterations;
            }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _iterations = (uint)value;
                Initialize();
            }
        }

        public byte[] Salt
        {
            get
            {
                return _salt.AsSpan(0, _salt.Length - sizeof(uint)).ToArray();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length < MinimumSaltSize)
                    throw new ArgumentException();

                _salt = new byte[value.Length + sizeof(uint)];
                value.AsSpan().CopyTo(_salt);
                Initialize();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_hmac != null)
                {
                    _hmac.Dispose();
                    _hmac = null;
                }

                if (_buffer != null)
                    Array.Clear(_buffer, 0, _buffer.Length);
                if (_password != null)
                    Array.Clear(_password, 0, _password.Length);
                if (_salt != null)
                    Array.Clear(_salt, 0, _salt.Length);
            }
            base.Dispose(disposing);
        }

        public override byte[] GetBytes(int cb)
        {
            Debug.Assert(_blockSize > 0);

            if (cb <= 0)
                throw new ArgumentOutOfRangeException();
            byte[] password = new byte[cb];

            int offset = 0;
            int size = _endIndex - _startIndex;
            if (size > 0)
            {
                if (cb >= size)
                {
                    Buffer.BlockCopy(_buffer, _startIndex, password, 0, size);
                    _startIndex = _endIndex = 0;
                    offset += size;
                }
                else
                {
                    Buffer.BlockCopy(_buffer, _startIndex, password, 0, cb);
                    _startIndex += cb;
                    return password;
                }
            }

            Debug.Assert(_startIndex == 0 && _endIndex == 0, "Invalid start or end index in the internal buffer.");

            while (offset < cb)
            {
                _buffer = Func();
                int remainder = cb - offset;
                if (remainder >= _blockSize)
                {
                    Buffer.BlockCopy(_buffer, 0, password, offset, _blockSize);
                    offset += _blockSize;
                }
                else
                {
                    Buffer.BlockCopy(_buffer, 0, password, offset, remainder);
                    _startIndex = remainder;
                    _endIndex = _buffer.Length;
                    return password;
                }
            }
            return password;
        }

        public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
        {
            // If this were to be implemented here, CAPI would need to be used (not CNG) because of
            // unfortunate differences between the two. Using CNG would break compatibility. Since this
            // assembly currently doesn't use CAPI it would require non-trivial additions.
            // In addition, if implemented here, only Windows would be supported as it is intended as
            // a thin wrapper over the corresponding native API.
            // Note that this method is implemented in PasswordDeriveBytes (in the Csp assembly) using CAPI.
            throw new PlatformNotSupportedException();
        }

        public override void Reset()
        {
            Initialize();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5350", Justification = "HMACSHA1 is needed for compat. (https://github.com/dotnet/corefx/issues/9438)")]
        private HMAC OpenHmac()
        {
            Debug.Assert(_password != null);

            HashAlgorithmName hashAlgorithm = HashAlgorithm;

            if (string.IsNullOrEmpty(hashAlgorithm.Name))
                throw new CryptographicException();

            if (hashAlgorithm == HashAlgorithmName.SHA1)
                return new HMACSHA1(_password);
            if (hashAlgorithm == HashAlgorithmName.SHA256)
                return new HMACSHA256(_password);
            if (hashAlgorithm == HashAlgorithmName.SHA384)
                return new HMACSHA384(_password);
            if (hashAlgorithm == HashAlgorithmName.SHA512)
                return new HMACSHA512(_password);

            throw new CryptographicException();
        }

        private void Initialize()
        {
            if (_buffer != null)
                Array.Clear(_buffer, 0, _buffer.Length);
            _buffer = new byte[_blockSize];
            _block = 1;
            _startIndex = _endIndex = 0;
        }

        // This function is defined as follows:
        // Func (S, i) = HMAC(S || i) ^ HMAC2(S || i) ^ ... ^ HMAC(iterations) (S || i) 
        // where i is the block number.
        private byte[] Func()
        {
             byte[] INT_block = Utils.Int(_block);

            _hmac.TransformBlock(_salt, 0, _salt.Length, null, 0);
            _hmac.TransformBlock(INT_block, 0, INT_block.Length, null, 0);
            _hmac.TransformFinalBlock(new Byte[0], 0, 0);
            byte[] temp = _hmac.Hash;
            //byte[] temp = m_hmacsha1.HashValue;
            _hmac.Initialize();

            byte[] ret = temp;
            for (int i = 2; i <= _iterations; i++)
            {
                _hmac.TransformBlock(temp, 0, temp.Length, null, 0);
                _hmac.TransformFinalBlock(new Byte[0], 0, 0);
                temp = _hmac.Hash;
                //temp = m_hmacsha1.HashValue;
                for (int j = 0; j < _blockSize; j++)
                {
                    ret[j] ^= temp[j];
                }
                _hmac.Initialize();
            }

            // increment the block count.
            _block++;
            return ret;
        }
    }
}
