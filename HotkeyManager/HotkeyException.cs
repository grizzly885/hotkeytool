using System;

namespace hotkeyManager
{
    class HotkeyException : Exception
    {
        public enum ErrorCodes
        {
            AlreadyRegistered = 1409,
            CriticalError = -1
        }

        public ErrorCodes Code { get; private set; }
        public int ErrorCode { get; private set; }

        public HotkeyException(string message, int errorCode):base(message)
        {
            ErrorCode = errorCode;
            Code = GetErrorCode(ErrorCode);
        }

        private ErrorCodes GetErrorCode(int code)
        {
            try
            {
                return (ErrorCodes) code;
            }
            catch (Exception)
            {
                return ErrorCodes.CriticalError;
            }
        }
    }
}
