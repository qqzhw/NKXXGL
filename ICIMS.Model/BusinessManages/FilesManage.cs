using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Model.BusinessManages
{
    public class FilesManage: BindableBase
    {
        public long Id { get; set; }
        private int? _entityId;
        public int? EntityId { get => _entityId; set => SetProperty(ref _entityId, value); }
        private string _entityKey;
        public string EntityKey { get => _entityKey; set => SetProperty(ref _entityKey, value); }
        private string _entityName;
        public string EntityName { get => _entityName; set => SetProperty(ref _entityName, value); }
        
        /// <summary>
        /// DownloadUrl
        /// </summary>
        private string _downloadurl;
        public string DownloadUrl { get => _downloadurl; set => SetProperty(ref _downloadurl, value); }
        /// <summary>
        /// UploadType
        /// </summary>
        private string _uploadtype;
        public string UploadType { get => _uploadtype; set => SetProperty(ref _uploadtype, value); }


        /// <summary>
        /// ContentType
        /// </summary>
        private string _contenttype;
        public string ContentType { get => _contenttype; set => SetProperty(ref _contenttype, value); }

        private string _filename;
        public string FileName { get => _filename; set => SetProperty(ref _filename, value); }

        private string _fullname;
        public string FullName { get => _fullname; set => SetProperty(ref _fullname, value); }

        /// <summary>
        /// FileSize
        /// </summary>
        private long _filesize;
        public long FileSize { get => _filesize; set { SetProperty(ref _filesize, value);
                _filesizetext= ByteFormatter.ToString(_filesize);
                OnPropertyChanged(() => FileSizeText);
            } }
        private string _filesizetext;
        public string FileSizeText { get => _filesizetext; set => SetProperty(ref _filesizetext, value); }
        /// <summary>
        /// Extension
        /// </summary>
        private string _extension;
        public string Extension { get => _extension; set => SetProperty(ref _extension, value); }


        /// <summary>
        /// DisplayOrder
        /// </summary>
        private int _displayorder;
        public int DisplayOrder { get => _displayorder; set => SetProperty(ref _displayorder, value); }

        /// <summary>
        /// TenantId
        /// </summary>
        private int? _tenantId;
        public int? TenantId { get => _tenantId; set => SetProperty(ref _tenantId, value); }

        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; set => SetProperty(ref _creationTime, value); }
    }
}
