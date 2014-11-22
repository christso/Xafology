using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;

namespace Xafology.ExpressApp.SystemModule
{
    [NonPersistent]
    public class OpenFileData : IFileData
    {
        public OpenFileData()
        {
        }

        public void Load(string fileName, byte[] buffer)
        {
            Guard.ArgumentNotNull(buffer, "buffer");
            Guard.ArgumentNotNullOrEmpty(fileName, "fileName");
            this.FileName = fileName;
            this.Content = buffer;
        }

        public void LoadFromStream(string fileName, Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");
            Guard.ArgumentNotNullOrEmpty(fileName, "fileName");
            this.FileName = fileName;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            this.Content = buffer;
        }

        public void SaveToStream(Stream stream)
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                throw new InvalidOperationException();
            }
            stream.Write(this.Content, 0, this.Size);
            stream.Flush();
        }

        public void Clear()
        {
            Content = null;
            FileName = string.Empty;
        }

        public override string ToString()
        {
            return _FileName;
        }

        private string _FileName = "";
        [Size(260)]
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (value == null)
                {
                    _FileName = "";
                }
                else
                {
                    _FileName = value;
                }
            }
        }

        private byte[] _Content;
        [MemberDesignTimeVisibility(false)]
        public byte[] Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
                if (value != null)
                {
                    _Size = value.Length;
                }
                else
                {
                    _Size = 0;
                }
            }
        }
        private int _Size;
        public int Size
        {
            get { return _Size; }
        }
    }
}
